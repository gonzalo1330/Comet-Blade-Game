using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBehavior : MonoBehaviour
{
    [SerializeField]
    private float groundCheckDistance;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool groundDetected;
    private int facingDirection = -1;
    private float moveSpeed = 5f;

    public float startTime;
    private float idleTime;
    private bool isIdleTimeOver;

    void Update() {
        UpdateSidewaysMovingState();
    }

    private void UpdateSidewaysMovingState()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        if(gameObject.tag == "SidewaysSaw") {
            if(groundDetected)
            {
                Flip();
            }
            else
            {
                Vector3 pos = transform.position;
                pos.y += facingDirection * moveSpeed * Time.deltaTime;
                transform.position = pos;
            }
        }
        else if(gameObject.tag == "UprightSaw") {
            if(!groundDetected)
            {
                Flip();
            }
            else
            {
                Vector3 pos = transform.position;
                pos.x += facingDirection * moveSpeed * Time.deltaTime;
                transform.position = pos;
            }
        }

    }

    private void Flip()
    {
        facingDirection *= -1;
        gameObject.transform.Rotate(0.0f, 180.0f, 0.0f);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    }
}


/*
    private State currentState;

    [SerializeField]
    private float
        groundCheckDistance,
        wallCheckDistance,
        movementSpeed;
    [SerializeField]
    private Transform
        groundCheck,
        wallCheck;
    [SerializeField]
    private LayerMask whatIsGround;

    private Vector2 movement;

    private int 
        facingDirection;

    private bool
        groundDetected,
        wallDetected;

    private Rigidbody2D aliveRb;
    private Animator aliveAnim;

    private const float moveSpeed = 20f;

    private void Start()
    {
        aliveRb = GetComponent<Rigidbody2D>();
        aliveAnim = GetComponent<Animator>();

        facingDirection = 1;
    }

    private void Update()
    {
        UpdateMovingState();
    }

    private void UpdateMovingState()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.left, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.up, wallCheckDistance, whatIsGround);

        if(!groundDetected || wallDetected)
        {
            Flip();
        }
        else
        {
            transform.Translate(Vector2.up * facingDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        gameObject.transform.Rotate(0.0f, 180.0f, 0.0f);

    }


    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("inside saw collision");
        if(collision.gameObject.tag == "Waypoint") {
            Debug.Log("flip");
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
    */