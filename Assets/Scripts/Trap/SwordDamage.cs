using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public AttackDetails attackDetails;
    // Start is called before the first frame update
    void Start()
    {
        attackDetails.damageAmount = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Player")
            collision.gameObject.GetComponent<Player>().Damage(attackDetails);
    }
}
