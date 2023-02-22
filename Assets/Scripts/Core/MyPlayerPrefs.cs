using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyPlayerPrefs
{
    static float music = 0f;
    static float volume = 0f;

    static int level = 1;
    static int levels = 1;

    static int players;
    static bool[] state;
    static byte[] score;

    static int PlayerId;
    static int[] enemies;
    static int EnemyId;

    static bool GameOver;
    static int deaths;

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

    static void CreateDashboard()
    {
        state = new bool[players];
        score = new byte[players];
        deaths = 0;

        for (int k = 0; k < players; k++)
        {
            score[k] = 0;
            state[k] = false;
        }
    }

    public static void SetPlayerId(int playerId) => PlayerId = playerId;

    public static void SetFollowers()
    {
        enemies = new int[players];

        EnemyId = 0;
        enemies[EnemyId++] = PlayerId;
    }

    public static void SetEnemyId(int enemyId)
    {
        if (EnemyId < players) 
            enemies[EnemyId++] = enemyId;
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

    public static void SetLevel(int levelUp) => level = levelUp;

    public static void SetRounds(int rounds) => levels = rounds;

    public static void SetPlayers(int count)
    {
        players = count;
        if(level < 2) CreateDashboard();
    }

    public static bool IsGameOver() => GameOver;

    public static void SetGameOver(bool state) => GameOver = state;

    public static void IncreaseScore()
    {
        int index = -1;

        if (!GameOver)
        {
            for (int i = 0; i < state.Length; i++)
            {
                if (!state[i])
                {
                    index = i;
                    break;
                }
            }

            for (int j = 0; j < players; j++)
                state[j] = false;

            if (index != -1)
                score[index]++;
        }
    }

    public static int GetPlayerId() => PlayerId;

    public static int GetEnemyId() => enemies[EnemyId - 1];

    public static void KillAgent(Action action, int PlayerId)
    {
        deaths++;
        int index = PlayerId - 1;
        if (!state[index]) state[index] = true;

        if (deaths >= state.Length - 1)
        {
            deaths = 0;
            action();
        }
    }

    public static bool IsAlive(int playerId) => !state[playerId - 1];

    public static byte[] GetDashboard() => score;

    public static float GetMusic() => music;

    public static float GetVolume() => volume;

    public static int GetLevel() => level;

    public static int GetLevels() => levels;

    public static int GetPlayers() => players;

    public static void EndSession()
    {
        if (PlayerPrefs.HasKey("music")) PlayerPrefs.DeleteKey("music");
        PlayerPrefs.SetFloat("music", music);

        if (PlayerPrefs.HasKey("volume")) PlayerPrefs.DeleteKey("volume");
        else PlayerPrefs.SetFloat("volume", volume);

        PlayerPrefs.Save();
    }
}
