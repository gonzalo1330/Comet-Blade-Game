using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour {
    [SerializeField]
    private float attack1Radius, attack1Damage, stunDamageAmount;
    [SerializeField]
    private LayerMask whatIsDamageable;
    [SerializeField]
    private Transform attack1HitBoxPos;

    private float speed = 10f;
    private AttackDetails attackDetails;

	private void Update() {
        CheckAttackHitBox();
	}

    // Update is called once per frame
    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;
        bool hit = false;

		foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.SendMessage("Damage", attackDetails);
		}
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }

}
