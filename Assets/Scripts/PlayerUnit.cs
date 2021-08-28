using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [Range(1, 9)]
    public int PlayerId;
    public float moveSpeed = 5f;
    public int bombs = 2;

    public bool canKick = false;
    public int explosion_power = 2;
    public Material[] Materials;

    public bool dead = false;
    public bool respawning = false;
    public Gameplay gameplay;

    public void Start()
    {
        gameplay = FindObjectOfType<Gameplay>();
        var mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        mesh.material = Materials[PlayerId - 1];
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!dead && other.CompareTag("Explosion"))
        {
            dead = true;
            gameplay.KillAgent(PlayerId);
            Destroy(gameObject);
        }
    }
}
