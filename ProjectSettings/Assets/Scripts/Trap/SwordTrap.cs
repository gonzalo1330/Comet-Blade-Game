using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrap : MonoBehaviour
{
    private float riseSpeed = 14f;
    private float timer = 1.5f;
    private SwordTrapState mSwordTrapState = SwordTrapState.RiseState;


    private enum SwordTrapState {
        RiseState,
        IdleState,
        LowerState,
        IdleState2
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFSM();
    }

    private void UpdateFSM() {
        switch(mSwordTrapState) {
            case SwordTrapState.RiseState:
                RiseState();
                break;
            case SwordTrapState.IdleState:
                IdleState();
                break;
            case SwordTrapState.LowerState:
                LowerState();
                break;
            case SwordTrapState.IdleState2:
                IdleState2();
                break;
        }
    }


    void RiseState() {
        Vector3 curr = transform.position;
        curr.y += riseSpeed * Time.deltaTime;
        if (curr.y >= -14.2) {
            mSwordTrapState = SwordTrapState.IdleState;
        } else {
            transform.position = curr;
        }
    }

    void IdleState() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            mSwordTrapState = SwordTrapState.LowerState;
            timer = 1.5f;
        }
    }

    void LowerState() {
        Vector3 curr = transform.position;
        curr.y -= riseSpeed * Time.deltaTime;
        if (curr.y <= -17f) {
            mSwordTrapState = SwordTrapState.IdleState2;
        }
        else {
            transform.position = curr;
        }
    }

    void IdleState2() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            mSwordTrapState = SwordTrapState.RiseState;
            timer = 1.5f;
        }
    }
}
