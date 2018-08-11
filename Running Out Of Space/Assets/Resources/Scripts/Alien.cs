using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour {

    public Vector2 timeBtwAtkMinMax;

    public SlimeBall slimeBallPrefab;

    public Transform slimeBallSpawnPos;

    private float atkDlyTimer;

    private Animator anim;

    private bool isAttacking = false;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void Start() {
        if (slimeBallPrefab == null)
            Debug.LogWarning("No slime ball prefab attached!");

        atkDlyTimer = Random.Range(timeBtwAtkMinMax.x, timeBtwAtkMinMax.y);
    }

    private void Update() {
        if (atkDlyTimer > 0) {
            atkDlyTimer -= Time.deltaTime;

            if (atkDlyTimer <= 0 && !isAttacking)
                
                StartAttack();
        }

    }

    private void StartAttack() {
        Debug.Log("startAttack");
        if (!isAttacking) {
            isAttacking = true;
            Debug.Log("Set Anim Atk");
            anim.SetTrigger("attack");
        }
    }

    public void Attack() {
        Debug.Log("Attacking");
        atkDlyTimer = Random.Range(timeBtwAtkMinMax.x, timeBtwAtkMinMax.y);
        isAttacking = false;
        Instantiate(slimeBallPrefab, slimeBallSpawnPos.position, Quaternion.identity);
    }


}
