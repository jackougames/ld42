using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public string firstLevelName = "Moon";

    public GameObject helpWindow;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Play();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Help();
        }
    }

    public void Play() {
        FindObjectOfType<SceneController>().LoadScene(firstLevelName);
    }

    public void Quit() {
        Application.Quit();
    }

    public void Help() {
        if (helpWindow.activeInHierarchy) {
            helpWindow.SetActive(false);
        }
        else {
            helpWindow.SetActive(true);
        }
    }

}
