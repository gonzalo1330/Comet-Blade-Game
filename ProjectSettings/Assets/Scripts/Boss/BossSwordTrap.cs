using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwordTrap : MonoBehaviour
{
    public float dropSpeed = 20f;
    public bool drop = false;
    public Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        if (drop && pos.y >= -56.5f) {
            pos.y -= dropSpeed * Time.deltaTime;
            transform.position = pos;
        }
        if (pos.y <= -56.5) {
            Destroy(gameObject, 3f);
        }
    }

    public void Drop() {
        drop = true;
    }
}
