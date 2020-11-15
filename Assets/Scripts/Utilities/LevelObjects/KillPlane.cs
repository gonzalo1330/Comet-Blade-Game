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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.gameObject.layer != 9 || collider.gameObject.layer != 10 || collider.gameObject.layer != 13)
        {
            collider.gameObject.transform.position = respawnPoint.transform.position;
        } else
        {
            //destroy enemies
        }
    }
}
