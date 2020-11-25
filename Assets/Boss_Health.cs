using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Health : MonoBehaviour
{
    public float bossHealth = 120f;
    public GameObject swordTrap;
    public Animator animator;
    public bool special1 = false;
    public float flyTimer = 4f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bossHealth == 80 && !special1) {
            StartCoroutine(Timer());
            DropSword();
            flyTimer = 4f;
        }
        if (flyTimer <= 0) {
            animator.SetBool("Special1", false);
        }
        flyTimer -= Time.deltaTime;

        if (bossHealth == 0) {
            Die();
        }
    }
    public void Die() {
        Destroy(gameObject, 5f);
        bossHealth -= 10;
        animator.SetTrigger("Die");
    }
    public void DropSword() {
        animator.SetBool("Special1", true);
        special1 = true;
        Timer();
    }
    IEnumerator Timer() {
        yield return new WaitForSeconds(2.0f);
        swordTrap.GetComponent<BossSwordTrap>().Drop();
    }
    public void Damage(AttackDetails damageDetail) {
        bossHealth -= damageDetail.damageAmount;
    }
}
