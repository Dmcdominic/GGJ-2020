using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static LoadManager instance;

    [SerializeField] private float transitionFadeTime;
    [SerializeField] private CanvasGroup blackScreen;

    private Coroutine loading;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        if (loading == null)
        {
            loading = StartCoroutine(TransitionToScene(sceneName));
        }
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        Time.timeScale = 0;
        yield return StartCoroutine(FadeOut(transitionFadeTime));
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return StartCoroutine(FadeIn(transitionFadeTime));
        Time.timeScale = 1;
        yield return null;
        loading = null;
    }

    public void UnloadSceneAsync(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public void LoadSceneAsync(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void ResetSceneAsync(string sceneName)
    {
        UnloadSceneAsync(sceneName);
        LoadSceneAsync(sceneName);
    }

    //Transition functions

    private IEnumerator FadeOut(float time)
    {
        //if (blackScreen.canvas.worldCamera == null) blackScreen.canvas.worldCamera = Camera.main;
        float idx = 0f;
        float increment = 1 / (time / 0.01f);
        while (blackScreen.alpha < 1f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            blackScreen.alpha = idx;
            idx += increment;
        }
    }

    private IEnumerator FadeIn(float time)
    {
        //if (blackScreen.canvas.worldCamera == null) blackScreen.canvas.worldCamera = Camera.main;
        float idx = 1f;
        float increment = 1 / (time / 0.01f);
        while (blackScreen.alpha > 0f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            blackScreen.alpha = idx;
            idx -= increment;
        }
    }
}
