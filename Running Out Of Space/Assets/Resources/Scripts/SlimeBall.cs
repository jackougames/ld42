using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBall : MonoBehaviour {

    private float speed;

    public float yTargetPosRange = 1.25f;

    private Vector3 direction;

    private IngameMenu menu;

    public ParticleSystem explodeParticles;

    private void Awake() {

        menu = FindObjectOfType<IngameMenu>();

        //Get target position for this ball
        Vector3 targetPosition = FindObjectOfType<Player>().GetComponent<Transform>().position;
        float targetPosY = Random.Range(transform.position.y - yTargetPosRange, transform.position.y);
        targetPosition.y = targetPosY;

        //Debug.Log("TargetPos Y:" + targetPosition.y);

        //Get the direction to move slimeBall towards
        Vector3 heading = targetPosition - transform.position;
        float distance = heading.magnitude;

        direction = heading / distance;

    }

    private void Start() {
        speed = FindObjectOfType<Player>().moveSpeed * 1.5f;
    }

    private void OnEnable() {
        menu.OnRestart += DestroyOnRestart;
    }

    private void OnDisable() {
        menu.OnRestart -= DestroyOnRestart;
    }

    private void FixedUpdate() {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //Debug.Log("Projectile hit something: " + collision.tag);
        if (collision.tag == "Player") {
            //Debug.Log("Hit by projectile");
            StartCoroutine(Explode(true));
        }
        else if(collision.tag == "Ground" || collision.tag == "Obstacle") {
            //Debug.Log("Projectile hit something");
            StartCoroutine(Explode());
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "LevelBounds") {
            //Debug.Log("Destroy slime out of bounds");
            Destroy(gameObject);
        }
    }

    private IEnumerator Explode(bool hitPlayer = false) {
        //Debug.Log("Explode slime");
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Instantiate(explodeParticles, transform.position, Quaternion.identity);

        if (hitPlayer)
            yield return StartCoroutine(ShowRestartMenu());

        Destroy(gameObject);
    }

    private IEnumerator ShowRestartMenu() {
        yield return new WaitForSeconds(0.25f);

        menu.ShowMenu(true);
    }

    public void DestroyOnRestart() {
        Destroy(gameObject);
    }

}
