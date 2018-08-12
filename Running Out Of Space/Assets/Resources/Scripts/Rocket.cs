using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    
    public float launchForce = 200f;
    public float acceleration = 0.55f;

    public string nextLevel = "";

    public ParticleSystem rocketParticles;
    public Transform leftTurbine;
    public Transform rightTurbine;

    private Rigidbody2D rb;

    private bool launch = false;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch() {
        launch = true;
        GetComponent<BoxCollider2D>().isTrigger = true;
        Instantiate(rocketParticles, leftTurbine.position, Quaternion.Euler(270f, 0, 0), transform);
        Instantiate(rocketParticles, rightTurbine.position, Quaternion.Euler(270f, 0, 0), transform);
    }

    private void FixedUpdate() {
        if (launch) {
            rb.AddForce(new Vector2(0, launchForce));
            launchForce += launchForce * acceleration * Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "LevelBounds") {
            if (nextLevel == "") {
                Debug.LogWarning("Next level path not found. String empty!");
                return;
            }
            FindObjectOfType<SceneController>().LoadScene(nextLevel);
        }
    }

}
