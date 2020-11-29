using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{

    public float detectionRange = 25f;
    public LayerMask player;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(animator != null);   
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D findPlayer = Physics2D.OverlapCircle(transform.position, detectionRange, player);
        if (findPlayer) {
            animator.SetTrigger("PlayerDetected");
        }
    }

    void OnDrawGizmosSelected() {
        Vector3 pos = transform.position;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
