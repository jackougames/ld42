using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NewBehaviourScript : MonoBehaviour {

    public string id;
    public SaveData save;

    protected string key;

    private SceneController sceneController;

    private void Awake() {
        sceneController = FindObjectOfType<SceneController>();

        if (!sceneController)
            throw new UnityException("Could not find scene controller!");

        key = GenerateKey();
    }

    private void OnEnable() {
        sceneController.BeforeSceneUnload += Save;
        sceneController.AfterSceneLoad += Load;
    }

    private void OnDisable() {
        sceneController.BeforeSceneUnload -= Save;
        sceneController.AfterSceneLoad -= Load;
    }

    protected abstract string GenerateKey();
    protected abstract void Save();
    protected abstract void Load();

}
