using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnKP : MonoBehaviour {
    public KillPlane kp = null;

    // Start is called before the first frame update
    void Start () {
        Debug.Assert (kp != null);
    }

    // Update is called once per frame
    void Update () {
        if (PlayerContact ()) { kp.SetRespawn (transform.gameObject); }
    }

    bool PlayerContact () {
        return Physics2D.OverlapCircle (transform.position, 20f, LayerMask.GetMask ("Player"));
    }
}