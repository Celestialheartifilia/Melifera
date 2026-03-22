using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // prevent a second one from spawning 
            return;
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        if (mainMixer == null) return;
        float volume = MusicSlider.value;
        mainMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        if (mainMixer == null) return;
        float volume = SFXSlider.value;
        mainMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void LoadVolume()
    {
        //MusicSlider.value = PlayerPrefs.GetFloat("musicVolume",1f);
        //SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume",1f);

        //SetMusicVolume();
        //SetSFXVolume();
        if (MusicSlider != null) MusicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
        if (SFXSlider != null) SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Still apply the volume to the mixer even if sliders aren't present
        float mVol = PlayerPrefs.GetFloat("musicVolume", 1f);
        float sVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        mainMixer.SetFloat("music", Mathf.Log10(mVol) * 20);
        mainMixer.SetFloat("sfx", Mathf.Log10(sVol) * 20);
    }
}
