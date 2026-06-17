using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Настройки")]
    public GameSettings settings;

    [Header("UI - Видео")]
    public Button fullscreenBtn;
    public Button windowscreenBtn;

    [Header("UI - Текст")]
    public Slider textSpeedSlider;
    public TMP_Text textSpeedValueText;

    [Header("UI - Аудио")]
    public Slider musicVolumeSlider;
    public TMP_Text musicVolumeText;
    public Slider sfxVolumeSlider;
    public TMP_Text sfxVolumeText;

    [Header("Кнопки")]
    public Button backBtn;
    public Button backToMenuBtn;
    public Button applyBtn;

    [Header("Звуки")]
    public AudioClip btnClickSound;

    [Header("Настройки шрифта")]
    public TMP_FontAsset customFont;
    public FontStyles fontStyle = FontStyles.Normal;
    public float fontSize = 150;

    private bool settingsChanged = false;
    private bool isFullscreen = true;

    void Start()
    {
        // Загружаем настройки
        if (settings == null)
            settings = Resources.Load<GameSettings>("GameSettings");

        settings.Load();

        // Инициализируем UI
        InitializeUI();

        // Назначаем обработчики
        SetupListeners();
    }

    void InitializeUI()
    {
        // Видео
        isFullscreen = settings.isFullscreen;
        UpdateScreenButtons();

        // Текст
        if (textSpeedSlider != null)
        {
            textSpeedSlider.value = settings.textSpeed;
            UpdateTextSpeedText(settings.textSpeed);
        }

        // Аудио
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = settings.musicVolume;
            UpdateVolumeText(musicVolumeText, settings.musicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = settings.sfxVolume;
            UpdateVolumeText(sfxVolumeText, settings.sfxVolume);
        }
    }

    void SetupListeners()
    {
        // Видео
        if (fullscreenBtn != null)
            fullscreenBtn.onClick.AddListener(() => SetScreenMode(true));

        if (windowscreenBtn != null)
            windowscreenBtn.onClick.AddListener(() => SetScreenMode(false));

        // Текст
        if (textSpeedSlider != null)
            textSpeedSlider.onValueChanged.AddListener(OnTextSpeedChanged);

        // Аудио
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        // Кнопки
        if (backBtn != null)
            backBtn.onClick.AddListener(BackToGame);

        if (applyBtn != null)
            applyBtn.onClick.AddListener(ApplySettings);

        if (backToMenuBtn != null)
            backToMenuBtn.onClick.AddListener(BackToMainMenu);
    }

    void SetScreenMode(bool fullscreen)
    {
        settingsChanged = true;
        isFullscreen = fullscreen;
        UpdateScreenButtons();
        PlayClickSound();

        Debug.Log($"Режим экрана выбран: {(fullscreen ? "Fullscreen" : "Windowed")}");
    }

    void UpdateScreenButtons()
    {
        // Обновляем состояние кнопок
        if (fullscreenBtn != null)
            fullscreenBtn.interactable = !isFullscreen; // Активна если не выбрана

        if (windowscreenBtn != null)
            windowscreenBtn.interactable = isFullscreen; // Активна если не выбрана
    }

    void OnTextSpeedChanged(float value)
    {
        settingsChanged = true;
        UpdateTextSpeedText(value);
        PlayClickSound();
    }

    void OnMusicVolumeChanged(float value)
    {
        settingsChanged = true;
        UpdateVolumeText(musicVolumeText, value);
        settings.SetMusicVolume(value);
        PlayClickSound();
    }

    void OnSFXVolumeChanged(float value)
    {
        settingsChanged = true;
        UpdateVolumeText(sfxVolumeText, value);
        settings.SetSFXVolume(value);
        PlayClickSound();
    }

    void UpdateTextSpeedText(float speed)
    {
        if (textSpeedValueText != null)
        {
            // (0.01 = 5 %, 0.2 = 100 %)
            int percent = Mathf.RoundToInt(((speed - 0.01f) / 0.19f) * 100f);
            textSpeedValueText.text = $"{percent}%";
        }
    }

    void UpdateVolumeText(TMP_Text text, float value)
    {
        if (text != null)
        {
            int percent = Mathf.RoundToInt(value * 100f);
            text.text = $"{percent}%";
        }
    }

    void BackToGame()
    {
        PlayClickSound();

        // Загружаем последние сохраненные настройки перед выходом
        settings.Load();
        settings.ApplySettings();

        InitializeUI();
    }

    void ApplySettings()
    {
        PlayClickSound();

        if (settingsChanged)
        {
            // Применяем видео настройки
            settings.SetFullscreen(isFullscreen);

            // Применяем настройки текста
            settings.SetTextSpeed(textSpeedSlider.value);

            // Сохраняем все
            settings.Save();
            settings.ApplySettings();

            settingsChanged = false;
            Debug.Log("Настройки применены и сохранены!");

            // Показываем сообщение
            StartCoroutine(ShowApplyMessage());
        }
    }

    void BackToMainMenu()
    {
        PlayClickSound();

        // Загружаем последние сохраненные настройки перед выходом
        settings.Load();
        settings.ApplySettings();
        InitializeUI();

        var sceneChanger = GameObject.Find("StartGame").GetComponent<SceneChanger>();
        if (sceneChanger != null)
            sceneChanger.levelToLoad = 0;
        else
            Debug.LogWarning("SceneChanger не найден!");
    }

    void PlayClickSound()
    {
        if (btnClickSound != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(btnClickSound);
    }

    IEnumerator ShowApplyMessage()
    {
        // Создаем временное сообщение
        GameObject messageObj = new GameObject("ApplyMessage");
        messageObj.transform.SetParent(transform);

        var rect = messageObj.AddComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, 200);
        rect.sizeDelta = new Vector2(300, 50);

        var text = messageObj.AddComponent<TextMeshProUGUI>();
        text.text = "Настройки сохранены!";
        text.fontSize = fontSize;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Center;

        // Устанавливаем шрифт
        if (customFont != null)
        {
            text.font = customFont;
        } else
        {
            TMP_FontAsset defaultFont = Resources.Load<TMP_FontAsset>("Fonts/DefaultFont");
            if (defaultFont != null)
                text.font = defaultFont;
        }

        text.fontStyle = fontStyle;

        // Автоматическое удаление
        yield return new WaitForSeconds(2f);
        Destroy(messageObj);
    }
}
