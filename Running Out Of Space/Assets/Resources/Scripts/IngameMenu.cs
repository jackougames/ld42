using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class IngameMenu : MonoBehaviour {

    public event Action OnResume;
    public event Action OnRestart;
    public event Action OnPause;

    public TextMeshProUGUI title;
    public TextMeshProUGUI mainBtnText;

    public Button restartBtn;

    public TextMeshProUGUI maxJetPackChargesText;
    private float maxJetPackCharges;
    public TextMeshProUGUI currentJetPackChargesText;
    private int currentJetPackCharges;

    public Image resourceImage;

    public TextMeshProUGUI maxResourcesText;
    private float maxResources;
    public TextMeshProUGUI currentResourcesText;
    private int currentResources;

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

    public GameObject levelFinishedUI;
    public TextMeshProUGUI pointsText;
    public int pointsPerResource;
    public int pointsPerJetPackCharge;
    private bool resourceBonus = false;
    private bool jetPackBonus = false;
    private int levelPoints;

    private void Awake() {
        sceneController = FindObjectOfType<SceneController>();
        if (!sceneController)
            throw new UnityException("No scene controller found");

        ColorUtility.TryParseHtmlString("#ECFF45", out pauseColor);
        ColorUtility.TryParseHtmlString("#FF465D", out restartColor);

    }

    private void Start() {
        SetJetPackTextColor(Color.green);
        SetResourceTextColor(Color.red);
    }

    public void LoadPauseMenu() {
        title.text = pause;
        title.color = pauseColor;
        mainBtnText.text = resume;
        restartBtn.gameObject.SetActive(true);
    }

    public void LoadRestartMenu() {
        title.text = crap;
        title.color = restartColor;
        mainBtnText.text = restart;
        restartBtn.gameObject.SetActive(false);
    }

    public void SetJetPackCharges(int current, int max) {
        currentJetPackCharges = current;
        maxJetPackCharges = max;
        maxJetPackChargesText.text = "/ " + max.ToString();
        currentJetPackChargesText.text = current.ToString();
        if (current == 0)
            SetJetPackTextColor(Color.red);
        else if(current * 1f / max * 1f <= 0.34f) {
            SetJetPackTextColor(Color.yellow);
        }
    }

    void SetJetPackTextColor(Color color) {
        maxJetPackChargesText.color = color;
        currentJetPackChargesText.color = color;
    }

    void SetResourceTextColor(Color color) {
        maxResourcesText.color = color;
        currentResourcesText.color = color;
    }

    public void SetMaxResourcesToGather(int max) {
        maxResourcesText.text = "/ " + max.ToString();
        maxResources = max;
    }

    public void SetCurrentResources(int current) {
        currentResourcesText.text = current.ToString();
        currentResources = current;
        if(current == maxResources) {
            SetResourceTextColor(Color.green);
        }else  if(current * 1f / maxResources >= 0.5f) {
            SetResourceTextColor(Color.yellow);
        }
        
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                OnMainBtnClick();
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

    public IEnumerator ShowLevelComplete() {
        levelFinishedUI.SetActive(true);

        yield return StartAddingPoints();
        StartCoroutine(FindObjectOfType<Player>().JumpToRocket());
    }

    public IEnumerator StartAddingPoints() {

        //Debug.Log("Current Res: " + currentResources + ", current JetPack: " + currentJetPackCharges);

        //Points for jetpack charges
        if (currentJetPackCharges == maxJetPackCharges)
            jetPackBonus = true;
        int maxI = currentJetPackCharges;
        for (int i = 0; i < maxI; i++) {
            currentJetPackChargesText.text = (--currentJetPackCharges).ToString();
            yield return AddPoints(pointsPerJetPackCharge);
        }
        if (jetPackBonus) {
            yield return AddPoints(pointsPerJetPackCharge * 2);
            jetPackBonus = false;
        }

        //Points for resources
        if (currentResources == maxResources)
            resourceBonus = true;
        maxI = currentResources;
        for (int i = 0; i < maxI; i++) {
            currentResourcesText.text = (--currentResources).ToString();
            yield return AddPoints(pointsPerResource);
        }
        if (resourceBonus) {
            yield return AddPoints(pointsPerResource * 2);
            resourceBonus = false;
        }

    }

    private IEnumerator AddPoints(int points, float speed = 0.5f) {
        speed = speed / points;
        for (int i = 0; i < points; i++) {
            pointsText.text = (++levelPoints).ToString();
            yield return new WaitForSeconds(speed);
        }
    }

    private void Resume() {
        menuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if(OnResume != null)
            OnResume();
    }

    public void Restart() {
        menuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        isRestartingMenu = false;

        ResetMenu();

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

    //Reset Menu here
    private void ResetMenu() {
        SetJetPackTextColor(Color.green);
        SetResourceTextColor(Color.red);
        StopAllCoroutines();
        levelFinishedUI.SetActive(false);
        levelPoints = 0;
        jetPackBonus = false;
        resourceBonus = false;
    }

}
