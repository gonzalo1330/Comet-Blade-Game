using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBehavior : MonoBehaviour {
    private const float moveSpeed = 20f;
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
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private GameObject director = null;

    // Start is called before the first frame update
    void Start() {
        mLifeCount = kLifeTime;
        Destroy(gameObject, 3f);
        rb = GetComponent<Rigidbody2D> ();
        director = GameObject.Find("Director");
    }

    // Update is called once per frame
    void Update() {
        RaycastHit2D hitInfo = Physics2D.Raycast(attack1HitBoxPos.position, transform.up, whatIsDamageable);
        if (hitInfo.collider != null) {
            attackDetails.damageAmount = attack1Damage;
            attackDetails.position = transform.position;
            attackDetails.stunDamageAmount = stunDamageAmount;
            if (hitInfo.collider.gameObject.tag == "Enemy") {
                GetComponent<Collider>().transform.parent.SendMessage("Damage", attackDetails);
                Destroy(gameObject);
            }
        }

        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
    }

    void FixedUpdate() {
        CheckSurroundings();
    }

    private void CheckSurroundings()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(attack1HitBoxPos.position, transform.up, whatIsDamageable);

        if(isGrounded || isTouchingWall) {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        print(collision.gameObject.name);
        GameObject parent = collision.transform.parent.gameObject;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}