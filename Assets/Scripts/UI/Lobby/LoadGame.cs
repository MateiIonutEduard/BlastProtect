using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LoadGame : MonoBehaviour
{
    public Text text;
    private AsyncOperation loadingOperation;
    private Slider progressBar;

    public void Start()
    {
        loadingOperation = SceneManager.LoadSceneAsync(1);
        progressBar = GetComponentInChildren<Slider>();
    }

    public void Update()
    {
        float val = Mathf.Clamp01(loadingOperation.progress / 0.9f);
        text.text = $"loading: {(int)(val * 100f)}%";
        progressBar.value = val;

    }
}
