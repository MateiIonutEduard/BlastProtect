using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private float theta;
    public BonusType BonusType;
    public bool IsRotated;

    public void Awake()
    {
        theta = 0f;
        transform.localScale = new Vector3(.75f, .75f, .75f);
    }

    public void FixedUpdate()
    {
        float alpha = IsRotated ? -90f : 0f;
        var axis = new Vector3(alpha, theta, 0f);
        transform.rotation = Quaternion.Euler(axis);
        theta += 3f;

        if (theta >= 360) theta = 0;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(gameObject);
            var player = other.GetComponent<PlayerUnit>();

            switch(BonusType)
            {
                case BonusType.FireUp:
                        player.explosion_power++;
                    break;
                case BonusType.Bombs:
                    player.bombs++;
                    break;
                case BonusType.SpeedUp:
                    player.moveSpeed++;
                    break;
                case BonusType.Kick:
                    player.canKick = true;
                    break;
            }
        }
    }
}
