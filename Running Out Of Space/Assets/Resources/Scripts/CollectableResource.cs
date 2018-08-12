using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectableResource : MonoBehaviour {

    public event Action OnCollect;

    public float bounceRange = 0.5f;
    public float bounceSpeed = 1.75f;

    private Vector3 targetPos;
    private Vector3 startPos;

    private AudioSource audioSource;
    public AudioClip[] collectSFX;

    public ParticleSystem collectParticleFX;

    private IngameMenu menu;

    bool collected = false;

    private void Awake() {
        targetPos = new Vector2(transform.position.x, transform.position.y + bounceRange);
        startPos = transform.position;
        audioSource = GetComponent<AudioSource>();
        menu = FindObjectOfType<IngameMenu>();
    }

    private void OnEnable() {
        menu.OnRestart += ResetOnRestart;
    }

    private void OnDisable() {
        menu.OnRestart -= ResetOnRestart;
    }

    private void Update() {
        if(transform.position == targetPos) {
            Vector3 tmp = startPos;
            startPos = targetPos;
            targetPos = tmp;
        }
    }

    private void FixedUpdate() {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, bounceSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player" && !collected) {
            collected = true;
            int i = UnityEngine.Random.Range(0, collectSFX.Length - 1);
            audioSource.PlayOneShot(collectSFX[i]);
            GetComponent<SpriteRenderer>().enabled = false;
            Instantiate(collectParticleFX, transform.position, Quaternion.identity);
            if (OnCollect != null)
                OnCollect();
            else
                Debug.LogWarning("Nothing collected, but resource will be deactivated. Subscribe to OnCollect");

            StartCoroutine(Deactivate());
        }
    }

    private IEnumerator Deactivate() {
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().enabled = true;
        collected = false;
        gameObject.SetActive(false);
    }

    public void ResetOnRestart() {
        StopAllCoroutines();
        GetComponent<SpriteRenderer>().enabled = true;
        collected = false;
        gameObject.SetActive(true);
    }
}
