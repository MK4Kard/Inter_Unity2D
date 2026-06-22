using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    private Animator anim;
    public int levelToLoad;

    public GameObject loadingScreen;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator компонент отсутствует на объекте " + gameObject.name);
        }

        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    public void FadeToLevel()
    {
        if (anim == null)
        {
            Debug.LogError("Не могу запустить анимацию: Animator = null");
            return;
        }
        anim.SetTrigger("fade");
    }

    public void OnFadeComplete()
    {
        StartCoroutine(LoadLevelAsync());
    }

    IEnumerator LoadLevelAsync()
    {
        // Показываем экран загрузки
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        // Асинхронная загрузка сцены
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad);

        // Ждем, пока загрузка не завершится
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
