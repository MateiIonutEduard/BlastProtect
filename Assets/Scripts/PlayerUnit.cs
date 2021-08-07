using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [Range(1, 8)]
    public int PlayerId;
    public float moveSpeed = 5f;
    public int bombs = 2;

    public bool canKick = false;
    public int explosion_power = 2;

    public bool dead = false;
    public bool respawning = false;
    public GlobalStateManager globalManager;

    public void Start()
    {
        var mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        mesh.material = mesh.materials[PlayerId - 1];
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!dead && other.CompareTag("Explosion"))
        {
            dead = true;
            globalManager.PlayerDied(PlayerId);
            Destroy(gameObject);
        }
    }
}
