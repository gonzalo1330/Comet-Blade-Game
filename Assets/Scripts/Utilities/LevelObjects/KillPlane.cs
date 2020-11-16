using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    public GameObject respawnPoint = null;

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
        if(collider.gameObject.layer == 11)
        {
            collider.gameObject.transform.position = respawnPoint.transform.position;
        } else
        {
            if (collider.gameObject.layer != 12)
            {
                Destroy(collider.gameObject);
            }
        }
    }
}
