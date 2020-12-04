using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerAirAttackState AirAttackState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    #endregion

    #region Check Transforms

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    public Transform launchPoint;
    [SerializeField]
    public LayerMask whatIsDamageable;

    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    public HealthBar healthBar;
    private float flashtimer = 0.3f;
    private float maxHealth = 50f;
    private float currentHealth = 50f;
    private float currentStunResistance;
    private float lastDamageTime = 0f;

    [SerializeField]
    private float knockbackDuration;
    [SerializeField]
    private Vector2 knockbackSpeed;
    private float knockbackStartTime;
    private bool knockback;

    private Vector2 workspace;
    public float damageHopSpeed = 10f;
    public float stunRecoveryTime = 2f;
    public float stunResistance = 3f;

    protected bool isStunned;
    protected bool isDead;

    // attack
    private AttackDetails attackDetails;
    public PlayerShoot PS;

    // checkpoint
    private bool checkpointMet = false;
    private Vector3 checkpointPostion;

    // power up respawn 
    private Vector3 powerUpPostion;

    // damage
    public GameObject damageParticle;
    private float deadTime = 0f;

    // powerups
    private float lastPowerupTime = 0f;
    private bool powerupActive;

    // UI
    private float coinCount = 0f;
    private Camera minimap;
    #endregion

    //stats
    public string path = "Assets/statistics.txt";
    private int respawnCount;

    #region Unity Callback Functions
    private void Awake () {
        StateMachine = new PlayerStateMachine ();

        IdleState = new PlayerIdleState (this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState (this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState (this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState (this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState (this, StateMachine, playerData, "land");
        WallSlideState = new PlayerWallSlideState (this, StateMachine, playerData, "wallSlide");
        WallGrabState = new PlayerWallGrabState (this, StateMachine, playerData, "wallGrab");
        WallClimbState = new PlayerWallClimbState (this, StateMachine, playerData, "wallClimb");
        WallJumpState = new PlayerWallJumpState (this, StateMachine, playerData, "inAir");
        LedgeClimbState = new PlayerLedgeClimbState (this, StateMachine, playerData, "ledgeClimbState");
        DashState = new PlayerDashState (this, StateMachine, playerData, "inAir");
        AttackState = new PlayerAttackState (this, StateMachine, playerData, "attack1");
        AirAttackState = new PlayerAirAttackState (this, StateMachine, playerData, "attack2");
    }

    private void Start () {
        Anim = GetComponent<Animator> ();
        InputHandler = GetComponent<PlayerInputHandler> ();
        RB = GetComponent<Rigidbody2D> ();
        PS = GameObject.Find ("LaunchPoint").GetComponent<PlayerShoot> ();
        minimap = GameObject.Find ("MinimapCamera").GetComponent<Camera> ();
        DashDirectionIndicator = transform.Find ("DashDirectionIndicator");

        FacingDirection = 1;

        StateMachine.Initialize (IdleState);
        respawnCount = 0;
    }

    private void Update () {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate ();

        Anim.SetFloat ("yVelocity", RB.velocity.y);

        if (Time.time >= lastDamageTime + stunRecoveryTime) {
            ResetStunResistance ();
        }

        if (powerupActive && Time.time >= lastPowerupTime + 5f) {
            ResetPowerup ();
        }

        CheckKnockback ();
        UpdateHealthBar ();
        Checkpoint ();
        if (minimap != null) {
            UpdateCameraPosition ();
        } else {
            Debug.Log ("NULL CAMERA");
        }

        SwitchScenes ();

        if (Input.GetKeyDown (KeyCode.Q))
            Application.Quit ();
    }

    private void SwitchScenes () {
        // essentially skips a level since it will load the following scene
        if (Input.GetKeyDown (KeyCode.P)) {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);

            Scene currentScene = SceneManager.GetActiveScene ();
            string sceneName = currentScene.name;
            if (sceneName != "Intro") {
                //Write some text to the .txt file
                StreamWriter writer = new StreamWriter (path, true);
                writer.WriteLine ("Skipped");
                writer.Close ();
            }
        }

        // resetting current scene
        if (Input.GetKeyDown (KeyCode.R)) {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
        }

        // loads the game menu while in game-play
        if (Input.GetKeyDown (KeyCode.M)) {
            SceneManager.LoadSceneAsync ("GameMenu");
        }
    }

    private void FixedUpdate () {
        StateMachine.CurrentState.PhysicsUpdate ();
    }
    #endregion

    #region Set Functions

    public void SetVelocityZero () {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    public void SetVelocity (float velocity, Vector2 angle, int direction) {
        angle.Normalize ();
        workspace.Set (angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocity (float velocity, Vector2 direction) {
        workspace = direction * velocity;
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityX (float velocity) {
        workspace.Set (velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY (float velocity) {
        if (powerupActive) {
            velocity *= 2;
        }
        workspace.Set (CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetHealth () {
        currentHealth = maxHealth;
    }

    #endregion

    #region Getter Functions

    public float GetHealth () {
        return currentHealth;
    }

    public bool GetCheckpointStatus () {
        return checkpointMet;
    }

    #endregion

    #region Check Functions

    public bool CheckIfGrounded () {
        return Physics2D.OverlapCircle (groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall () {
        return Physics2D.Raycast (wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingLedge () {
        return Physics2D.Raycast (ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWallBack () {
        return Physics2D.Raycast (wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public void CheckIfShouldFlip (int xInput) {
        if (xInput != 0 && xInput != FacingDirection) {
            Flip ();
        }
    }

    #endregion

    #region Other Functions

    public Vector2 DetermineCornerPosition () {
        RaycastHit2D xHit = Physics2D.Raycast (wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        float xDist = xHit.distance;
        workspace.Set (xDist * FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast (ledgeCheck.position + (Vector3) (workspace), Vector2.down, ledgeCheck.position.y - wallCheck.position.y, playerData.whatIsGround);
        float yDist = yHit.distance;

        workspace.Set (wallCheck.position.x + (xDist * FacingDirection), ledgeCheck.position.y - yDist);
        return workspace;
    }

    private void AnimationTrigger () => StateMachine.CurrentState.AnimationTrigger ();

    private void AnimtionFinishTrigger () => StateMachine.CurrentState.AnimationFinishTrigger ();

    private void CheckAttackHitBox () {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll (attack1HitBoxPos.position, playerData.attack1Radius, whatIsDamageable);

        attackDetails.damageAmount = playerData.attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = playerData.stunDamageAmount;

        foreach (Collider2D collider in detectedObjects) {
            if (collider.gameObject.name == "Boss") {
                collider.transform.SendMessage ("Damage", attackDetails);
            } else
                collider.transform.parent.SendMessage ("Damage", attackDetails);
            //Instantiate hit particle
        }
    }

    private void FinishAttack1 () {
        Anim.SetBool ("isAttacking", false);
        Anim.SetBool ("firstAttack", false);
        Anim.SetBool ("attack1", false);
        StateMachine.ChangeState (IdleState);
    }

    private void FinishSecondAttack () {
        Anim.SetBool ("isAttacking", false);
        Anim.SetBool ("firstAttack", false);
        Anim.SetBool ("attack2", false);
        StateMachine.ChangeState (InAirState);
    }

    public void Knockback (int direction) {
        Debug.Log ("player knockback");
        knockback = true;
        knockbackStartTime = Time.time;
        RB.velocity = new Vector2 (knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockback () {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback) {
            knockback = false;
            Debug.Log ("Stopped knockback");
            RB.velocity = new Vector2 (0.0f, RB.velocity.y);
        }
    }

    public virtual void ResetStunResistance () {
        isStunned = false;
        currentStunResistance = stunResistance;
    }

    public virtual void ResetPowerup () {
        Debug.Log ("Stopping powerup");
        powerupActive = false;
        workspace.Set (CurrentVelocity.x, playerData.jumpVelocity);
        CurrentVelocity = workspace;
        // power up will reappear in the world
        GameObject e = Instantiate (Resources.Load ("Prefabs/LevelObjects/Collectables/timer1") as GameObject);
        e.transform.position = powerUpPostion; // location change
    }

    public virtual void Damage (AttackDetails attackDetails) {
        GameObject.Instantiate (damageParticle, transform.position, damageParticle.transform.rotation);
        lastDamageTime = Time.time;

        Debug.Log ("Player takes " + attackDetails.damageAmount + " damage");
        currentHealth -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;

        int direction;
        if (attackDetails.position.x < transform.position.x) direction = 1;
        else direction = -1;

        Knockback (direction);

        if (currentStunResistance <= 0) {
            isStunned = true;
        }

        if (currentHealth <= 0) {
            incrementSpawnCount ();
            isDead = true;
            Anim.SetBool ("dead", isDead);
            deadTime = Time.time;
        }
    }

    private void Flip () {
        FacingDirection *= -1;
        transform.Rotate (0.0f, 180.0f, 0.0f);
    }

    private void UpdateHealthBar () {
        float healthPercent = (float) currentHealth / maxHealth;
        if (healthPercent >= 0) {
            healthBar.setSize (new Vector3 (1.0f * healthPercent, 1.0f));
        }
        flashtimer -= Time.deltaTime;
        if (healthPercent < 0.3f) {
            if (flashtimer <= 0) {
                healthBar.setColor (Color.white);
                flashtimer = 0.3f;
            } else {
                healthBar.setColor (Color.red);
            }
        }
    }

    // function for checking if checkpoints are met when you lose all health
    public void Checkpoint () {
        if (isDead) {
            if (checkpointMet) {
                if (Time.time >= deadTime + 0.4f) {
                    RespawnAtLastCheckPoint ();
                }
            } else {
                SceneManager.LoadSceneAsync ("GameMenu");
            }
        }
    }

    // respawns character at last known checkpoint and restores health
    public void RespawnAtLastCheckPoint () {
        Debug.Log ("Respawn");
        isDead = false;
        currentHealth = 50;
        healthBar.setColor (Color.red);
        gameObject.transform.position = checkpointPostion; // location change
        Anim.SetBool ("dead", isDead);
    }

    public void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.tag == "Power") {
            powerUpPostion = collision.transform.position;
            powerupActive = true;
            lastPowerupTime = Time.time;
            Destroy (collision.gameObject);
        }
        if (collision.gameObject.tag == "Coin") {
            coinCount++;
            Destroy (collision.gameObject);
        }
        if (collision.gameObject.tag == "Checkpoint") {
            checkpointPostion = collision.transform.position;
            checkpointMet = true;
            Destroy (collision.gameObject);
        }

        if (collision.gameObject.tag == "EndLevel") {
            EndOfLevel ();
        }
    }

    public void EndOfLevel () {
        //Write some text to the .txt file
        StreamWriter writer = new StreamWriter (path, true);
        writer.WriteLine ("Completed " + coinCount + " " + respawnCount);
        writer.Close ();
    }

    public void incrementSpawnCount () {
        respawnCount++;
    }

    public string CoinStatus () {
        return "Found " + coinCount + " Gold";
    }

    private void UpdateCameraPosition () {
        Vector3 pos = transform.position;
        pos.z = -10f;
        minimap.gameObject.transform.position = pos;
    }

    #endregion
}