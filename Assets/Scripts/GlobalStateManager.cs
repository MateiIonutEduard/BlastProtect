using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalStateManager : MonoBehaviour
{
    public List<GameObject> Players = new List<GameObject>();

    private int deadPlayers = 0;
    private int deadPlayerNumber = -1;

    public void PlayerDied(int playerNumber)
    {
        deadPlayers++;

        if (deadPlayers == 1)
        {
            deadPlayerNumber = playerNumber;
            Invoke("CheckPlayersDeath", .3f);
        }
    }

    void CheckPlayersDeath()
    {
        if (deadPlayers == 1)
        {

            if (deadPlayerNumber == 1)
            { //P1 dead, P2 is the winner
                Debug.Log("Player 2 is the winner!");
            }
            else
            { //P2 dead, P1 is the winner
                Debug.Log("Player 1 is the winner!");
            }
        }
        else
        {  //Multiple dead players, it's a draw
            Debug.Log("The game ended in a draw!");
        }
    }
}
