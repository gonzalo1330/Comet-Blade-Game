using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehavior : MonoBehaviour
{
	public DoorTrigger doorTrig;
	Animator anim;
    private bool active = false; 

	void Start () {
		anim = GetComponent<Animator> ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
        // animate door
		doorTrig.Toggle(true);

        if (other.tag == "Player") {
            Debug.Log("Inside switch");
            anim.SetBool ("active", true);
        }
	}

    void OnTriggerExit2D(Collider2D other){
		doorTrig.Toggle(false);

        if (other.tag == "Player") {
            anim.SetBool ("active", false);
        }
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(transform.position, doorTrig.transform.position);
	}
}
