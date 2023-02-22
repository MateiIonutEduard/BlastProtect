using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Gameplay : MonoBehaviour
{
    public Text round;
    public GameObject Dashboard;

    public void GameStart()
    {
        int roundId = MyPlayerPrefs.GetLevel();
        int levels = MyPlayerPrefs.GetLevels();

        round.text = $"{roundId}/{levels}";
        MyPlayerPrefs.SetFollowers();
    }

    public void GameEnd()
    {
        MyPlayerPrefs.IncreaseScore();
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
            Dashboard.SetActive(true);
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
        MyPlayerPrefs.KillAgent(GameEnd, PlayerId);
    }
}
