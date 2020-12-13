using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private float startTime;
    void Start() {
        startTime = Time.time;
    }
    void Update() {
        FinishAnim();
    }
    private void FinishAnim()
    {
        if(Time.time >= startTime + 0.3f) {
            Destroy(gameObject);
        }
    }
}
