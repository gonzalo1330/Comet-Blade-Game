using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_EnragedRun : StateMachineBehaviour {
    public Transform player;
    public Rigidbody2D rb;
    public float speed = 2.5f;
    public Boss boss;
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public Transform wallCheck;
    public LayerMask whatIsGround;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    public bool WallCheck() {
        return Physics2D.Raycast(wallCheck.position, wallCheck.right, 1f, whatIsGround);
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        wallCheck = GameObject.FindGameObjectWithTag("BossWallCheck").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        boss.LookAtPlayer();
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        if (!WallCheck())
            rb.MovePosition(newPos);
        if (Vector2.Distance(player.position, rb.position) <= attackRange && attackCooldown <= 0) {
            animator.SetTrigger("EnragedAttack");
            attackCooldown = 2.0f;
        }
        attackCooldown -= Time.deltaTime;
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.ResetTrigger("Attack");
    }

}
