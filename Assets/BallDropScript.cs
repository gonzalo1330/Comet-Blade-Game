using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDropScript : MonoBehaviour
{
    public float timer = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            GameObject e = Instantiate(Resources.Load("Prefabs/SpikyBall") as GameObject);
            e.transform.localPosition = transform.position;
            timer = 2f;
        }
    }
}
