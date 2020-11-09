using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundMask;
    public LayerMask objectMask;

    const int MAX_JUMPS = 1;
    public int jumpsRemaining;

    float kMove;
    float kJump;
    float collisionCheck;

    void Start()
    {
        kMove = 5f;
        kJump = 7f;

        collisionCheck = 0.1f;

        jumpsRemaining = MAX_JUMPS;
        body = gameObject.GetComponent<Rigidbody2D>();

        Debug.Assert(body != null);

        //floorCheck = gameObject.GetComponent<Collider2D>().bounds.extents.y;
        //wallCheck = gameObject.GetComponent<Collider2D>().bounds.extents.x;
    }
    
    void Update()
    {
        if (IsGroundedGround() || IsGroundedObject())
        {
            jumpsRemaining = MAX_JUMPS;
        }
        Move();
        Jump();
    }

    void FixedUpdate()
    {
        
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if(wallCheck.localPosition.x < 0)
            {
                wallCheck.localPosition = -wallCheck.localPosition;
            }
            if (!AgainstWall())
                transform.localPosition += new Vector3(kMove * Time.smoothDeltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (wallCheck.localPosition.x > 0)
            {
                wallCheck.localPosition = -wallCheck.localPosition;
            }
            if (!AgainstWall())
                transform.localPosition -= new Vector3(kMove * Time.smoothDeltaTime, 0, 0);
        }
    }

    void Jump()
    {
        //https://stackoverflow.com/questions/25350411/unity-2d-jumping-script
        if (Input.GetKeyDown(KeyCode.W) && jumpsRemaining > 0 && body != null)
        {
            if(jumpsRemaining < MAX_JUMPS)
            {
                body.velocity = new Vector2(body.velocity.x, 0f);
            }
            jumpsRemaining--;
            body.AddForce(new Vector2(0,kJump), ForceMode2D.Impulse);
        }
    }

    //https://answers.unity.com/questions/196381/how-do-i-check-if-my-rigidbody-player-is-grounded.html
    //https://www.youtube.com/watch?v=86Bgt--Ww7w&list=PLiyfvmtjWC_Up8XNvM3OSqgbJoMQgHkVz
    private bool IsGroundedGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, collisionCheck, groundMask);
    }

    private bool IsGroundedObject()
    {
        return Physics2D.OverlapCircle(groundCheck.position, collisionCheck, objectMask);
    }

    private bool AgainstWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, collisionCheck, groundMask);
    }
}
