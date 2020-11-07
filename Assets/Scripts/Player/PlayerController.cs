using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public const float attackRange = 0.8f;

    private Rigidbody2D rb;
    private Animator anim;

    private bool canJump;
    private bool jumpAttack = false;
    private bool attack = false;
    private int amountOfJumpsLeft;
    public int amountOfJumps = 1;
    public Transform attackPoint;

    public Transform groundCheck;

    public LayerMask whatIsGround;
    public LayerMask enemyLayers;

    public bool checkpointMet;
    public int health = 5;

    private Vector3 savedPostion;

    // Start is called before the first frame update
    void Start () {
        checkpointMet = false;
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
        Checkpoint ();

        if (Input.GetKeyDown (KeyCode.P))
            SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);

        if (Input.GetKeyDown (KeyCode.G))
            health = 0;
    }

    // function for checking if checkpoints are met when you lose all health
    private void Checkpoint () {
        if (health == 0 && checkpointMet) { // met checkpoint but lost all health
            RespawnAtLastCheckPoint ();
        } else if (health == 0) { // checkpoint not met and died
            Application.Quit ();
        }
    }

    // respawns character at last known checkpoint and restores health
    private void RespawnAtLastCheckPoint () {
        isDashing = false;
        health = 5;
        gameObject.transform.position = savedPostion; // location change
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

        if (Input.GetKeyDown(KeyCode.X)) {
            jumpAttack = true;
            attack = true;
        }
        Attack();
        if (Input.GetButtonDown ("Jump")) {
            Jump ();
    
        }


        CheckDash ();
        attack = false;
    }
    void Attack() {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        if (isGrounded && attack) {
            anim.SetBool("attack", true);
            print("attack clicked");
        }
        if (isGrounded && !attack) {
            anim.SetBool("attack", false);
        }
        if (jumpAttack && !isGrounded && !this.anim.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack")) {
            anim.SetBool("jumpAttack", true);
        }
        if (!jumpAttack && !anim.GetCurrentAnimatorStateInfo(1).IsTag("JumpAttack")) {
            anim.SetBool("jumpAttack", false);
        }
        foreach (Collider2D enemy in hitEnemies) {
            Debug.Log("We hit" + enemy.name);
        }
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
            Destroy (collision.gameObject);
        }

        if (collision.gameObject.name == "Checkpoint") {
            Destroy (collision.gameObject);
            checkpointMet = true;
            savedPostion = collision.transform.position;
        }
    }

    IEnumerator SetDashToFalse () {
        yield return new WaitForSeconds (defaultDashTime); // waits before continuing in seconds
        isDashing = false;
    }
}