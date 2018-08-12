using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour {


    private CollectableResource[] resources;
    private Player player;
    private IngameMenu menu;

    private void Awake() {
        player = FindObjectOfType<Player>();
        menu = FindObjectOfType<IngameMenu>();
        resources = GetComponentsInChildren<CollectableResource>();
    }

    private void OnEnable() {
        menu.OnRestart += ResetResources;
        foreach (CollectableResource r in resources) {
            r.OnCollect += ChargeJetPack;
        }
    }

    private void OnDisable() {
        menu.OnRestart -= ResetResources;
        foreach (CollectableResource r in resources) {
            r.OnCollect -= ChargeJetPack;
        }
    }

    public void ChargeJetPack() {
        player.ChargeJetPack();
    }

    public void ResetResources() {
        foreach (CollectableResource r in resources) {
            r.gameObject.SetActive(true);
        }
    }
}
