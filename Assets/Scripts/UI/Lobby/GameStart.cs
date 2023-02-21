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
        MyPlayerPrefs.SetPlayers(Players);
        MyPlayerPrefs.SetRounds(Rounds);
        SceneManager.LoadScene(1);
    }

    public void InitGame()
    {
        int playerId = Random.Range(0, Players) + 1;
        MyPlayerPrefs.SetPlayers(Players);

        MyPlayerPrefs.SetPlayerId(playerId);
        MyPlayerPrefs.SetRounds(Rounds);
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
