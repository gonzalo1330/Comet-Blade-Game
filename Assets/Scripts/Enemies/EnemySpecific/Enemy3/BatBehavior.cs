using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehavior : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    private float currentHealth;
    private float lastDamageTime;

    private Vector2 velocityWorkspace;

    protected bool isStunned;
    protected bool isDead;

    public virtual void Start()
    {
        currentHealth = 30f;
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update() {
        CheckHealth();
    }

    private void CheckHealth() {
        if(currentHealth <= 0) {
            anim.SetBool("dead", true);
        }
    }

    public virtual void DamageHop(float velocity)
    {
        velocityWorkspace.Set(rb.velocity.x, velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual void Damage(AttackDetails attackDetails)
    {
        lastDamageTime = Time.time;

        currentHealth -= attackDetails.damageAmount;

        DamageHop(2f);

        //Instantiate(entityData.hitParticle, aliveGO.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        if(currentHealth <= 0)
        {
            isDead = true;
            gameObject.layer = LayerMask.NameToLayer("Dead");
        }
        else {
            isStunned = true;       // new
        }

    }

    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.name == "fireball") {
            currentHealth -= 10f;
        }
    }
}
