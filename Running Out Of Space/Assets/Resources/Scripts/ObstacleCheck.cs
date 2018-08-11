using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCheck : MonoBehaviour {

    IngameMenu menu;

    private void Awake() {
        menu = FindObjectOfType<IngameMenu>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            Debug.Log("Hit Obstacle!");
            menu.ShowMenu(true);
        }
    }

}
