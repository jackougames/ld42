using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour {

    public Vector2 timeBtwAtkMinMax;

    public SlimeBall slimeBallPrefab;

    public Transform slimeBallSpawnPos;

    public Transform patrolPointA;
    public Transform patrolPointB;
    public float moveSpeed = 1.5f;

    public Vector2 patrolPauseMinMax = new Vector2(1f, 2.5f);
    [Range(1, 100f)]
    public float patrolRandomSwitch = 2.5f;
    public float patrolRandomCheck = 2f;
    private float patrolRandomTimer;
    private Transform currentPatrolPoint;
    private bool switching = false;
    private bool patrolling = false;

    private float atkDlyTimer;

    private Animator anim;

    private bool isAttacking = false;

    private Player player;
    private SpriteRenderer sprite;
    private bool facingRight = false;

    private IngameMenu menu;
    private Vector3 startPosition;

    private AudioSource audioSource;
    public AudioClip[] atkSFX;

    private void Awake() {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        sprite = GetComponent<SpriteRenderer>();
        menu = FindObjectOfType<IngameMenu>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        menu.OnRestart += ResetOnRestart;
    }

    private void OnDisable() {
        menu.OnRestart -= ResetOnRestart;
    }

    private void Start() {
        if (slimeBallPrefab == null)
            Debug.LogWarning("No slime ball prefab attached!");

        atkDlyTimer = Random.Range(timeBtwAtkMinMax.x, timeBtwAtkMinMax.y);
        if (patrolPointA && patrolPointB) {
            currentPatrolPoint = patrolPointA;
            patrolling = true;
        }
        patrolRandomTimer = patrolRandomCheck;

        startPosition = transform.position;
    }

    private void Update() {
        if (atkDlyTimer > 0) {
            atkDlyTimer -= Time.deltaTime;

            if (atkDlyTimer <= 0 && !isAttacking)
                
                StartAttack();
        }

        if (patrolling) {
            if (transform.position == currentPatrolPoint.position && !switching) {
                switching = true;
                StartCoroutine(SwitchPatrolPoint());
            }
            else if (patrolRandomTimer <= 0) {
                if (Random.Range(1, 100) <= patrolRandomSwitch) {
                    //Debug.Log("Patroling random");
                    StartCoroutine(SwitchPatrolPoint(true));
                }
                patrolRandomTimer = patrolRandomCheck;
            }
            else {
                patrolRandomTimer -= Time.deltaTime;
            }
        }

        //flip sprite
        if(player.transform.position.x > transform.position.x && !facingRight){
            sprite.flipX = !sprite.flipX;
            facingRight = true;
        }


    }

    private IEnumerator SwitchPatrolPoint(bool skipWait = false) {
        if(!skipWait)
            yield return new WaitForSeconds(Random.Range(patrolPauseMinMax.x, patrolPauseMinMax.y));

        currentPatrolPoint = currentPatrolPoint == patrolPointA ? patrolPointB : patrolPointA;
        switching = false;
    }

    private void FixedUpdate() {
        if(patrolling)
            transform.position = Vector2.MoveTowards(transform.position, currentPatrolPoint.position, moveSpeed * Time.fixedDeltaTime);
    }

    private void StartAttack() {
        //Debug.Log("startAttack");
        if (!isAttacking) {
            isAttacking = true;
            //Debug.Log("Set Anim Atk");
            anim.SetTrigger("attack");
        }
    }

    public void Attack() {
        //Debug.Log("Attacking");
        atkDlyTimer = Random.Range(timeBtwAtkMinMax.x, timeBtwAtkMinMax.y);
        isAttacking = false;
        int i = Random.Range(0, atkSFX.Length - 1);
        audioSource.PlayOneShot(atkSFX[i]);
        Instantiate(slimeBallPrefab, slimeBallSpawnPos.position, Quaternion.identity);
    }

    public void ResetOnRestart() {
        atkDlyTimer = Random.Range(timeBtwAtkMinMax.x, timeBtwAtkMinMax.y);
        isAttacking = false;
        transform.position = startPosition;
        if (facingRight) {
            sprite.flipX = !sprite.flipX;
            facingRight = false;
        }
        if (patrolling) {
            StopAllCoroutines();
            currentPatrolPoint = patrolPointA;
            patrolRandomTimer = patrolRandomCheck;
            switching = false;
        }
    }


}
