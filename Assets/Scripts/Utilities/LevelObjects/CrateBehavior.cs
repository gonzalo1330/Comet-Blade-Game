//CrateBehavior
//Author:   Nick Soerens
//Purpose:  Behaviors for crates, which can hold down switches and be used as platforms

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBehavior : MonoBehaviour
{
    public bool canCapture;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CanCapture() { canCapture = true; }
    public bool Capturable() { return canCapture; }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        //If player character, be pushed in response
        //If capture and hit by capture ray, disappear to open slot location
    }
}
