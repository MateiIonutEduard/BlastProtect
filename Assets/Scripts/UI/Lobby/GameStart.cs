using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public Slider PlayerBar;
    public Slider RoundBar;
    public GameObject Menu;

    public Text players;
    public Text rounds;

    private int Players;
    private int Rounds;

    public void Start()
    {
        Players = 2;
        Rounds = 1;

        if (PlayerPrefs.HasKey("players")) PlayerPrefs.DeleteKey("players");
        if (PlayerPrefs.HasKey("rounds")) PlayerPrefs.DeleteKey("rounds");
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void OnGameStart()
    {
        PlayerPrefs.SetInt("players", Players);
        PlayerPrefs.SetInt("rounds", Rounds);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnRoundChanged()
    {
        Rounds = (int)RoundBar.value;
        rounds.text = $"{(int)RoundBar.value}";
    }

    public void OnPlayerChanged()
    {
        Players = (int)PlayerBar.value;
        players.text = $"{(int)PlayerBar.value}";
    }
}
