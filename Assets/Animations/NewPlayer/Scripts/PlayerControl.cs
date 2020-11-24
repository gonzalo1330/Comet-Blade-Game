using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

    private float movementInputDirection;
    private float jumpTimer;
    private float turnTimer;
    private float wallJumpTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;
    private int lastWallJumpDirection;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool hasWallJumped;
    private bool isTouchingLedge;
    private bool canClimbLedge = false;
    private bool ledgeDetected;
    private bool isDashing;
    private bool switchPower = false;
    private bool knockback;

    [SerializeField]
    private Vector2 knockbackSpeed;

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJumps = 1;

    public float movementSpeed = 10.0f;
    private float defaultSpeed = 10.0f;
    public float jumpForce = 21.0f;
    private float defaultForce = 21.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;
    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;
    public float ledgeClimbYOffset2 = 0f;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCoolDown;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;
    private Vector2 velocityWorkspace;

    public Transform groundCheck;
    public Transform wallCheck;
    public Transform ledgeCheck;

    public LayerMask whatIsGround;

    private bool isObject;
    private bool powerup;
    private float coinCount = 0f;
    private const float defaultDashTime = 9.0f;

    private PlayerCombat PC;
    private GameObject director;
    private Camera minimap;

    // Start is called before the first frame update
    void Start () {
        rb = GetComponent<Rigidbody2D> ();
        anim = GetComponent<Animator> ();
        PC = GetComponent<PlayerCombat> ();
        director = GameObject.Find ("Director");
        minimap = GameObject.Find ("MinimapCamera").GetComponent<Camera> ();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize ();
        wallJumpDirection.Normalize ();
    }

    // Update is called once per frame
    void Update () {
        CheckInput ();
        CheckMovementDirection ();
        UpdateAnimations ();
        UpdateCameraPosition ();
        CheckIfCanJump ();
        CheckIfWallSliding ();
        CheckJump ();
        CheckLedgeClimb ();
        CheckDash ();
        CheckKnockback ();

        // shorcut to enter the following level
        if (Input.GetKeyDown (KeyCode.P)) {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
        }

        // for access to the Menu scene
        if (Input.GetKeyDown (KeyCode.M)) {
            SceneManager.LoadScene (0);
            //SceneManager.LoadScene ("Game Menu");
        }
    }

    private void FixedUpdate () {
        ApplyMovement ();
        CheckSurroundings ();
    }

    public virtual void DamageHop (float velocity) {
        velocityWorkspace.Set (rb.velocity.x, velocity);
        rb.velocity = velocityWorkspace;
    }

    private void CheckIfWallSliding () {
        if (isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0 && !canClimbLedge) {
            isWallSliding = true;
        } else {
            isWallSliding = false;
        }
    }

    private void CheckLedgeClimb () {
        if (ledgeDetected && !canClimbLedge) {
            canClimbLedge = true;

            if (isFacingRight) {
                ledgePos1 = new Vector2 (Mathf.Floor (ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor (ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2 (Mathf.Floor (ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor (ledgePosBot.y) + ledgeClimbYOffset2);
            } else {
                ledgePos1 = new Vector2 (Mathf.Ceil (ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor (ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2 (Mathf.Ceil (ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor (ledgePosBot.y) + ledgeClimbYOffset2);
            }

            canMove = false;
            canFlip = false;

            anim.SetBool ("canClimbLedge", canClimbLedge);
        }

        if (canClimbLedge) {
            transform.position = ledgePos1;
        }
    }

    public void FinishLedgeClimb () {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        anim.SetBool ("canClimbLedge", canClimbLedge);
    }

    private void CheckSurroundings () {
        isGrounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast (wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        isTouchingLedge = Physics2D.Raycast (ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);

        if (isTouchingWall && !isTouchingLedge && !ledgeDetected) {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }

    private void CheckIfCanJump () {
        if (isGrounded && rb.velocity.y <= 0.01f) {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (isTouchingWall) {
            checkJumpMultiplier = false;
            canWallJump = true;
        }

        if (amountOfJumpsLeft <= 0) {
            canNormalJump = false;
        } else {
            canNormalJump = true;
        }

    }

    private void CheckMovementDirection () {
        if (isFacingRight && movementInputDirection < 0) {
            Flip ();
        } else if (!isFacingRight && movementInputDirection > 0) {
            Flip ();
        }

        if (Mathf.Abs (rb.velocity.x) >= 0.01f) {
            isWalking = true;
        } else {
            isWalking = false;
        }
    }

    private void UpdateCameraPosition () {
        Vector3 pos = transform.position;
        pos.z = -10f;
        minimap.gameObject.transform.position = pos;
    }

    private void UpdateAnimations () {
        anim.SetBool ("isWalking", isWalking);
        anim.SetBool ("isGrounded", isGrounded);
        anim.SetFloat ("yVelocity", rb.velocity.y);
        anim.SetBool ("isWallSliding", isWallSliding);
    }

    private void CheckInput () {
        movementInputDirection = Input.GetAxisRaw ("Horizontal");

        if (Input.GetButtonDown ("Jump")) {
            if (isGrounded || (amountOfJumpsLeft > 0 && !isTouchingWall)) {
                NormalJump ();
            } else {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if (Input.GetButtonDown ("Horizontal") && isTouchingWall) {
            if (!isGrounded && movementInputDirection != facingDirection) {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if (turnTimer >= 0) {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0) {
                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMultiplier && !Input.GetButton ("Jump")) {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }

        if (Input.GetButtonDown ("Dash")) {
            if (Time.time >= (lastDash + dashCoolDown))
                AttemptToDash ();
        }

        if (Input.GetKeyDown (KeyCode.Tab)) {
            switchPower = !switchPower;
            PC.ToggleSpecialAttack (switchPower);
            GetComponent<CaptureBehavior> ().enabled = !switchPower;
            director.SetActive (!switchPower);
        }

    }

    public bool GetGroundedStatus () {
        return isGrounded;
    }

    public bool GetDashStatus () {
        return isDashing;
    }

    public void Knockback (int direction) {
        Debug.Log ("player knockback");
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2 (knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockback () {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback) {
            knockback = false;
            rb.velocity = new Vector2 (0.0f, rb.velocity.y);
        }
    }

    private void AttemptToDash () {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool ();
        lastImageXpos = transform.position.x;
    }

    public int GetFacingDirection () {
        return facingDirection;
    }

    private void CheckDash () {
        if (powerup) {
            jumpForce = 31;
        } else {
            movementSpeed = defaultSpeed;
            jumpForce = defaultForce;
        }

        if (isDashing) {
            if (dashTimeLeft > 0) {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2 (dashSpeed * facingDirection, 0.0f);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs (transform.position.x - lastImageXpos) > distanceBetweenImages) {
                    PlayerAfterImagePool.Instance.GetFromPool ();
                    lastImageXpos = transform.position.x;
                }
            }

            if (dashTimeLeft <= 0 || isTouchingWall) {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }

        }
    }

    private void CheckJump () {
        if (jumpTimer > 0) {
            //WallJump
            if (!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection) {
                WallJump ();
            } else if (isGrounded) {
                NormalJump ();
            }
        }

        if (isAttemptingToJump) {
            jumpTimer -= Time.deltaTime;
        }

        if (wallJumpTimer > 0) {
            if (hasWallJumped && movementInputDirection == -lastWallJumpDirection) {
                rb.velocity = new Vector2 (rb.velocity.x, 0.0f);
                hasWallJumped = false;
            } else if (wallJumpTimer <= 0) {
                hasWallJumped = false;
            } else {
                wallJumpTimer -= Time.deltaTime;
            }
        }
    }

    private void NormalJump () {
        if (canNormalJump) {
            rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump () {
        if (canWallJump) {
            rb.velocity = new Vector2 (rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2 (wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce (forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;

        }
    }

    private void ApplyMovement () {

        if (!isGrounded && !isWallSliding && movementInputDirection == 0 && !knockback) {
            rb.velocity = new Vector2 (rb.velocity.x * airDragMultiplier, rb.velocity.y);
        } else if (canMove && !knockback) {
            rb.velocity = new Vector2 (movementSpeed * movementInputDirection, rb.velocity.y);
        }

        if (isWallSliding) {
            if (rb.velocity.y < -wallSlideSpeed) {
                rb.velocity = new Vector2 (rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    public void DisableFlip () {
        canFlip = false;
    }

    public void EnableFlip () {
        canFlip = true;
    }

    private void Flip () {
        if (!isWallSliding && canFlip && !knockback) {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate (0.0f, 180.0f, 0.0f);
        }
    }

    public void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.tag == "Power") {
            powerup = true;
            Destroy (collision.gameObject);
            StartCoroutine (SetDashToFalse ());
        }

        if (collision.gameObject.tag == "Coin") {
            coinCount++;
            Destroy (collision.gameObject);
        }
        if (collision.gameObject.tag == "Trap") {
            ActivateTrap (collision.gameObject);
        }

    }

    private void ActivateTrap (GameObject collision) {
        GameObject box = GameObject.Find ("CaptureCrate10");
        box.SetActive (false);
        Destroy (collision.gameObject);

        GameObject spikes = GameObject.Find ("RisingSpikes");
        spikes.GetComponent<SpriteRenderer> ().enabled = true;
        spikes.GetComponent<SpikeBehavior> ().enabled = true;
        GameObject environment = Instantiate (Resources.Load ("Prefabs/Obstacles/LavaEnvironment") as GameObject);
        SpriteRenderer s = environment.GetComponent<SpriteRenderer> ();
        Color c = s.color;
        c.a -= 0.75f;
        s.color = c;
    }

    public string CoinStatus () {
        return "Found " + coinCount + " Gold";
    }

    IEnumerator SetDashToFalse () {
        yield return new WaitForSeconds (defaultDashTime); // waits before continuing in seconds
        powerup = false;
    }

    private void OnDrawGizmos () {
        Gizmos.DrawWireSphere (groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine (wallCheck.position, new Vector3 (wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}