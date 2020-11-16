using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlane : MonoBehaviour
{
    public GameObject respawnPoint = null;
    public PlayerStats PS;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(respawnPoint != null);
    }


    private void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.gameObject.tag == "Player") {
            if(PS.GetCheckpointStatus()) {
                PS.RespawnAtLastCheckPoint();
            }
            else {
                Vector3 pos = respawnPoint.transform.position;
                collider.transform.position = pos;
            }
        }
        else {
            Destroy(collider.gameObject);
        }
    }
/*
    private void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.gameObject.tag == "Player") {
            PS.RespawnAtLastCheckPoint();
        }
        else {
            Destroy(collider.gameObject);
        }
    }
*/
}
