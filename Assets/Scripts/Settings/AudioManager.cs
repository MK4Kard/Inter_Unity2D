using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Настройки")]
    public GameSettings settings;

    // Параметры в Audio Mixer
    private const string MUSIC_PARAM = "MusicVolume";
    private const string SFX_PARAM = "SFXVolume";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (settings == null)
            settings = Resources.Load<GameSettings>("GameSettings");

        // Подписываемся на события
        GameSettings.OnMusicVolumeChanged += SetMusicVolume;
        GameSettings.OnSFXVolumeChanged += SetSFXVolume;

        // Применяем текущие настройки
        SetMusicVolume(settings.musicVolume);
        SetSFXVolume(settings.sfxVolume);
    }

    void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(MUSIC_PARAM, LinearToDecibel(volume));
    }

    void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(SFX_PARAM, LinearToDecibel(volume));
    }

    float LinearToDecibel(float linear)
    {
        return linear > 0.0001f ? 20f * Mathf.Log10(linear) : -80f;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    void OnDestroy()
    {
        GameSettings.OnMusicVolumeChanged -= SetMusicVolume;
        GameSettings.OnSFXVolumeChanged -= SetSFXVolume;
    }
}
