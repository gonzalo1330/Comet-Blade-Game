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

    public void SetRespawn(GameObject SpawnPoint)
    {
        respawnPoint = SpawnPoint;
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.gameObject.layer == 11) {
            if(PS.GetCheckpointStatus()) {
                PS.RespawnAtLastCheckPoint();
            }
            else {
                Vector3 pos = respawnPoint.transform.position;
                collider.transform.position = pos;
                if(PS.GetHealth() < 50f) {
                    PS.SetHealth();
                }
            }
        }
        else {
            if (collider.gameObject.layer != 12)
            {
                Destroy(collider.gameObject);
            }
        }
    }
}


