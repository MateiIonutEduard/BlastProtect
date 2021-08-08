using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMap : MonoBehaviour
{
    public GameObject[] list;
    public GameObject Player;
    public FollowPlayer Follow;

    private GameObject blocks;
    private GameObject floor;

    public int Players;
    public int PlayerId;

    public void Awake()
    {
        blocks = transform.GetChild(0).gameObject;
        floor = transform.GetChild(1).gameObject;
        LoadGame();
    }

    public void Start()
    {
        var array = new Vector3[] { 
            new Vector3(1f, 0.5f, 1f),
            new Vector3(1f, 0.5f, 11f),
            new Vector3(11f, 0.5f, 1f),
            new Vector3(11f, 0.5f, 11f)
        };

        for(int i = 1; i <= Players; i++)
        {
            var player = Instantiate(Player, array[i - 1], Quaternion.identity);
            var unit = player.GetComponent<PlayerUnit>();
            unit.PlayerId = i;

            if (i != PlayerId)
            {
                player.GetComponent<PlayerController>().enabled = false;
                player.GetComponent<EnemyController>().enabled = true;
            }
            else
            {
                var obj = player.GetComponent<PlayerController>();
                Follow.player = obj;
            }
        }
    }

    private void LoadGame()
    {
        for(int i = 0; i < 13; i++)
        {
            for(int j = 0; j < 13; j++)
            {
                var obj = Instantiate(list[2], new Vector3(i, -0.5f, j), Quaternion.identity);
                obj.transform.SetParent(floor.transform);
            }
        }

        for(int k = 0; k < 13; k++)
        {
            var pos = new Vector3(0, .5f, k);
            var obj = Instantiate(list[0], pos, Quaternion.identity);
            obj.transform.SetParent(blocks.transform);

            pos = new Vector3(12, .5f, k);
            obj = Instantiate(list[0], pos, Quaternion.identity);
            obj.transform.SetParent(blocks.transform);

            pos = new Vector3(k, .5f, 0);
            obj = Instantiate(list[0], pos, Quaternion.identity);
            obj.transform.SetParent(blocks.transform);

            pos = new Vector3(k, .5f, 12);
            obj = Instantiate(list[0], pos, Quaternion.identity);
            obj.transform.SetParent(blocks.transform);
        }

        for(int i = 2; i < 13; i += 2)
        {
            for(int j = 2; j < 13; j += 2)
            {
                var obj = Instantiate(list[0], new Vector3(i, 0.5f, j), Quaternion.identity);
                obj.transform.SetParent(blocks.transform);
            }
        }
        // create bricks...
        for(int i = 3; i < 10; i++)
        {
            var brick = Instantiate(list[1], new Vector3(i, .5f, 1f), Quaternion.identity);
            brick.transform.SetParent(blocks.transform);
        }
    }
}
