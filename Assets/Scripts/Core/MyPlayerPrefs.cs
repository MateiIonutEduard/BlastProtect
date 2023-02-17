using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyPlayerPrefs
{
    static float music = 0f;
    static float volume = 0f;

    public static void BeginSession()
    {
        bool force = SceneManager.GetActiveScene().buildIndex == 0;

        if (force)
        {
            if (PlayerPrefs.HasKey("music"))
                music = PlayerPrefs.GetFloat("music");
            else
            {
                if (music == 0)
                    music = .5f;
            }

            if (PlayerPrefs.HasKey("volume"))
                volume = PlayerPrefs.GetFloat("volume");
            else
            {
                if (volume == 0)
                    volume = .5f;
            }
        }
    }

    public static void SetMusic(float percent)
    {
        float ratio = (float)Math.Round(percent * 100f);
        music = ratio / 100f;
    }

    public static void SetVolume(float percent)
    {
        float ratio = (float)Math.Round(percent * 100f);
        volume = ratio / 100f;
    }

    public static float GetMusic() => music;

    public static float GetVolume() => volume;

    public static void EndSession()
    {
        if (PlayerPrefs.HasKey("music")) PlayerPrefs.DeleteKey("music");
        PlayerPrefs.SetFloat("music", music);

        if (PlayerPrefs.HasKey("volume")) PlayerPrefs.DeleteKey("volume");
        else PlayerPrefs.SetFloat("volume", volume);

        PlayerPrefs.Save();
    }
}
