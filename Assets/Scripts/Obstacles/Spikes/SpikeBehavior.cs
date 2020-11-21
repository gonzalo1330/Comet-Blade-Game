using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehavior : MonoBehaviour
{
    private float moveSpeed = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= 234f) {
            Vector3 pos = transform.position;
            pos.y += moveSpeed * Time.deltaTime;
            transform.position = pos;
        }
    }
}
