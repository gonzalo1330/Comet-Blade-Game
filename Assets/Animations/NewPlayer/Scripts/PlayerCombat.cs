using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled, specialAttackEnabled;
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;
    [SerializeField]
    private float stunDamageAmount = 1f;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;
    
    private bool gotInput, isAttacking, isFirstAttack;

    private float lastInputTime = Mathf.NegativeInfinity;

    private AttackDetails attackDetails;

    private Animator anim;

    private PlayerStats PS;
    private PlayerControl PC;

    public GameObject bulletPrefab;
    public Transform firePoint;

    // damage
    private float damageHopSpeed = 3f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
        PC = GetComponent<PlayerControl>();
        PS = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckCombatInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            if (combatEnabled)
            {
                //Attempt combat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    public void ToggleSpecialAttack(bool toggle) {
        specialAttackEnabled = toggle;
    }
    
    private void CheckAttacks()
    {
        if (gotInput)
        {
            //Perform Attack1
            if (!isAttacking)
            {
                // set variables
                gotInput = false;
                isAttacking = true;
                
                if(PC.GetGroundedStatus()) {
                    // set animatinos
                    isFirstAttack = true;
                    anim.SetBool("attack1", true);
                }
                else if (!PC.GetGroundedStatus()) {
                    isFirstAttack = false;
                    anim.SetBool("attack2", true);

                    if(specialAttackEnabled) {
                        GameObject e = Instantiate (bulletPrefab, firePoint.position, firePoint.rotation);
                        e.transform.position = firePoint.position;
                        e.transform.up =  firePoint.up;
                    }
                }
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);
            }
        }

        if(Time.time >= lastInputTime + inputTimer)
        {
            //Wait for new input
            gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;

        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);
            //Instantiate hit particle
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
        anim.SetBool("attack2", false);
    }

    private void Damage(AttackDetails attackDetails)
    {
        if (!PC.GetDashStatus())
        {
            int direction;

            PS.DecreaseHealth(attackDetails.damageAmount);

            if (attackDetails.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            PC.Knockback(direction);
        }        
    }

    void OnCollisionEnter2D (Collision2D collision) {
        // collision with enemy missile
        if (collision.gameObject.tag == "Hazard") {
            Debug.Log("Hit by ball");
            PS.DecreaseHealth(10f);
        }
        else if (collision.gameObject.tag == "Egg") {
            Debug.Log("Hit by egg");
            PS.DecreaseHealth(5f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }

}
