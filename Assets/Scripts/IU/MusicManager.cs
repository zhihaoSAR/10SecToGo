using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class MusicManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider Music;
    public Slider SFX;
    private void Start()
    {
        setMusicVolume(PlayerPrefs.GetFloat("Music", 0));
        setSFXVolume(PlayerPrefs.GetFloat("SFX", 0));
        Music.value = PlayerPrefs.GetFloat("Music", 0);
        SFX.value = PlayerPrefs.GetFloat("SFX", 0);
    }
    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }
    public void setSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
    }
    private void OnDisable()
    {
        float music = 0;
        float sfx = 0;
        audioMixer.GetFloat("Music", out music);
        audioMixer.GetFloat("SFX", out sfx);
        PlayerPrefs.SetFloat("Music",music);
        PlayerPrefs.SetFloat("SFX", sfx);
        PlayerPrefs.Save();
    }
}
