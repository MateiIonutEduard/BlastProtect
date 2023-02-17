using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEffect : MonoBehaviour
{
    private AudioSource musicSource;
    private float musicVolume;
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicVolume = MyPlayerPrefs.GetMusic();
        musicSource.volume = musicVolume;
    }

    void Update()
    {
        if(musicVolume != MyPlayerPrefs.GetMusic())
        {
            musicVolume = MyPlayerPrefs.GetMusic();
            musicSource.volume = musicVolume;
        }
    }
}
