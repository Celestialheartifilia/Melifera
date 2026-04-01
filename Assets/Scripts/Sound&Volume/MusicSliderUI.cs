using UnityEngine;
using UnityEngine.UI;

public class MusicSliderUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        if (musicSlider == null)
            musicSlider = GetComponent<Slider>();

        if (SoundManager.instance == null || musicSlider == null)
            return;

        musicSlider.value = SoundManager.instance.GetMusicVolume();
        musicSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        SoundManager.instance.SetMusicVolume(value);
    }
}