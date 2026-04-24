using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip underwaterAmbience;
    public AudioClip helensTheme;

    [Header("Volume")]
    [Range(0f, 1f)] public float helensThemeVolume = 0.8f;
    [Range(0f, 1f)] public float underwaterAmbienceVolume = 0.4f;

    private AudioSource ambienceSource;
    private AudioSource themeSource;

    void Start()
    {
        // Create two separate AudioSources on this GameObject
        ambienceSource = gameObject.AddComponent<AudioSource>();
        themeSource = gameObject.AddComponent<AudioSource>();

        // Configure ambience
        ambienceSource.clip = underwaterAmbience;
        ambienceSource.loop = true;
        ambienceSource.volume = underwaterAmbienceVolume;
        ambienceSource.playOnAwake = false;
        ambienceSource.Play();

        // Configure theme
        themeSource.clip = helensTheme;
        themeSource.loop = true;
        themeSource.volume = helensThemeVolume;
        themeSource.playOnAwake = false;
        themeSource.Play();
    }
}