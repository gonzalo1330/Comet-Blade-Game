using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
	Animator anim;
    private float open;

    // Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	public void OpenDoor()
	{
        Debug.Log("Set bool");
        anim.SetBool ("Opens", true);
	}

	public void CloseDoor()
	{
        open = 0;
		anim.SetBool ("Opens", false);
	}

	public void CollEnable()
	{
		GetComponent<BoxCollider2D>().enabled = true;
	}

	public void CollDisable()
	{
		GetComponent<BoxCollider2D>().enabled = false;
	}

}
