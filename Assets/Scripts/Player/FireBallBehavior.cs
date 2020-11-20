using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBehavior : MonoBehaviour {
    private const float kEggSpeed = 20f;
    private const float kLifeTime = 10; // Alife for this number of cycles
    private float mLifeCount = 0;
    private static Cursor sCursor = null;
    private Bounds mWorldBounds;
    static public void SetCursor(Cursor g) { sCursor = g; }

    public float groundCheckRadius;
    public float wallCheckDistance;

    private AttackDetails attackDetails;

    [SerializeField]
    private Transform attack1HitBoxPos;

    [SerializeField]
    private float stunDamageAmount = 1f;

    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;

    [SerializeField]
    private LayerMask whatIsDamageable;

    public Transform groundCheck;
    public Transform wallCheck;

    private bool isGrounded;
    private bool isTouchingWall;

    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start() {
        mLifeCount = kLifeTime;
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.right * (kEggSpeed * Time.smoothDeltaTime);
        mLifeCount -= Time.smoothDeltaTime;

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
            collider.transform.parent.SendMessage("Damage", attackDetails);
            hit = true;
            //Instantiate hit particle
        }
        if (hit)
            Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        print(collision.gameObject.name);
        GameObject parent = collision.transform.parent.gameObject;
        CheckAttackHitBox();
        //parent.GetComponent<BasicEnemyController>().TakeDamage(5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}