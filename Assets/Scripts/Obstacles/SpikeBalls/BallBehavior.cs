using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour {
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
    void Start()
    {
        Destroy(gameObject, 15f);
    }

    // Update is called once per frame
    void Update()
    {
    }   

}
