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
    public bool isMoving = true;

    private Animator anim;
    private bool isFalling = false;
    public float fallMultiplier = 1.5f;

    public Transform startPosition;

    public float jetPackForce = 50f;
    public int jetPackUses = 3;
    public Transform jetPackPos;
    public ParticleSystem jetPackParticles;

    private int jetPackUsesLeft;
    private bool useJetPack = false;

    private void Awake() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        FindObjectOfType<IngameMenu>().OnRestart += Reset;
        jetPackUsesLeft = jetPackUses;
    }

    // Use this for initialization
    void Start () {
        rb.gravityScale = gravityScale;
	}
	
	// Update is called once per frame
	void Update () {
        //Check for input
        if (!IngameMenu.isPaused) { 
            if (Input.GetKey(KeyCode.Space)) {
                if(isGrounded && !isJumping)
                    jump = true;
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (isJumping && jetPackUsesLeft > 0) {
                    useJetPack = true;
                    jetPackUsesLeft--;
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

        if (rb.velocity.y < -0.25f && !isFalling && isJumping) {
            anim.SetTrigger("falling");
            isFalling = true;
        }

        if (isGrounded && isFalling) {
            isFalling = false;
            isJumping = false;
            anim.SetTrigger("grounded");
        }
    }

    private void FixedUpdate() {
        
        if (isMoving && isGrounded) {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        if (jump) {
            rb.AddForce(new Vector3(0, jumpForce));
            jump = false;
            isJumping = true;
            anim.SetTrigger("jump");
        }

        if (useJetPack) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector3(0, jetPackForce));
            Instantiate(jetPackParticles, jetPackPos.position, Quaternion.Euler(270f, 0, 0), transform);
            anim.SetTrigger("jetPack");
            useJetPack = false;
        }

        if(rb.velocity.y > maxJumpSpeed) {
            rb.velocity = new Vector2(rb.velocity.x, maxJumpSpeed);
        }

    }
    
    public void SetGravityScale(float gravityScale) {
        this.gravityScale = gravityScale;
        rb.gravityScale = gravityScale;
    }

    public void Reset() {
        //Reset position
        transform.position = startPosition.position;
        //Reset speed
        rb.velocity = Vector2.zero;
        //reset jump
        jump = false;
        isJumping = false;
        isFalling = false;
        jetPackUsesLeft = jetPackUses;

        //Reset Animator
        anim.ResetTrigger("falling");
        anim.ResetTrigger("grounded");
        anim.ResetTrigger("jetPack");
        anim.ResetTrigger("jump");
        anim.Play("Run");
    }


}
