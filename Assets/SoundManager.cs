using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // Only ONE SoundManager in the game

    [Header("Music")]
    public AudioSource musicSource; // The speaker
    public AudioClip backgroundMusic; // The music

    private float currentVolume; // How loud the music is

    private void Awake()
    {
        // ?? FOR TESTING ONLY (Resets volume every time you press Play in Unity)
#if UNITY_EDITOR
        PlayerPrefs.DeleteKey("MusicVolume"); // Remove saved volume
#endif

        // If another SoundManager already exists, delete this one
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Keep this object when switching scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Load saved volume (default = 1 if nothing saved)
        currentVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        // Apply volume
        if (musicSource != null)
            musicSource.volume = currentVolume;

        // Play music on loop
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true; // Repeat forever
            musicSource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        currentVolume = volume;

        // Change volume
        if (musicSource != null)
            musicSource.volume = volume;

        // Save volume
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public float GetMusicVolume()
    {
        return currentVolume;
    }

    // ?? OPTIONAL: Button to reset volume manually
    public void ResetVolume()
    {
        currentVolume = 1f; // back to full volume

        if (musicSource != null)
            musicSource.volume = currentVolume;

        PlayerPrefs.SetFloat("MusicVolume", currentVolume);
    }
}