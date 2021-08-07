using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public GameObject[] list;

    public void OnDestroy()
    {
        Instantiate(list[3], transform.position, Quaternion.identity);

        if (Random.Range(0.0f, 1.0f) > 0.7f)
        {
            int index = Random.Range(0, 3);
            Instantiate(list[index], transform.position, Quaternion.identity);
        }
    }
}
