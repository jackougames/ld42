using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Resources : MonoBehaviour {

    private int maxResources;
    private static int currentResources;
    private CollectableResource[] resources;
    private IngameMenu menu;

    public static bool hasEnoughResources = false;

    private void Awake() {
        menu = FindObjectOfType<IngameMenu>();

        resources = GetComponentsInChildren<CollectableResource>();
        
    }

    private void Start() {
        maxResources = resources.Length;
        currentResources = 0;
        menu.SetMaxResourcesToGather(maxResources);
        menu.SetCurrentResources(0);
    }

    private void OnEnable() {
        menu.OnRestart += ResetResources;
        foreach(CollectableResource r in resources) {
            r.OnCollect += CollectResource;
        }
    }

    private void OnDisable() {
        menu.OnRestart -= ResetResources;
        foreach (CollectableResource r in resources) {
            r.OnCollect -= CollectResource;
        }
    }
    
    public void ResetResources() {
        foreach(CollectableResource r in resources) {
            r.gameObject.SetActive(true);
            currentResources = 0;
            menu.SetCurrentResources(0);
        }
    }

    public void CollectResource() {
        menu.SetCurrentResources(++currentResources);
        if(currentResources * 1f / maxResources * 1f >= 0.5f) {
            hasEnoughResources = true;
        }
        if (currentResources > maxResources)
            Debug.LogWarning("Something went wrong, there should not be more than " + maxResources + " resources to collect. Count is: " + currentResources);
    }

}
