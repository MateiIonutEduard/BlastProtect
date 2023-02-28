using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject bombPrefab;
    private PlayerUnit player;
    private Rigidbody rigidBody;
    private Animator animator;
    private List<Bomb> bombs;

    private List<Distance> detections;
    private Status state = Status.Idle;
    private bool dodge;

    public bool drop_bomb_on_player;
    public PlayerType mode;
    private bool on_bomb = false;

    private bool bomb;
    private bool powerup;
    private bool follow;

    private Vector3 next_pos;
    private Vector3 next_dir;
    private List<Vector3> path = new List<Vector3>();

    private struct Distance
    {
        public Vector3 dir;
        public int dist;

        public string tag;
        public int weight;
    }

    public void Start()
    {
        player = GetComponent<PlayerUnit>();
        bombs = FindObjectOfType<LoadMap>().BombList;
        
        rigidBody = GetComponent<Rigidbody>();
        animator = transform.Find("PlayerModel").GetComponent<Animator>();

        bomb = true;
        follow = true;
        powerup = true;
        dodge = true;
    }

    public void Update()
    {
        animator.SetBool("Walking", false);

        switch (state)
        {
            case Status.Dodge:
                MovePlayer(next_dir, next_pos);
                break;
            case Status.Center:
                MovePlayer(next_dir, Round(transform.position));
                break;
            case Status.FollowPlayer:
                MovePath();
                break;
            case Status.Bomb:
                DropBomb();
                on_bomb = true;
                state = Status.Idle;
                break;

            case Status.Idle:

                detections = FindNearCollisions(transform.position);

                if (dodge)
                {
                    if (HasTag(detections, "Bomb") || on_bomb)
                    {
                        on_bomb = false;
                        NextDodgeLocation(detections);

                        state = Status.Dodge;
                        break;
                    }
                    else
                    {
                        if (bombs.Count > 0 && !HasBombsActive()) 
                            bombs.Clear();
                    }
                }

                if (powerup)
                {
                    if (HasTag(detections, "PowerUp"))
                    {
                        int count = detections.Count;

                        for(int k = 0; k < count; k++)
                        {
                            if (string.Compare(detections[k].tag, "PowerUp") == 0)
                            {
                                path.Clear();
                                path.Add(transform.position + detections[k].dir);
                                state = Status.FollowPlayer;
                                break;
                            }
                        }
                    }
                }

                if (bomb)
                {
                    if (player.bombs != 0 && CanPlaceBomb())
                    {
                        state = Status.Bomb;
                        break;
                    }
                }

                if (follow)
                {
                    if (path.Count == 0)
                    {
                        switch (mode)
                        {
                            case PlayerType.Aggressive:
                                var src = FindObjectsOfType<PlayerController>();
                                int len = src.Length;

                                for (int j = 0; j < len; j++)
                                {
                                    if (src[j].isActiveAndEnabled)
                                    {
                                        path = FollowLocation(transform.position, Round(src[j].transform.position));
                                        break;
                                    }
                                }

                                break;
                            case PlayerType.Random:
                                path = FollowLocation(transform.position, FindRandomWalk());
                                break;
                            case PlayerType.Farm:

                                path = GetClosestBreakable(Round(transform.position), detections);

                                break;
                            case PlayerType.Survivor:
                                path = GetSafePosition(transform.position);
                                break;
                        }

                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, path[0]) > 1)
                            path.Clear();
                        else
                        {
                            state = Status.FollowPlayer;
                            break;
                        }
                    }
                }

                if (transform.position != Round(transform.position))
                    state = Status.Center;

                break;
        }
    }

    private bool HasBombsActive()
    {
        int len = bombs.Count;

        for (int i = 0; i < len; i++)
            if (bombs[i] != null) return true;

        return false;
    }

    private bool FindLastCollide(Vector3 pos_test)
    {
        int len = bombs.Count;

        if (len > 0)
        {
            for (int i = 0; i < len; i++)
            {
                if ((bombs[i] != null) && (bombs[i].transform.position.x == pos_test.x || bombs[i].transform.position.z == pos_test.z))
                    return true;
            }
        }

        return false;
    }

    private List<Vector3> GetClosestBreakable(Vector3 position, List<Distance> det)
    {
        List<Vector3> p = new List<Vector3>();
        Distance next = new Distance();
        bool found = false;

        List<Distance> t = det;
        int len = t.Count;

        for(int k = 0; k < len; k++)
        {
            if (string.Compare(t[k].tag, "Breakable") == 0)
            {
                if (t[k].dist < next.dist)
                {
                    next = t[k];
                    found = true;
                }
            }
        }

        if (found)
            p.Add(position + Round(next.dir));
        else
        {
            for(int j = 0; j < len; j++)
            {
                if (t[j].dist >= 1)
                {
                    next = t[j];
                    found = true;
                }
            }

            p.Add(position + Round(next.dir));
        }

        return p;
    }

    private List<Vector3> FollowLocation(Vector3 startpos, Vector3 goal)
    {
        bool done = false;
        List<Vector3> result = new List<Vector3>();
        Vector3 currentpos = startpos;

        var temp = FindObjectOfType<LoadMap>();
        Vector3 old_pos = startpos;

        while (!done)
        {
            if (currentpos == goal)
            {
                done = true;
                continue;
            }
            old_pos = currentpos;
            foreach (Distance d in FindNearCollisions(currentpos))
            {
                Vector3 temp_pos = Round(currentpos + d.dir);
                if (MyCustomMap.CanWalk((int)temp_pos.x, (int)temp_pos.z))
                {
                    if (Math.Abs(goal.x - temp_pos.x) < Math.Abs(goal.x - currentpos.x) ||
                    Math.Abs(goal.z - temp_pos.z) < Math.Abs(goal.z - currentpos.z))
                    {

                        result.Add(currentpos);
                        currentpos = temp_pos;
                        break;
                    }
                }
            }

            if (currentpos == old_pos)
                done = true;
        }

        return result;
    }

    private Vector3 FindRandomWalk()
    {
        var m = FindObjectOfType<LoadMap>();
        Vector3 res = Vector3.zero;
        bool found = false;

        while (!found)
        {
            Vector3 test = new Vector3(UnityEngine.Random.Range(1, m.size - 1), 0, UnityEngine.Random.Range(1, m.size - 1));

            if (MyCustomMap.CanWalk((int)test.x, (int)test.z))
            {
                found = true;
                res = test;
            }
        }

        return res;
    }

    private List<Vector3> GetSafePosition(Vector3 pos)
    {
        List<Vector3> result = new List<Vector3>();
        Vector3 location = pos;

        List<Vector3> list = new List<Vector3>();
        bool safePos = false;

        while (!safePos)
        {
            var buffer = FindNearCollisions(location);

            if (GetWays(buffer) == 4)
            {
                safePos = true;
                break;
            }

            bool found = false;

            foreach (Distance d in buffer)
            {
                if (d.dist >= 1)
                {
                    if (!list.Contains(location + d.dir))
                    {
                        location += d.dir;
                        list.Add(location);
                        result.Add(location);

                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                if (result.Count > 1)
                {
                    result.RemoveAt(result.Count - 1);
                    location = result[result.Count - 1];
                }

                if(result.Count <= 1) break;
            }
        }

        return result;
    }

    private void MovePath()
    {

        if (path.Count != 0)
        {
            Vector3 direction = (path[0] - Round(transform.position)).normalized;
            var temp = FindNearCollisions(path[0]);

            var temp2 = FindNearCollisions(transform.position);
            bool temp2_res = false;

            foreach(Distance d in temp2)
            {
                if (d.dir == direction && d.dist == 0 && d.tag != "PowerUp")
                    temp2_res = true;
            }

            if (!HasTag(temp, "Bomb") && !HasTag(temp, "Explosion") && !temp2_res)
                MovePlayer(direction, path[0]);
            else
            {
                if (transform.position != Round(transform.position))
                    state = Status.Center;
                else
                    state = Status.Idle;
            }
            
            if (Vector3.Distance(transform.position, path[0]) == 0)
                path.RemoveAt(0);
        }
        else
            state = Status.Idle;
    }

    private void TestBomb()
    {
        if (dodge)
        {
            detections = FindNearCollisions(transform.position);

            if (HasTag(detections, "Bomb") || on_bomb)
            {
                on_bomb = false;
                NextDodgeLocation(detections);
                state = Status.Dodge;
            }
        }
    }

    private ArrayList FindEscapePath(Vector3 dest, Vector3 src)
    {
        ArrayList result = new ArrayList();
        Vector3 location = src;
        ArrayList all = new ArrayList();
        bool safePos = false;

        while (!safePos)
        {
            if (dest.x != location.x && dest.z != location.z)
            {
                safePos = true;
                break;
            }

            var distances = FindNearCollisions(location);
            bool found = false;

            for(int j = 0; j < distances.Count; j++)
            {
                Distance d = distances[j];

                if (d.dist >= 1 && !FindLastCollide(location + d.dir) && !all.Contains(location + d.dir))
                {
                    location += d.dir;
                    all.Add(location);
                    result.Add(location);

                    found = true;
                    break;
                }
            }

            if (!found)
            {
                if (result.Count > 1)
                {
                    result.RemoveAt(result.Count - 1);
                    location = (Vector3)result[result.Count - 1];
                }
                if (result.Count <= 1) break;
            }
        }

        return result;
    }

    private bool CanPlaceBomb()
    {
        detections = FindNearCollisions(transform.position);

        for (int k = 0; k < detections.Count; k++)
        {
            Distance d = detections[k];

            if (d.tag == "Breakable" || (d.tag == "Player" && drop_bomb_on_player))
            {
                if (d.dist < player.explosion_power)
                {
                    ArrayList temp = FindEscapePath(transform.position, transform.position);
                    if (temp.Count >= 2) return true;
                }
            }
        }

        return false;
    }

    private int GetWays(List<Distance> detections)
    {
        int i = 0;

        for(int k = 0; k < detections.Count; k++)
        {
            Distance d = detections[k];
            if (d.dist > 0) i++;
        }

        return i;
    }

    private void NextDodgeLocation(List<Distance> buffer)
    {
        Distance temp = new Distance();
        bool found = false;
        int counter = 0;

        while (!found)
        {
            if (counter > 4)
            {
                found = true;
                continue;
            }

            counter++;


            for(int j = 0; j < buffer.Count; j++)
            {
                Distance d = buffer[j];
                if (d.weight > temp.weight) temp = d;
            }

            next_pos = transform.position + temp.dir;
            next_dir = temp.dir;

            var array = FindNearCollisions(next_pos);

            if (HasTag(array, "Bomb"))
            {
                List<Distance> list = new List<Distance>();
                Distance t = new Distance();

                for(int i = 0; i < buffer.Count; i++)
                {
                    Distance d = buffer[i];

                    if (d.dir == next_dir)
                    {
                        t = d;
                        t.weight = 1 / 2;
                    }
                    else t = d;

                    list.Add(t);
                }

                buffer = list;
            }
            else
                found = true;
        }
    }

    private void MovePlayer(Vector3 direction, Vector3 position)
    {
        Vector3 movePosition = Vector3.MoveTowards(transform.position, position, (player.moveSpeed / 2) * Time.deltaTime);
        transform.position = movePosition;

        if (direction == Vector3.forward)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction == Vector3.back)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (direction == Vector3.left)
            transform.rotation = Quaternion.Euler(0, 270, 0);
        else if (direction == Vector3.right)
            transform.rotation = Quaternion.Euler(0, 90, 0);

        animator.SetBool("Walking", true);

        if (Vector3.Distance(transform.position, position) == 0)
            state = Status.Idle;
    }

    private bool HasTag(List<Distance> list, string tag)
    {
        int len = list.Count;

        for (int k = 0; k < len; k++)
            if (string.Compare(list[k].tag, tag) == 0)
                return true;

        return false;
    }

    private List<Distance> FindNearCollisions(Vector3 pos)
    {
        var temp = new List<Distance>();
        temp.Add(DetectCollision(Vector3.forward, pos));
        temp.Add(DetectCollision(Vector3.back, pos));

        temp.Add(DetectCollision(Vector3.left, pos));
        temp.Add(DetectCollision(Vector3.right, pos));
        return temp;
    }

    private Distance DetectCollision(Vector3 direction, Vector3 position)
    {
        Distance result = new Distance();
        bool collide = false;

        int i = 1;
        RaycastHit hit;

        while (!collide)
        {
            Physics.Raycast(position, direction, out hit, i);

            if (hit.collider)
            {
                collide = true;
                result.dir = direction;
                result.tag = hit.collider.tag;

                result.dist = i - 1;
                result.weight = result.dist;

                if (string.Compare(result.tag, "Bomb") == 0)
                {
                    if (result.dist > 0)
                    {
                        result.weight = 1 / 2;
                        var obj = hit.collider.gameObject.GetComponent<Bomb>();

                        if (!bombs.Contains(obj))
                            bombs.Add(obj);
                    }
                }
            }

            i++;
        }

        return result;
    }

    private Vector3 Round(Vector3 pos)
    {
        int xp = Mathf.RoundToInt(pos.x);
        int zp = Mathf.RoundToInt(pos.z);
        var res = new Vector3(xp, .5f, Mathf.RoundToInt(pos.z));
        return res;
    }

    private void DropBomb()
    {
        if (player.bombs > 0 && bombPrefab)
        {
            var obj = Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(transform.position.x),
             bombPrefab.transform.position.y, Mathf.RoundToInt(transform.position.z)),
             bombPrefab.transform.rotation);

            player.bombs--;
            var temp = obj.GetComponent<Bomb>();
            temp.PlayerId = player.PlayerId;
            bombs.Add(temp);

            obj.GetComponent<Bomb>().explode_size = player.explosion_power;
            obj.GetComponent<Bomb>().player = player;
        }
    }
}
