using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Window : MonoBehaviour
{
    public GameObject GameBoard;
    public GameObject OptionsWindow;
    private bool IsActive;

    public void Awake()
    {
        IsActive = false;
    }

    public void OnClose()
    {
        IsActive = false;
        OptionsWindow.SetActive(IsActive);
        GameBoard.SetActive(!IsActive);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsActive = !IsActive;
            OptionsWindow.SetActive(IsActive);
            GameBoard.SetActive(!IsActive);
        }
    }
}
