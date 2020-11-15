using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // health
    public float health = 50f;
    public float maxHealth = 50f;
    public HealthBar healthBar;
    private float flashtimer = 0.3f;

    // checkpoint
    private bool checkpointMet = false;
    private Vector3 savedPostion;

    private void Update() {
        UpdateHealthBar();
        Checkpoint();
    }

    public void DecreaseHealth(float damage) {
        health -= damage;
    }

    public float GetHealth() {
        return health;
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
    private void Checkpoint() {
        if (health == 0)   // met checkpoint but lost all health
        {
            if(checkpointMet) {
                RespawnAtLastCheckPoint ();
            }
            else {
                Application.Quit();
            }
        }
    }
    
    // respawns character at last known checkpoint and restores health
    private void RespawnAtLastCheckPoint () {
        health = 50;
        healthBar.setColor(Color.red);
        gameObject.transform.position = savedPostion; // location change
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
