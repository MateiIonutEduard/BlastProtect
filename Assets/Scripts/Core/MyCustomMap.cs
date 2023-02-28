using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCustomMap
{
    static PowerUp[] PowersUp;
    static Block[] map;
    static int size;

    public static void CreateMap(int mapSize)
    {
        size = mapSize;
        int half = size >> 1;

        map = new Block[size * size];
        int players = MyPlayerPrefs.GetPlayers();

        var temp = new Vector2[]
        {
            new Vector2(1f, 1f),
            new Vector2(size - 2, size - 2),
            new Vector2(1f, size - 2),
            new Vector2(size - 2, 1f),

            new Vector2(half, half),

            new Vector2(1f, half),
            new Vector2(half, 1f),
            new Vector2(size - 2, half),
            new Vector2(half, size - 2),
        };

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int index = i * size + j;
                map[index] = Block.Breakable;
            }
        }

        for (int i = 0; i < players; i++)
        {
            var list = FindLocation(temp[i]);

            for (int j = 0; j < list.Length; j++)
            {
                int x = Mathf.RoundToInt(list[j].x);
                int y = Mathf.RoundToInt(list[j].y);
                map[x * size + y] = Block.Born;
            }
        }

        for (int i = 0; i < size; i++)
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

        GenItems(mapSize);
    }

    static void GenItems(int mapSize)
    {
        if(mapSize == 15)
            GenItems(new int[] { 14, 10, 12 }, 2);
        else
            GenItems(new int[] { 12, 8, 10 }, 2);
    }

    static void GenItems(int[] powersUp, int distance)
    {
        int count = 0;
        int len = powersUp.Length;

        var list = new List<PowerUp>();
        bool done = false;

        var items = new Queue<BonusType>();
        int[] g = new int[len];

        for (int i = 0; i < len; i++)
        {
            g[i] = powersUp[i];
            count += g[i];
        }

        while(count > 0)
        {
            int index = Random.Range(0, (int)BonusType.Bombs);

            if (g[index] > 0)
            {
                items.Enqueue((BonusType)(index + 1));
                g[index]--;
                count--;
            }
        }

        while(items.Count > 0)
        {
            int dx = Random.Range(0, size);
            int dy = Random.Range(0, size);

            int index = dx * size + dy;
             done = false;

            do
            {
                if (map[index] == Block.Breakable)
                {
                    done = true;

                    for(int j = 0; j < list.Count; j++)
                    {
                        int diff = (int)Mathf.Abs(list[j].Location.x - dx);
                        diff += (int)Mathf.Abs(list[j].Location.z - dy);

                        if (diff < distance)
                        {
                            done = false;
                            break;
                        }
                    }

                    if(done)
                    {
                        BonusType bonusType = items.Dequeue();

                        var powerUp = new PowerUp
                        {
                            BonusType = bonusType,
                            Location = new Vector3(dx, .5f, dy)
                        };

                        list.Add(powerUp);
                    }
                }

                dx = Random.Range(0, size);
                dy = Random.Range(0, size);
                index = dx * size + dy;
            }
            while (!done);
        }

        PowersUp = list.ToArray();
    }

    public static int GetItem(Vector3 position)
    {
        int index = -1;

        for(int i = 0; i < PowersUp.Length; i++)
        {
            if(Vector3.Distance(position, PowersUp[i].Location) == 0)
            {
                index = i;
                break;
            }
        }

        return index > 0 ? (int)PowersUp[index].BonusType : -1;
    }

    public static Block GetBlock(int j, int k)
    {
        int index = j * size + k;
        return map[index];
    }

    public static bool CanWalk(int j, int k)
    {
        int index = size * j + k;
        return map[index] != Block.Wall;
    }

    static Vector2[] FindLocation(Vector2 pos)
    {
        int[] v = new int[] { -1, 0, 1 };
        var list = new List<Vector2>();

        for (int i = 0; i < v.Length; i++)
        {
            for (int j = 0; j < v.Length; j++)
            {
                var point = new Vector2(pos.x + v[i], pos.y + v[j]);

                if (point.x > 0 && point.y > 0)
                    list.Add(point);
            }
        }

        return list.ToArray();
    }
}
