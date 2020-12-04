using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSawBehavior : MonoBehaviour {
    [SerializeField]
    private float groundCheckDistance;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool groundDetected;
    private int facingDirection = -1;
    public float moveSpeed = 8f;

    private float startTime;
    private float idleTime;
    private bool isIdleTimeOver;
    private AttackDetails attackDetails;
    private float damage = 15f;

    private void Start() {
        attackDetails.damageAmount = damage;
    }

    void Update() {
        UpdateSidewaysMovingState();
    }

    private void UpdateSidewaysMovingState() {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        if (gameObject.tag == "SidewaysSaw") {
            if (groundDetected) {
                Flip();
            }
            else {
                Vector3 pos = transform.position;
                pos.y += facingDirection * moveSpeed * Time.deltaTime;
                transform.position = pos;
            }
        }
        else if (gameObject.tag == "UprightSaw") {
            if (!groundDetected) {
                Flip();
            }
            else {
                Vector3 pos = transform.position;
                pos.x += facingDirection * moveSpeed * Time.deltaTime;
                transform.position = pos;
            }
        }

    }

    private void Flip() {
        facingDirection *= -1;
        gameObject.transform.Rotate(0.0f, 180.0f, 0.0f);

    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Player") {
            collision.gameObject.GetComponent<Player>().Damage(attackDetails);
        }
    }
}