using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
 	public DoorBehavior door;
	public bool ignoreTrigger;
	SpriteRenderer sprite;
	private bool trig = false;

	private void Start() {
		sprite = gameObject.GetComponent<SpriteRenderer>();
	}

	public void Toggle(bool state)
	{
		trig = state;
		if (state) {
			door.OpenDoor();
			door.CollDisable();
		}
		else {
			door.CloseDoor();
			door.CollEnable();
		}
	}


	void OnDrawGizmos()
	{
		if (!ignoreTrigger) {
			BoxCollider2D box = GetComponent<BoxCollider2D>();
			Gizmos.DrawWireCube(transform.position, new Vector2(box.size.x,box.size.y));
		}
	}
}

/*
	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("enter");
		if (ignoreTrigger) {
			return;
		}

		if (other.tag == "Player") {
			door.OpenDoor();
		}	 
		
		sprite.sortingLayerName = "Foreground Decoration";
	}

	void OnTriggerExit2D(Collider2D other){
		if (ignoreTrigger) {
			return;
		}

		if (other.tag == "Player") {
			door.CloseDoor();
		}
	}
	*/