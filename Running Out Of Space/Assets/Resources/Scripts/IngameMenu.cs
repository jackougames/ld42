using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class IngameMenu : MonoBehaviour {

    public event Action OnResume;
    public event Action OnRestart;
    public event Action OnPause;

    public TextMeshProUGUI title;
    public TextMeshProUGUI mainBtnText;

    private SceneController sceneController;

    private string pause = "Pause";
    private string resume = "RESUME";

    private string crap = "Oh Crap!";
    private string restart = "RESTART";

    private bool isRestartingMenu = false;

    Color pauseColor;
    Color restartColor;

    [HideInInspector]
    public static bool isPaused = false;

    public GameObject menuUI;

    private void Awake() {
        sceneController = FindObjectOfType<SceneController>();
        if (!sceneController)
            throw new UnityException("No scene controller found");

        ColorUtility.TryParseHtmlString("#ECFF45", out pauseColor);
        ColorUtility.TryParseHtmlString("#FF465D", out restartColor);
    }

    public void LoadPauseMenu() {
        title.text = pause;
        title.color = pauseColor;
        mainBtnText.text = resume;
    }

    public void LoadRestartMenu() {
        title.text = crap;
        title.color = restartColor;
        mainBtnText.text = restart;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                Resume();
            }
            else {
                ShowMenu();
            }
        }
    }

    public void ShowMenu(bool isRestart = false) {
        if (isRestart) {
            isRestartingMenu = true;
            LoadRestartMenu();
        }
        else {
            LoadPauseMenu();
        }

        menuUI.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;

        if (OnPause != null)
            OnPause();

    }

    private void Resume() {
        menuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if(OnResume != null)
            OnResume();
    }

    private void Restart() {
        menuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        isRestartingMenu = false;

        if(OnRestart != null)
            OnRestart();
    }

    public void OnMainBtnClick() {
        if (isRestartingMenu) {
            Restart();
        }
        else {
            Resume();
        }
    }

    public void LoadMainMenu() {
        Debug.Log("Loading Menu");
        //sceneController.LoadScene("MainMenu");
    }

    public void QuitGame() {
        Debug.Log("Quitting Game.");
        Application.Quit();
    }
   
}
