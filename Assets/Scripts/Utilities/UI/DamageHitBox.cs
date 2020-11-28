using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHitBox : MonoBehaviour
{
    private AttackDetails attackDetails;

    void OnCollisionEnter2D(Collision2D collision) {
        attackDetails.damageAmount = 5f;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = 0f;
        if(collision.gameObject.tag == "Player") {
            collision.transform.SendMessage("Damage", attackDetails);
        }
     }
}
