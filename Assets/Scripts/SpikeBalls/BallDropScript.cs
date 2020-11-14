using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDropScript : MonoBehaviour
{
    public GameObject prefab;
    public float timer = 2;

    // Update is called once per frame
    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            GameObject e = Instantiate(prefab);
            e.transform.localPosition = transform.position;
            timer = 2f;
        }
    }
}
