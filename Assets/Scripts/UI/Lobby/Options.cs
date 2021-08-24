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
        music = GetValue("music");
        volume = GetValue("volume");

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

    public void OnMusicEnable()
    {
        if (music != 0f) music = -music;
        else music = GetValue("music");
        music_enabled = music > 0f;
        SetState(0);
    }

    public void OnMusicChanged()
    {
        if (!music_enabled)
        {
            music = sliders[0].value;
            UpdateState(0);
        }
        else
        {
            music = GetValue("music");
            SetState(0);
        }
    }

    public void OnVolumeEnable()
    {
        if (volume != 0f) volume = -volume;
        else volume = GetValue("volume");
        volume_enabled = volume > 0f;
        SetState(1);
    }

    public void OnVolumeChanged()
    {
        if (volume_enabled)
        {
            volume = sliders[1].value;
            UpdateState(1);
        }
        else
        {
            volume = GetValue("volume");
            SetState(1);
        }
    }

    private void UpdateState(int index)
    {
        if (index == 0)
        {
            all[index].image.sprite = music <= 0f ? list[1] : list[0];
            SetValue("music", music);
        }
        else
        {
            all[index].image.sprite = volume <= 0f ? list[3] : list[2];
            SetValue("volume", volume);
        }
    }

    private void SetState(int index)
    {
        if (index == 0)
        {
            all[index].image.sprite = music <= 0f ? list[1] : list[0];
            sliders[index].value = music <= 0f ? 0f : music;
            SetValue("music", music);
        }
        else
        {
            all[index].image.sprite = volume <= 0f ? list[3] : list[2];
            sliders[index].value = volume <= 0f ? 0f : volume;
            SetValue("volume", volume);
        }
    }

    private float GetValue(string key)
    {
        if (!PlayerPrefs.HasKey(key)) return .5f;
        else return PlayerPrefs.GetFloat(key);
    }

    private void SetValue(string key, float val)
    {
        if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
        PlayerPrefs.SetFloat(key, val);
        PlayerPrefs.Save();
    }
}
