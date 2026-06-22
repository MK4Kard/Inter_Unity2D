using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TypewriterWithDOTween : MonoBehaviour
{
    public Canvas textCanvas;
    public TMP_Text textComponent;
    public string fullText;
    public float typeDuration = 2f;
    public float delayBeforeShow = 3f;
    public bool autoClose = true;
    public bool waitForSpace = false;
    public UnityEvent onTypingComplete;

    private bool isTyping = true;
    private bool isClosed = false;
    private bool isFull = false;
    private GameSettings settings;
    private Tween typingTween;

    void Start()
    {
        // Загружаем настройки игры
        settings = Resources.Load<GameSettings>("GameSettings");

        // Подписываемся на событие изменения скорости текста
        GameSettings.OnTextSpeedChanged += OnTextSpeedChanged;

        // На старте Canvas выключен (текст не виден)
        textCanvas.gameObject.SetActive(false);

        // Получаем длительность печати из настроек игры
        float actualSpeed = GetTypingDuration();
        typeDuration = actualSpeed;

        // Запускаем печать с задержкой delayBeforeShow секунд
        Invoke("ActivateAndType", delayBeforeShow);
    }

    // Получает длительность печати из настроек игры
    float GetTypingDuration()
    {
        // Конвертируем скорость из настроек в длительность
        if (settings != null)
        {
            return fullText.Length * settings.textSpeed;
        }
        return typeDuration;
    }

    // Активирует Canvas и запускает анимацию печати текста
    void ActivateAndType()
    {
        // Включаем Canvas с текстом
        textCanvas.gameObject.SetActive(true);

        // Очищаем текст перед началом печати
        textComponent.text = "";

        // Запускаем анимацию печати с помощью DOTween
        typingTween = textComponent.DOText(fullText, typeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                isTyping = false;

                // Если включено автоматическое закрытие и не нужно ждать пробел
                if (autoClose && !waitForSpace)
                    Invoke("CloseText", 2f);
            });
    }

    void Update()
    {
        // Проверяем условия для закрытия по пробелу
        if (waitForSpace && !isClosed && textCanvas.gameObject.activeSelf && 
            Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!isFull)
            {
                typingTween?.Kill();
                textComponent.text = fullText;
                isTyping = false;
                isFull = true;
            }
            else
            {
                onTypingComplete?.Invoke();
                CloseText();
            }
        }
    }

    // Позволяет изменить скорость печати прямо во время анимации
    void OnTextSpeedChanged(float newSpeed)
    {
        // Если идёт печать и анимация существует и активна
        if (isTyping && typingTween != null && typingTween.IsActive())
        {
            // Обновляем скорость во время печати
            float newDuration = fullText.Length * newSpeed;
            float progress = textComponent.text.Length / (float)fullText.Length;

            typingTween.Kill();
            typingTween = textComponent.DOText(fullText, newDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => {
                    isTyping = false;
                    if (autoClose && !waitForSpace)
                        Invoke("CloseText", 2f);
                });

            typingTween.Goto(progress * newDuration);
        }
    }

    // Закрывает текст с анимацией затухания
    void CloseText()
    {
        if (isClosed) return;
        isClosed = true;

        typingTween?.Kill();
        textComponent.text = fullText;

        textComponent.DOFade(0f, 0.5f)
            .OnComplete(() => textCanvas.gameObject.SetActive(false));
    }

    // Вызывается при уничтожении объекта
    void OnDestroy()
    {
        GameSettings.OnTextSpeedChanged -= OnTextSpeedChanged;
        typingTween?.Kill();
    }
}
