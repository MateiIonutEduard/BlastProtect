using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dashboard : MonoBehaviour
{
    public GameObject Board;
    public GameObject[] gameObjects;

    public void OnEnable()
    {
        DestroyWorld();
        ShowDashboard();
    }

    public void OnDisable()
    {
        for (int i = 0; i < gameObjects.Length; i++)
            gameObjects[i].SetActive(false);
    }

    private void DestroyWorld()
    {
        Board.SetActive(false);
        FindObjectOfType<Clock>().enabled = false;
        var enemies = FindObjectsOfType<PlayerUnit>();

        foreach(var enemy in enemies)
        {
            if (enemy.gameObject != null)
                Destroy(enemy.gameObject);
        }
    }

    public void QuitGame()
    {
        var scene = SceneManager.GetActiveScene();
        int id = scene.buildIndex;

        MyPlayerPrefs.SetLevel(1);
        SceneManager.LoadScene(id - 1);
    }

    private void ShowDashboard()
    {
        int players = MyPlayerPrefs.GetPlayers();
        Entity[] entities = GetPlayers();

        for(int i = 0; i < players; i++)
        {
            if (!gameObjects[i].activeSelf) gameObjects[i].SetActive(true);
            var obj = gameObjects[i].transform.GetChild(0).gameObject;

            var playerName = obj.transform.GetChild(1).gameObject.GetComponent<Text>();
            var playerScore = obj.transform.GetChild(0).gameObject.GetComponent<Text>();

            playerName.text = entities[i].PlayerName;
            playerScore.text = entities[i].Score.ToString();
        }
    }

    private Entity[] GetPlayers()
    {
        int count = MyPlayerPrefs.GetPlayers();
        byte[] scores = MyPlayerPrefs.GetDashboard();

        int playerId = MyPlayerPrefs.GetPlayerId();
        List<Entity> list = new List<Entity>();
        int[] pos = new int[count];

        for (int k = 0; k < count; k++)
            pos[k] = k;

        for(int i = 0; i < count - 1; i++)
        {
            for(int j = i + 1; j < count; j++)
            {
                if (scores[pos[i]] < scores[pos[j]])
                {
                    int t = pos[i];
                    pos[i] = pos[j];
                    pos[j] = t;
                }
            }
        }

        for(int i = 0; i < count; i++)
        {
            var obj = new Entity
            {
                PlayerName = $"Player {pos[i] + 1}",
                Score = scores[pos[i]]
            };

            if (playerId == pos[i] + 1)
                obj.PlayerName = $"Player {pos[i] + 1} (You)";

            list.Add(obj);
        }

        return list.ToArray();
    }
}
