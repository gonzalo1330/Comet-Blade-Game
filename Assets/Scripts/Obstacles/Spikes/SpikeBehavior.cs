using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehavior : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.5f;
    
    private Vector3 originialPos;
    public GameObject player;
    

    void Start() {
        originialPos = GetComponent<Transform>().position;
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= 234f) {
            Vector3 pos = transform.position;
            pos.y += moveSpeed * Time.deltaTime;
            transform.position = pos;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player") {
            transform.position = originialPos;
        }
    }
}
