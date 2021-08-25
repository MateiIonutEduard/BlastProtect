using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public void Awake()
    {
        if (PlayerPrefs.HasKey("RoundId"))
            PlayerPrefs.DeleteKey("RoundId");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
