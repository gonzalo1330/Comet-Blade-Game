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

    private bool jumpAttack = false;
    private bool attack = false;
    public int attackDamage = 10;
    private float attackCooldown = 0.2f;

    private bool isDashing;
    private const float defaultDashTime = 8.0f;
    private const float defaultSpeed = 5.0f;
    private const float defaultForce = 16.0f;
    public const float attackRange = 0.8f;

    private Rigidbody2D rb;
    private Animator anim;

    private bool canJump;
    private int amountOfJumpsLeft;
    public int amountOfJumps = 1;
    public Transform attackPoint;

    public Transform groundCheck;

    public LayerMask whatIsGround;
    public LayerMask enemyLayers;

    public bool checkpointMet;
    public int health = 5;
    public int maxHealth = 5;
    //Get health bar
    public HealthBar healthBar;
    private float flashtimer = 0.3f;


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
        attackDamage = 10;
    }

    // Update is called once per frame
    void Update () {
        CheckInput ();
        CheckMovementDirection ();
        UpdateAnimations ();
        CheckIfCanJump ();
        Checkpoint ();
        UpdateHealthBar();

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

        if (Input.GetKeyDown (KeyCode.X) && attackCooldown <= 0) { // make sure cooldown is reached before using attack
            jumpAttack = true;
            attack = true;
            attackCooldown = 1f;
        }
        if (Input.GetButtonDown ("Jump")) {
            Jump ();

        }
        if (attackCooldown >= 0) {
            attackCooldown -= Time.deltaTime;
        }
        CheckDash ();
        Attack();
        attack = false;
        jumpAttack = false;
    }
    void Attack () {
        Collider2D[] hitEnemies = { };
        if (isGrounded && attack) {
            anim.SetBool ("attack", true);
            hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            print ("attack clicked");
        }
        if (isGrounded && !attack) {
            anim.SetBool ("attack", false);
        }
        if (jumpAttack && !isGrounded) {
            GameObject e = Instantiate(Resources.Load("Prefabs/fireball") as GameObject);
            e.transform.localPosition = transform.localPosition;
            e.transform.localRotation = transform.localRotation;
            anim.SetBool ("jumpAttack", true);
            print("Jump Attack called");
        }
        if (!jumpAttack) {
            anim.SetBool("jumpAttack", false);
        }
        foreach (Collider2D enemy in hitEnemies) { 
            GameObject parent = enemy.transform.parent.gameObject;
            parent.gameObject.GetComponent<BasicEnemyController>().TakeDamage(attackDamage);
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

        // player located the checkpoint
        if (collision.gameObject.name == "Checkpoint") {
            Destroy (collision.gameObject);
            checkpointMet = true;
            savedPostion = collision.transform.position;
        }

        // collision with enemies within the world
        if (collision.gameObject.tag == "Enemy") {
            health--;
        }
    }
    public void UpdateHealthBar() {
        float healthPercent = (float)health / maxHealth;
        if (healthPercent >= 0) {
            healthBar.setSize(new Vector3(1.0f * healthPercent, 1.0f));
        }
        flashtimer -= Time.deltaTime;
        if (healthPercent < 0.3f) {
            print(healthPercent * 100f);
            if (flashtimer <= 0) {
                healthBar.setColor(Color.white);
                flashtimer = 0.3f;
            }
            else {
                healthBar.setColor(Color.red);
            }
        }
    }

    void OnTriggerEnter2D (Collider2D collision) {
        // collision with enemy missile
        if (collision.gameObject.tag == "Enemy") {
            health--;
        }
    }

    IEnumerator SetDashToFalse () {
        yield return new WaitForSeconds (defaultDashTime); // waits before continuing in seconds
        isDashing = false;
    }
}