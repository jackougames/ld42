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

    private void Awake() {
        targetPos = new Vector2(transform.position.x, transform.position.y + bounceRange);
        startPos = transform.position;
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
        if(collision.tag == "Player") {
            if (OnCollect != null)
                OnCollect();
            else
                Debug.LogWarning("Nothing collected, but resource will be deactivated. Subscribe to OnCollect");

            gameObject.SetActive(false);
        }
    }
}
