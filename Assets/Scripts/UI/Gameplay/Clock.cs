using TMPro;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Clock : MonoBehaviour
{
    public TMP_Text clock;
    private float totalTime;
    public bool GameOver;

    void Start()
    {
        totalTime = 300f;
        GameOver = false;
    }

    public void Update()
    {
        if (!GameOver)
        {
            if (totalTime > 0f)
                totalTime -= Time.deltaTime;
            else
            {
                totalTime = 0;
                GameOver = true;
            }

            int minutes = (int)totalTime / 60;
            int seconds = (int)totalTime % 60;
            clock.text = $"{minutes}:{seconds.ToString("00")}";
        }
    }
}
