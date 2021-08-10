using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerExit : MonoBehaviour
{
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            GetComponent<SphereCollider>().isTrigger = false;
    }
}
