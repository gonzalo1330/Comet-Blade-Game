using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Boss_Combat : MonoBehaviour
{

    public float damage = 5f;
    public float attackRange = 15f;
    public LayerMask playerMask;
    public Transform attackPos;
    public AttackDetails attackDetails;
    // Start is called before the first frame update
    void Start()
    {
        attackDetails.damageAmount = damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack() {
            Vector3 pos = transform.position;
            Collider2D player = Physics2D.OverlapCircle(attackPos.position, attackRange, playerMask);
        if (player != null && player.gameObject.name == "Player")
            player.GetComponent<Player>().Damage(attackDetails);
    }

    void OnDrawGizmosSelected() {
        Vector3 pos = transform.position;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
