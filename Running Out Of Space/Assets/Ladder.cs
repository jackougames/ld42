using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {

    private IngameMenu menu;

    private void Awake() {
        menu = FindObjectOfType<IngameMenu>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            //Decide whether or not level has been completed
            if (Resources.hasEnoughResources) {
                FindObjectOfType<Player>().TransitioningToNextLevel();
            }
            else {
                menu.ShowMenu(true);
            }
        }
    }


}
