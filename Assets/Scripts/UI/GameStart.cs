using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameStart : MonoBehaviour
{
    public Slider PlayerBar;
    public Slider RoundBar;

    public Text players;
    public Text rounds;

    public void OnRoundChanged()
    {
        rounds.text = $"{(int)RoundBar.value}";
    }

    public void OnPlayerChanged()
    {
        players.text = $"{(int)PlayerBar.value}";
    }
}
