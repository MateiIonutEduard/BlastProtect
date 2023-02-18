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
        int roundId = MyPlayerPrefs.GetLevel();
        int levels = MyPlayerPrefs.GetLevels();
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
        int roundId = MyPlayerPrefs.GetLevel();

        int rounds = MyPlayerPrefs.GetLevels();
        var scene = SceneManager.GetActiveScene();
        int id = scene.buildIndex;

        if (roundId < rounds)
        {
            roundId++;
            MyPlayerPrefs.SetLevel(roundId);
            SceneManager.LoadScene(id);
        }
        else
        {
            MyPlayerPrefs.SetLevel(1);
            SceneManager.LoadScene(id - 1);
        }
    }

    public void NewGame()
    {
        MyPlayerPrefs.SetLevel(1);
        var scene = SceneManager.GetActiveScene();

        int index = scene.buildIndex;
        SceneManager.LoadScene(index);
    }

    public void KillAgent(int PlayerId)
    {
        deaths++;
        int index = PlayerId - 1;
        if (!state[index]) state[index] = true;
        if (deaths >= state.Length - 1) GameEnd();
    }
}
