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
    // Start is called before the first frame update
    void Start() {
        mLifeCount = kLifeTime;
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.right * (kEggSpeed * Time.smoothDeltaTime);
        mLifeCount -= Time.smoothDeltaTime;
        //if (!mWorldBounds.Contains(transform.position)) {
        //    Destroy(transform.gameObject);  // kills self
        //}
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        print(collision.gameObject.name);
        GameObject parent = collision.transform.parent.gameObject;
        //parent.GetComponent<BasicEnemyController>().TakeDamage(5f);
    }
}