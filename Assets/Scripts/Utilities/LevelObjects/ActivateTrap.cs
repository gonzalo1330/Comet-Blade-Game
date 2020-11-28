using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrap : MonoBehaviour
{
    public void OnCollisionEnter2D (Collision2D collision) {
        if(collision.gameObject.tag == "Player") {
            // change layer so dont collide with player
            gameObject.layer = LayerMask.NameToLayer("Player");

            // close door
            GameObject box = GameObject.Find("CaptureCrate6");
            box.SetActive(false);

            // activate lava
            GameObject spikes = GameObject.Find("RisingSpikes");
            spikes.GetComponent<SpriteRenderer>().enabled = true;
            spikes.GetComponent<SpikeBehavior>().enabled = true;
            GameObject environment = Instantiate(Resources.Load("Prefabs/Obstacles/LavaEnvironment") as GameObject);
            SpriteRenderer s = environment.GetComponent<SpriteRenderer>();
            Color c = s.color;
            c.a -= 0.75f;
            s.color = c;
        }
    }
}
