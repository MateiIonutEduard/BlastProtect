using TMPro;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Clock : MonoBehaviour
{
    public TMP_Text clock;
    private float totalTime;
    private Gameplay game;

    void Start()
    {
        totalTime = 180f;
        MyPlayerPrefs.SetGameOver(false);
        game = FindObjectOfType<Gameplay>();
    }

    public void Update()
    {
        if (!MyPlayerPrefs.IsGameOver())
        {
            if (totalTime > 0f)
                totalTime -= Time.deltaTime;
            else
            {
                totalTime = 0;
                MyPlayerPrefs.SetGameOver(true);
                game.GameEnd();
            }

            int minutes = (int)totalTime / 60;
            int seconds = (int)totalTime % 60;
            clock.text = $"{minutes}:{seconds.ToString("00")}";
        }
    }
}
