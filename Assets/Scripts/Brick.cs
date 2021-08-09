using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public GameObject[] list;
    public bool Collide;

    public void OnBecameInvisible()
    {
        if (Collide)
        {
            Instantiate(list[3], transform.position, Quaternion.identity);
            if (Random.Range(0.0f, 1.0f) > 0.5f)
            {
                int index = Random.Range(0, 3);
                Instantiate(list[index], transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
