using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    private AttackDetails attackDetails;
    private float attack1Damage = 10f;
    private float stunDamageAmount = 5f;
    private void OnColiisionEnter2D(Collider2D collision) {
        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;
        if(collision.gameObject.tag == "enemy") {
            collision.transform.parent.SendMessage("Damage", attackDetails);
        }
    }
}
