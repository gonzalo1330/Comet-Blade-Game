using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {
    // health
    public HealthBar healthBar;
    public float health = 50f;
    public float maxHealth = 50f;
    private float flashtimer = 0.3f;
    private float damageHopSpeed = 3f;

    // checkpoint
    private bool checkpointMet = false;
    private Vector3 savedPostion;

    private Animator anim;
    private bool dead;

    private float deadTime = Mathf.NegativeInfinity;
    [SerializeField]
    private float deadTimer;

    void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update () {
        UpdateHealthBar ();
        Checkpoint ();
    }

    public void DecreaseHealth (float damage) {
        health -= damage;
    }

    public float GetHealth () {
        return health;
    }

    public void SetHealth() {
        health = maxHealth;
    }

    public bool GetCheckpointStatus() {
        return checkpointMet;
    }

    private void UpdateHealthBar () {
        float healthPercent = (float) health / maxHealth;
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
    public void Checkpoint () 
    {
        if(dead) {
            if (checkpointMet)
            {
                if(Time.time >= deadTime + deadTimer) {
                    RespawnAtLastCheckPoint();
                    RespawnPowerups();
                }
            } 
            else            // if die w/o checkpoint taken back to prev scence 
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene ().buildIndex - 1);
            }
        }
        else {
            if (health <= 0)    // met checkpoint but lost all health
            { 
                //Debug.Log("dead");
                dead = true;
                anim.SetBool("dead", dead);
                deadTime = Time.time;
            }
        }
    }

    // respawns character at last known checkpoint and restores health
    public void RespawnAtLastCheckPoint () {
        Debug.Log("Respawn");
        dead = false;
        health = 50;
        healthBar.setColor (Color.red);
        gameObject.transform.position = savedPostion; // location change
        anim.SetBool("dead", dead);
    }

    private void RespawnPowerups() {
        if (GameObject.Find ("Timer1(Clone)") == null) {
            Debug.Log ("should appear new one");
            GameObject Timer1 = Instantiate (Resources.Load ("Prefabs/LevelObjects/Collectables/Timer1") as GameObject);
            Timer1.transform.position = new Vector3 (53, -20, 0);
        }
    }

    public void OnCollisionEnter2D (Collision2D collision) {
        // player located the checkpoint
        if (collision.gameObject.tag == "Checkpoint") {
            savedPostion = collision.transform.position;
            checkpointMet = true;
            Destroy (collision.gameObject);
        }
    }
}