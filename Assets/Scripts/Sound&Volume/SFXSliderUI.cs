using UnityEngine;
using UnityEngine.UI;

public class SFXSliderUI : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (sfxSlider == null)
            sfxSlider = GetComponent<Slider>();

        if (SoundManager.instance == null)
            return;

        sfxSlider.value = SoundManager.instance.GetSFXVolume();
        sfxSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        SoundManager.instance.SetSFXVolume(value);
    }
}