using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {
	float moveSpeed = 10f;

	private Rigidbody2D rb;

	private GameObject target;
	private Vector2 moveDirection;

	public float groundCheckRadius;
    public float wallCheckDistance;

    private AttackDetails attackDetails;

    [SerializeField]
    private float stunDamageAmount = 1f;

    [SerializeField]
    private float attack1Radius, attack1Damage;

    [SerializeField]
    private LayerMask whatIsDamageable;

    [SerializeField]
    private Transform attack1HitBoxPos;
    public Transform groundCheck;
    public Transform wallCheck;

    private bool isGrounded;
    private bool isTouchingWall;

    public LayerMask whatIsGround;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		target = GameObject.Find ("Player");
		moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
		rb.velocity = new Vector2 (moveDirection.x, moveDirection.y);
		
	}

	private void Update() {
	    CheckSurroundings();
        CheckAttackHitBox();
	}

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        if(isGrounded || isTouchingWall) {
            Destroy(gameObject);
        }
    }

	private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;
        bool hit = false;

		foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.SendMessage("Damage", attackDetails);
			Destroy(gameObject);
		}
	}

	private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
		Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}