using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Options : MonoBehaviour
{
    public Sprite[] list;
    public Button[] all;
    public Slider[] sliders;

    private float music;
    private float volume;

    private bool music_enabled;
    private bool volume_enabled;

    public void Start()
    {
        MyPlayerPrefs.BeginSession();
        music = MyPlayerPrefs.GetMusic();
        volume = MyPlayerPrefs.GetVolume();

        music_enabled = music > 0f;
        volume_enabled = volume > 0f;

        SetState(0);
        SetState(1);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            var obj = transform.parent.GetChild(0);
            obj.gameObject.SetActive(true);
        }
    }

    public void OnMusicChanged()
    {
        music = sliders[0].value;
        UpdateState(0);
    }

    public void OnVolumeChanged()
    {
        volume = sliders[1].value;
        UpdateState(1);
    }

    private void UpdateState(int index)
    {
        if (index == 0)
        {
            all[index].image.sprite = music <= 0f ? list[1] : list[0];
            MyPlayerPrefs.SetMusic(music);
        }
        else
        {
            all[index].image.sprite = volume <= 0f ? list[3] : list[2];
            MyPlayerPrefs.SetVolume(volume);
        }
    }

    private void SetState(int index)
    {
        if (index == 0)
        {
            all[index].image.sprite = music <= 0f ? list[1] : list[0];
            sliders[index].value = music <= 0f ? 0f : music;
            MyPlayerPrefs.SetMusic(music);
        }
        else
        {
            all[index].image.sprite = volume <= 0f ? list[3] : list[2];
            sliders[index].value = volume <= 0f ? 0f : volume;
            MyPlayerPrefs.SetVolume(volume);
        }
    }
}
