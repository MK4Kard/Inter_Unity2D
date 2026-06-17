using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Видео")]
    public bool isFullscreen = true;
    public int screenWidth = 1920;
    public int screenHeight = 1080;

    [Header("Текст")]
    [Range(0.01f, 0.2f)]
    public float textSpeed = 0.05f;

    [Header("Аудио")]
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.8f;

    // События для обновления других скриптов
    public static event Action<float> OnTextSpeedChanged;
    public static event Action<bool> OnFullscreenChanged;
    public static event Action<float> OnMusicVolumeChanged;
    public static event Action<float> OnSFXVolumeChanged;

    // Путь для сохранения
    private static string SavePath => 
        Path.Combine(Application.persistentDataPath, "settings.json");

    // Сохранить настройки
    public void Save()
    {
        try
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"Настройки сохранены: {SavePath}");
        }
        catch (Exception e)
        {
            Debug.Log($"Ошибка сохранения настроек: {e.Message}");
        }
    }

    // Загрузить настройки
    public void Load()
    {
        try
        {
            string json = File.ReadAllText(SavePath);
            JsonUtility.FromJsonOverwrite(json, this);
            Debug.Log($"Настройки загружены: {SavePath}");
            ApplySettings();
        }
        catch (Exception e)
        {
            Debug.Log($"Ошибка загрузки настроек: {e.Message}");
        }
    }

    // Применить настройки
    public void ApplySettings()
    {
        // Применяем полноэкранный режим
        Screen.fullScreen = isFullscreen;
        if (!isFullscreen)
            Screen.SetResolution(screenWidth, screenHeight, false);

        // Оповещаем о изменениях
        OnTextSpeedChanged?.Invoke(textSpeed);
        OnFullscreenChanged?.Invoke(isFullscreen);
        OnMusicVolumeChanged?.Invoke(musicVolume);
        OnSFXVolumeChanged?.Invoke(sfxVolume);
    }

    // Методы для изменения настроек
    public void SetFullscreen(bool isFull)
    {
        isFullscreen = isFull;
        Screen.fullScreen = isFullscreen;
        OnFullscreenChanged?.Invoke(isFullscreen);
    }

    public void SetTextSpeed(float speed)
    {
        textSpeed = speed;
        OnTextSpeedChanged?.Invoke(speed);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        OnMusicVolumeChanged?.Invoke(musicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        OnSFXVolumeChanged?.Invoke(sfxVolume);
    }
}
