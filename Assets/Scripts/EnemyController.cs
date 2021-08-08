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

    public void Start()
    {
        bombs = FindObjectOfType<LoadMap>().BombList;
    }

    private Vector3 Round(Vector3 pos)
    {
        int xp = Mathf.RoundToInt(pos.x);
        int zp = Mathf.RoundToInt(pos.z);
        var res = new Vector3(xp, .5f, Mathf.RoundToInt(pos.z));
        return res;
    }

    private void dropBomb()
    {
        if (player.bombs > 0 && bombPrefab)
        {
            var obj = Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(transform.position.x),
             bombPrefab.transform.position.y, Mathf.RoundToInt(transform.position.z)),
             bombPrefab.transform.rotation);

            var temp = obj.GetComponent<Bomb>();
            bombs.Add(temp);

            obj.GetComponent<Bomb>().explode_size = player.explosion_power;
            obj.GetComponent<Bomb>().player = player;
        }
    }
}
