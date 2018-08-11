using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public event Action BeforeSceneUnload;
    public event Action AfterSceneLoad;

    public CanvasGroup faderCanvasGroup;
    public float fadeDuration = 1f;
    public string startingSceneName = "";

    private bool isFading = false;

    // Use this for initialization
    private IEnumerator Start () {

        faderCanvasGroup.alpha = 1f;

        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName));

        StartCoroutine(FadeTo(0f));
	}

    public void LoadScene(string sceneName) {
        if (!isFading) {
            StartCoroutine(SwitchToScene(sceneName));
        }
    }

    private IEnumerator SwitchToScene(string sceneName) {

        yield return StartCoroutine(FadeTo(1f));

        if (BeforeSceneUnload != null)
            BeforeSceneUnload();

        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        if (AfterSceneLoad != null)
            AfterSceneLoad();

        yield return StartCoroutine(FadeTo(0f));
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName) {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
    }

    private IEnumerator FadeTo(float alpha) {
        isFading = true;
        faderCanvasGroup.blocksRaycasts = true;

        float fSpeed = Mathf.Abs((faderCanvasGroup.alpha - alpha) / fadeDuration);
        while (!Mathf.Approximately(faderCanvasGroup.alpha, alpha)) {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, alpha, fSpeed * Time.deltaTime);
            yield return null;
        }

        faderCanvasGroup.blocksRaycasts = false;
        isFading = false;
    }
}
