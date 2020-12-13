using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_Health : MonoBehaviour {
    public float bossHealth = 160f;
    public GameObject swordTrap;
    public Animator animator;
    public bool special1 = false;
    public float flyTimer = 4f;
    public GameObject player;
    bool hurt = false;
    Color prevColor;
    GameObject head;

    // Start is called before the first frame update
    void Start () {
        Debug.Assert (player != null);
        head = GameObject.Find ("Head");
        var renderer = head.GetComponent<Renderer> ();
        prevColor = renderer.material.GetColor ("_Color");
    }

    // Update is called once per frame
    void Update () {
        if (bossHealth == 100 && !special1) {
            StartCoroutine (Timer ());
            DropSword ();
            flyTimer = 4f;
        }
        if (bossHealth == 80) {
            animator.SetBool ("IsEnraged", true);
        }
        if (flyTimer <= 0) {
            animator.SetBool ("Special1", false);
        }
        flyTimer -= Time.deltaTime;

        if (bossHealth == 0) {
            Die ();
        }
        HitColor ();
    }
    public void Die () {
        player.GetComponent<Player> ().EndOfLevel ();
        Destroy (gameObject, 5f);
        bossHealth -= 10;
        animator.SetTrigger ("Die");
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
    }
    public void DropSword () {
        animator.SetBool ("Special1", true);
        special1 = true;
        Timer ();
    }
    IEnumerator Timer () {
        yield return new WaitForSeconds (2.0f);
        swordTrap.GetComponent<BossSwordTrap> ().Drop ();
    }
    public void Damage (AttackDetails damageDetail) {
        bossHealth -= damageDetail.damageAmount;
        hurt = true;
        var renderer = head.GetComponent<Renderer> ();
        prevColor = renderer.material.GetColor ("_Color");
        HitColor ();
    }

    public void HitColor () {
        var renderer = head.GetComponent<Renderer> ();
        if (hurt) {
            renderer.material.SetColor ("_Color", Color.red);
            hurt = false;
        } else {
            renderer.material.SetColor ("_Color", prevColor);
        }
    }
}