using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMap : MonoBehaviour
{
    public GameObject[] list;
    public GameObject Player;
    public List<Bomb> BombList;

    private GameObject blocks;
    private GameObject floor;

    public int Players;
    public int PlayerId;

    private Block[] map;
    public int size = 13;

    public void Awake()
    {
        BombList = new List<Bomb>();
        blocks = transform.GetChild(0).gameObject;
        floor = transform.GetChild(1).gameObject;
        LoadGame();
    }

    public void Start()
    {
        AddPlayers();
    }

    private void AddPlayers()
    {
        var array = new Vector3[] {
            new Vector3(1f, 0.5f, 1f),
            new Vector3(1f, 0.5f, size - 2),
            new Vector3(size - 2, 0.5f, 1f),
            new Vector3(size - 2, 0.5f, size - 2)
        };

        for (int i = 1; i <= Players; i++)
        {
            var player = Instantiate(Player, array[i - 1], Quaternion.identity);
            var unit = player.GetComponent<PlayerUnit>();
            unit.PlayerId = i;

            if ((i & 1) == 0)
                player.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

            if (i != PlayerId)
            {
                player.GetComponent<PlayerController>().enabled = false;
                player.GetComponent<EnemyController>().enabled = true;
            }
            else
            {
                var obj = player.GetComponent<PlayerController>();
                var follow = FindObjectOfType<FollowPlayer>();
                follow.player = obj;
                follow.offset = new Vector3(-1, 9, -4);
            }
        }
    }

    private void LoadGame()
    {
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                var obj = Instantiate(list[2], new Vector3(i, -0.5f, j), Quaternion.identity);
                obj.transform.SetParent(floor.transform);
            }
        }

        map = new Block[size * size];
        var temp = new Vector2[]
        {
            new Vector2(1f, 1f),
            new Vector2(1f, 2f),
            new Vector2(2f, 1f),

            new Vector2(1f, size - 2),
            new Vector2(1f, size - 3),
            new Vector2(2f, size - 2),

            new Vector2(size - 2, 1f),
            new Vector2(size - 2, 2f),
            new Vector2(size - 3,  1f),

            new Vector2(size - 2, size - 2),
            new Vector2(size - 2, size - 3),
            new Vector2(size - 3, size - 2)
        };

        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                int index = i * size + j;
                map[index] = Block.Breakable;
            }    
        }

        for(int i = 0; i < temp.Length; i++)
        {
            int x = Mathf.RoundToInt(temp[i].x);
            int y = Mathf.RoundToInt(temp[i].y);
            map[x * size + y] = Block.Born;
        }

        for(int i = 0; i < size; i++)
        {
            map[i] = Block.Wall;
            map[(size - 1) * size + i] = Block.Wall;
            map[i * size] = Block.Wall;
            map[i * size + size - 1] = Block.Wall;
        }

        for (int i = 2; i < size; i += 2)
        {
            for (int j = 2; j < size; j += 2)
            {
                int index = i * size + j;
                map[index] = Block.Wall;
            }
        }

        for(int j = 0; j < size; j++)
        {
            for(int k = 0; k < size; k++)
            {
                int index = j * size + k;
                AddBlock(map[index], j, k);
            }
        }
    }

    public bool CanWalk(int j, int k)
    {
        int index = size * j + k;
        return map[index] != Block.Wall;
    }

    private void AddBlock(Block block, int i, int j)
    {
        if(block == Block.Wall)
        {
            var obj = Instantiate(list[0], new Vector3(i, .5f, j), Quaternion.identity);
            obj.transform.SetParent(blocks.transform);
        }
        else
            if(block == Block.Breakable)
        {
            var brick = Instantiate(list[1], new Vector3(i, .5f, j), Quaternion.identity);
            brick.transform.SetParent(blocks.transform);
        }
    }
}
