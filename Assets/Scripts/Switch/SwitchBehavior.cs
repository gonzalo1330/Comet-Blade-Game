using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehavior : MonoBehaviour {
    public DoorTrigger doorTrig;
    Animator anim;
    private bool active = false;
    private bool stay;
    private HashSet<Collider2D> checkObj = new HashSet<Collider2D>();
    void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        stay = false;
    }

    void OnTriggerStay2D(Collider2D other) {
        // animate door
        print(other);
            doorTrig.Toggle(true);
            anim.SetBool("active", true);
        checkObj.Add(other);
    }
    void OnTriggerExit2D(Collider2D other) {
        checkObj.Remove(other);
        if (checkObj.Count == 0) {
            doorTrig.Toggle(false);
            anim.SetBool("active", false);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, doorTrig.transform.position);
    }
}
