using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour
{
    public Text Power;
    public Text Speed;
    public Text Bombs;

    private PlayerUnit GetPlayer()
    {
        int id = MyPlayerPrefs.GetEnemyId();

        var player = FindObjectsOfType<PlayerUnit>()
            .FirstOrDefault(u => u.PlayerId == id);

        return player;
    }

    public void Update()
    {
        var player = GetPlayer();

        if(player != null)
        {
            Power.text = player.explosion_power.ToString();
            Speed.text = player.moveSpeed.ToString();
            Bombs.text = player.bombs.ToString();
        }
    }
}
