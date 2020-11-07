using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private float movementInputDirection;
    private int facingDirection = 1;
    private bool isFacingRight = true;
    private bool isWalking;
    public float movementSpeed;
    public float jumpForce;
    private bool isGrounded;
    public float groundCheckRadius;
    public int totalScore;

    private bool isDashing;
    private const float defaultDashTime = 8.0f;
    private const float defaultSpeed = 5.0f;
    private const float defaultForce = 16.0f;

    private Rigidbody2D rb;
    private Animator anim;

    private bool canJump;
    private int amountOfJumpsLeft;
    public int amountOfJumps = 1;

    public Transform groundCheck;

    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start () {
        rb = GetComponent<Rigidbody2D> ();
        anim = GetComponent<Animator> ();
        amountOfJumpsLeft = amountOfJumps;
        movementSpeed = defaultSpeed;
        jumpForce = defaultForce;
        totalScore = 0;
    }

    // Update is called once per frame
    void Update () {
        CheckInput ();
        CheckMovementDirection ();
        UpdateAnimations ();
        CheckIfCanJump ();
    }

    private void FixedUpdate () {
        ApplyMovement ();
        CheckSurroundings ();
    }

    private void CheckSurroundings () {
        isGrounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void CheckIfCanJump () {
        if ((isGrounded && rb.velocity.y <= 0)) {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (amountOfJumpsLeft <= 0) {
            canJump = false;
        } else {
            canJump = true;
        }

    }

    private void CheckMovementDirection () {
        if (isFacingRight && movementInputDirection < 0) {
            Flip ();
        } else if (!isFacingRight && movementInputDirection > 0) {
            Flip ();
        }

        if (rb.velocity.x > 0.01f || rb.velocity.x < -0.01f) {
            isWalking = true;
        } else {
            isWalking = false;
        }
    }

    private void UpdateAnimations () {
        anim.SetBool ("isWalking", isWalking);
        anim.SetBool ("isGrounded", isGrounded);
        anim.SetFloat ("yVelocity", rb.velocity.y);
    }

    private void CheckInput () {
        movementInputDirection = Input.GetAxisRaw ("Horizontal");

        if (Input.GetButtonDown ("Jump")) {
            Jump ();
        }

        CheckDash ();
    }

    private void CheckDash () {
        if (isDashing) {
            jumpForce = 33;
            movementSpeed = 21;
        } else {
            movementSpeed = defaultSpeed;
            jumpForce = defaultForce;
        }

        if (Input.GetKey (KeyCode.L))
            isDashing = false;
    }

    private void Jump () {
        if (canJump) {
            rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
    }

    private void ApplyMovement () {
        rb.velocity = new Vector2 (movementSpeed * movementInputDirection, rb.velocity.y);
    }

    private void Flip () {
        isFacingRight = !isFacingRight;
        transform.Rotate (0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos () {
        Gizmos.DrawWireSphere (groundCheck.position, groundCheckRadius);
    }

    public void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.name == "Timer") {
            isDashing = true;
            GameObject timer = GameObject.Find ("Timer");
            Destroy (timer);
            StartCoroutine (SetDashToFalse ());
        }

        if (collision.gameObject.name == "Coin") {
            totalScore++;
            Debug.Log ("Total score: " + totalScore);
            Destroy (collision.gameObject);
        }
    }

    IEnumerator SetDashToFalse () {
        yield return new WaitForSeconds (defaultDashTime); // waits before continuing in seconds
        isDashing = false;
    }
}