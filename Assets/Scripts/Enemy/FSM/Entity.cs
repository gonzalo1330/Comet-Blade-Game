using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the entity class itself will now be put on a game object
// the different enemy classes that derive from entity will be put on game objects
public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public Data_Entity entityData;

    public int facingDirection { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGObject { get; private set; }

    [SerializeField]
    private Transform wallCheck;
    private Transform ledgeCheck;

    private Vector2 velocityManip;
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        //aliveGObject = gameObject.Find("Alive");

        // every object will have an FSM instance
        stateMachine = new FiniteStateMachine();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate() {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity) {
        velocityManip.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityManip;
    }

    public virtual bool CheckWall() {
        // position to check from; direction; distance to check; 
        return Physics2D.Raycast(wallCheck.position, aliveGObject.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckLedge() {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }
}
