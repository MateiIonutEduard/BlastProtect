using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Gameplay : MonoBehaviour
{
    public Text round;
    private bool[] state;
    private byte[] score;
    private int deaths;

    public void GameStart()
    {
        int roundId = GetValue("RoundId");

        if(roundId == 0)
        {
            roundId = 1;
            SetValue("RoundId", roundId);
        }

        int levels = GetValue("rounds");
        round.text = $"{roundId}/{levels}";

        var agents = FindObjectsOfType<PlayerUnit>();
        state = new bool[agents.Length];
        score = new byte[state.Length];
        deaths = 0;
    }

    private void GameEnd()
    {
        int index = -1;

        for (int i = 0; i < state.Length; i++)
        {
            if (!state[i])
            {
                index = i;
                break;
            }
        }

        if (index != -1) score[index]++;
        int roundId = GetValue("RoundId");

        int rounds = GetValue("rounds");
        var scene = SceneManager.GetActiveScene();
        int id = scene.buildIndex;

        if (roundId < rounds)
        {
            roundId++;
            SetValue("RoundId", roundId);
            SceneManager.LoadScene(id);
        }
        else
        {
            DeleteValue("RoundId");
            SceneManager.LoadScene(id - 1);
        }
    }

    private void DeleteValue(string key)
    {
        if (PlayerPrefs.HasKey(key))
            PlayerPrefs.DeleteKey(key);
    }

    private int GetValue(string key)
    {
        if (!PlayerPrefs.HasKey(key)) return 0;
        return PlayerPrefs.GetInt(key);
    }

    private void SetValue(string key, int value)
    {
        if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public void KillAgent(int PlayerId)
    {
        deaths++;
        int index = PlayerId - 1;
        if (!state[index]) state[index] = true;
        if (deaths >= state.Length - 1) GameEnd();
    }
}
