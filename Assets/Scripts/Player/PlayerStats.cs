﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {
    // health
    public float health = 50f;
    public float maxHealth = 50f;
    public HealthBar healthBar;
    private float flashtimer = 0.3f;

    // checkpoint
    private bool checkpointMet = false;
    private Vector3 savedPostion;
    public AudioSource hurtSrc;
    public AudioSource source;

    void Start() {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        source = audioSources[0];
        hurtSrc = audioSources[1];
    }

    private void Update () {
        UpdateHealthBar ();
        Checkpoint ();
    }

    public void DecreaseHealth (float damage) {
        hurtSrc.Play ();
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
    public void Checkpoint () {
        if (health == 0) { // met checkpoint but lost all health
            if (checkpointMet) {
                RespawnAtLastCheckPoint ();
                if (GameObject.Find ("Timer1(Clone)") == null) {
                    Debug.Log ("should appear new one");
                    GameObject Timer1 = Instantiate (Resources.Load ("Prefabs/LevelObjects/Collectables/Timer1") as GameObject);
                    Timer1.transform.position = new Vector3 (53, -20, 0);
                }
            } else {
                SceneManager.LoadScene(SceneManager.GetActiveScene ().buildIndex - 1);
            }
        }
    }

    // respawns character at last known checkpoint and restores health
    public void RespawnAtLastCheckPoint () {
        health = 50;
        healthBar.setColor (Color.red);
        gameObject.transform.position = savedPostion; // location change
    }

    public void OnCollisionEnter2D (Collision2D collision) {
        // player located the checkpoint
        if (collision.gameObject.tag == "Checkpoint") {
            source.Play ();
            savedPostion = collision.transform.position;
            checkpointMet = true;
            Destroy (collision.gameObject);
        }
    }
}