using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMovement : MonoBehaviour {
  private float kliffSpeed = 25f;
  public AudioSource source;
  // Start is called before the first frame update
  void Start () {
    source = GetComponent<AudioSource> ();
  }

  // Update is called once per frame
  void Update () {
    if (Input.GetKey (KeyCode.A)) // left movement
      transform.position -= transform.right * (kliffSpeed * Time.smoothDeltaTime);
    if (Input.GetKey (KeyCode.D)) // right movement
      transform.position += transform.right * (kliffSpeed * Time.smoothDeltaTime);
    if (Input.GetKey (KeyCode.W)) // up direction movement
      transform.position += transform.up * (kliffSpeed * Time.smoothDeltaTime);
    if (Input.GetKey (KeyCode.S)) // left direction movement
      transform.position -= transform.up * (kliffSpeed * Time.smoothDeltaTime);

    if (Input.GetKeyDown (KeyCode.Space)) {
      // do the dash
      source.Play ();
    }
  }
}