using UnityEngine;
using System.Collections;


public class DestroySelf : MonoBehaviour
{
    public float Delay = 3f;

    public void Start()
    {
        Destroy(gameObject, Delay);
    }
}
