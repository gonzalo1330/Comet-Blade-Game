using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlane : MonoBehaviour {
  public GameObject respawnPoint = null;
  public Player stats;

  // Start is called before the first frame update
  void Start () {
    Debug.Assert (respawnPoint != null);
    stats = GameObject.Find ("Player").GetComponent<Player> ();
  }

  public void SetRespawn (GameObject SpawnPoint) {
    respawnPoint = SpawnPoint;
  }

  private void OnCollisionEnter2D (Collision2D collider) {
    if (collider.gameObject.layer == 11) {
      stats.incrementSpawnCount ();
      if (stats.GetCheckpointStatus ()) {
        stats.RespawnAtLastCheckPoint ();
      } else {
        Vector3 pos = respawnPoint.transform.position;
        collider.transform.position = pos;
        if (stats.GetHealth () < 50f) {
          stats.SetHealth ();
        }
      }
    } else {
      if (collider.gameObject.layer != 12 && collider.gameObject.tag != "Enemy") {
        Destroy (collider.gameObject);
      }
    }
  }
}