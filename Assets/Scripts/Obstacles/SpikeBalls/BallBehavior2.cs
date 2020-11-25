using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior2 : MonoBehaviour {
    private float speed = 10f;
    public LayerMask whatIsGround;
    private bool isGrounded = false;
    private float collisonRadius = 0.8f;
    private BallBehaviorState mState = BallBehaviorState.LowerState;
    private enum BallBehaviorState {
        RiseState,
        LowerState
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        isGrounded = Physics2D.OverlapCircle(transform.position, collisonRadius, whatIsGround);
        FSM();
    }

    void FSM() {
        switch (mState) {
            case BallBehaviorState.LowerState:
                LowerState();
                break;
            case BallBehaviorState.RiseState:
                RiseState();
                break;
        }
    }

    void RiseState() {
        Vector3 curr = transform.position;
        curr.y += speed * Time.deltaTime;
        if (isGrounded) {
            mState = BallBehaviorState.LowerState;
            curr.y -= 2 * speed * Time.deltaTime;
            transform.position = curr;
        }
        else {
            transform.position = curr;
        }
    }

    void LowerState() {
        Vector3 curr = transform.position;
        curr.y -= speed * Time.deltaTime;
        if (isGrounded) {
            mState = BallBehaviorState.RiseState;
            curr.y += 2 * speed * Time.deltaTime;
            transform.position = curr;
        }
        else {
            transform.position = curr;
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, collisonRadius);
    }
}
