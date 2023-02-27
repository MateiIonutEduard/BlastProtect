using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadMap : MonoBehaviour
{
    public GameObject[] list;
    public GameObject Player;
    public List<Bomb> BombList;

    private GameObject blocks;
    private GameObject floor;

    public int Players;
    public int PlayerId;
    public int size = 13;

    public void Awake()
    {
        BombList = new List<Bomb>();
        blocks = transform.GetChild(0).gameObject;
        PlayerId = MyPlayerPrefs.GetPlayerId();

        floor = transform.GetChild(1).gameObject;
        Players = MyPlayerPrefs.GetPlayers();

        if (Players == 0) Application.Quit(0);
        size = Players <= 4 ? 13 : 15;
        LoadGame();
    }

    public void Start()
    {
        AddPlayers();
        FindObjectOfType<Gameplay>().GameStart();
    }

    private void AddPlayers()
    {
        int half = size >> 1;

        var array = new Vector3[] {
            new Vector3(1f, 0.5f, 1f),
            new Vector3(size - 2, 0.5f, size - 2),
            new Vector3(1f, 0.5f, size - 2),
            new Vector3(size - 2, 0.5f, 1f),

            new Vector3(half, 0.5f, half),

            new Vector3(1f, 0.5f, half),
            new Vector3(half, 0.5f, 1f),
            new Vector3(size - 2, 0.5f, half),
            new Vector3(half, 0.5f, size - 2)
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
                player.GetComponent<EnemyController>().mode = (PlayerType)Random.Range(0, 4);
            }
            else
            {
                var obj = player.GetComponent<PlayerController>();
                var follow = FindObjectOfType<FollowPlayer>();
                follow.offset = new Vector3(-1, 9, -4);
            }
        }
    }

    private Vector2[] FindLocation(Vector2 pos)
    {
        int[] v = new int[] { -1, 0, 1 };
        var list = new List<Vector2>();

        for(int i = 0; i < v.Length; i++)
        {
            for(int j = 0; j < v.Length; j++)
            {
                var point = new Vector2(pos.x + v[i], pos.y + v[j]);

                if (point.x > 0 && point.y > 0)
                    list.Add(point);
            }
        }

        return list.ToArray();
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

        MyCustomMap.CreateMap(size);

        for(int j = 0; j < size; j++)
        {
            for(int k = 0; k < size; k++)
            {
                Block block = MyCustomMap.GetBlock(j, k);
                AddBlock(block, j, k);
            }
        }
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
