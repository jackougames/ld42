using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Range(10f, 500f)]
    public float jumpForce = 75f;

    public float maxJumpSpeed = 5f;

    [Range(0, 1f)]
    public float gravityScale = 1f;

    public float moveSpeed = 5f;
    private float saveMoveSpeed;

    [HideInInspector]
    public bool isGrounded;

    private Rigidbody2D rb;

    private bool jump = false;
    private bool isJumping = false;

    [HideInInspector]
    public bool isMoving = false;

    private Animator anim;
    private bool isFalling = false;

    public Transform startPosition;

    public float jetPackForce = 50f;
    public int jetPackUses = 3;
    public Transform jetPackPos;
    public ParticleSystem jetPackParticles;

    private int jetPackUsesLeft;
    private bool useJetPack = false;

    public Vector2 jumpToRocketForce;
    public float timeToDespawn;
    
    public Rocket endRocket;
    private bool jumpToRocket = false;

    public bool ignoreInput = false;

    private bool moveSpeedTowardsZero = false;

    private IngameMenu menu;
    public BoxCollider2D boxCollider;

    private AudioSource audioSource;
    public AudioClip[] hitClips;
    public AudioClip jetPackSFX;
    public AudioClip jetPackFailSFX;
    private float failSFXTimer = 0;

    private void Awake() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        menu = FindObjectOfType<IngameMenu>();
        jetPackUsesLeft = jetPackUses;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        menu.OnRestart += Reset;
    }

    private void OnDisable() {
        menu.OnRestart += Reset;
    }

    // Use this for initialization
    void Start () {
        rb.gravityScale = gravityScale;
        menu.SetJetPackCharges(jetPackUsesLeft, jetPackUses);
        Invoke("StartMoving", 1.5f);
	}

    private void StartMoving() {
        isMoving = true;
    }
	
	// Update is called once per frame
	void Update () {
        //Check for input
        if (!IngameMenu.isPaused && !ignoreInput) { 
            if (Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space)) {
                if(isGrounded && !isJumping && !isFalling) { 
                    jump = true;
                    Debug.Log("jump");
                }
                else if (!isGrounded && jetPackUsesLeft > 0 && Input.GetKeyDown(KeyCode.Space)) {
                    useJetPack = true;
                    jetPackUsesLeft--;
                    menu.SetJetPackCharges(jetPackUsesLeft, jetPackUses);
                }else if(jetPackUsesLeft <= 0 && failSFXTimer <= 0 && Input.GetKeyDown(KeyCode.Space)) {
                    audioSource.PlayOneShot(jetPackFailSFX);
                    failSFXTimer = 1f;
                }
            }
            if (Input.GetKeyDown(KeyCode.X)) {
                if (moveSpeed == 0)
                    moveSpeed = saveMoveSpeed;
                else {
                    saveMoveSpeed = moveSpeed;
                    moveSpeed = 0;
                }
            }
        }

        if(failSFXTimer > 0)
            failSFXTimer -= Time.deltaTime;

        if (moveSpeedTowardsZero && moveSpeed > 0) {
            moveSpeed = Mathf.MoveTowards(moveSpeed, 0, 7 * Time.deltaTime);
            if (moveSpeed <= 0) {
                rb.velocity = Vector2.zero;
                isMoving = false;
            }
        }

        if (rb.velocity.y < -0.25f && !isFalling && isJumping) {
            anim.SetTrigger("falling");
            anim.ResetTrigger("jump");
            isFalling = true;
        }

        if (isGrounded && isFalling) {
            isFalling = false;
            isJumping = false;
            anim.ResetTrigger("falling");
            anim.SetTrigger("grounded");
        }
    }

    private void FixedUpdate() {
        
        if (isMoving) {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        if (jump) {
            rb.AddForce(new Vector3(0, jumpForce));
            isJumping = true;
            anim.SetTrigger("jump");
        }

        if (jumpToRocket) {
            rb.AddForce(jumpToRocketForce);
            useJetPack = true;
            jumpToRocket = false;
        }

        if (useJetPack && !jump) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector3(0, jetPackForce));
            audioSource.PlayOneShot(jetPackSFX);
            Instantiate(jetPackParticles, jetPackPos.position, Quaternion.Euler(270f, 0, 0), transform);
            if(!isJumping && !isFalling) {
                isFalling = true;
            }
            anim.SetTrigger("jetPack");
        }

        if(rb.velocity.y > maxJumpSpeed) {
            rb.velocity = new Vector2(rb.velocity.x, maxJumpSpeed);
        }

        //resets
        useJetPack = false;
        jump = false;

    }
    
    public void SetGravityScale(float gravityScale) {
        this.gravityScale = gravityScale;
        rb.gravityScale = gravityScale;
    }


    public void TransitioningToNextLevel() {
        ignoreInput = true;
        gameObject.tag = "Inactive";
        rb.velocity = Vector2.zero;
        saveMoveSpeed = moveSpeed;
        moveSpeedTowardsZero = true;
        anim.SetTrigger("idle");
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn() {
        yield return menu.ShowLevelComplete();
    }

    public IEnumerator JumpToRocket() {
        jumpToRocket = true;
        boxCollider.isTrigger = true;

        yield return new WaitForSeconds(timeToDespawn);
        jumpToRocket = false;
        GetComponent<SpriteRenderer>().enabled = false;
        moveSpeed = 0;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(1f);
        if (endRocket == null) {
            Debug.LogError("No End rocket found...");
        }
        else {
            endRocket.Launch();
        }
    }

    public void ChargeJetPack() {
        jetPackUsesLeft++;
        menu.SetJetPackCharges(jetPackUsesLeft, jetPackUses);
    }

    public void PlayHitSound() {
        int i = UnityEngine.Random.Range(0, hitClips.Length - 1);
        ignoreInput = true;
        audioSource.PlayOneShot(hitClips[i]);
    }

    public void Reset() {
        StopAllCoroutines();
        ignoreInput = false;
        //Reset position
        transform.position = startPosition.position;
        //Reset speed
        rb.velocity = Vector2.zero;
        isMoving = true;
        //reset jump
        jump = false;
        isJumping = false;
        isFalling = false;
        //reset jetpack
        jetPackUsesLeft = jetPackUses;
        menu.SetJetPackCharges(jetPackUsesLeft, jetPackUses);

        if (moveSpeed == 0)
            moveSpeed = saveMoveSpeed;
        
        boxCollider.isTrigger = false;
        moveSpeedTowardsZero = false;

        gameObject.tag = "Player";

        //Reset Animator
        anim.ResetTrigger("falling");
        anim.ResetTrigger("grounded");
        anim.ResetTrigger("jetPack");
        anim.ResetTrigger("jump");
        anim.Play("Run");
    }


}
