using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss_Jump : StateMachineBehaviour {

    public float bossRiseSpeed = 5f;
    public Rigidbody2D rb;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Vector2 pos = rb.position;
        if (pos.y <= -45f) {
            pos.y += bossRiseSpeed * Time.deltaTime;
        }
        rb.MovePosition(pos);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }


}