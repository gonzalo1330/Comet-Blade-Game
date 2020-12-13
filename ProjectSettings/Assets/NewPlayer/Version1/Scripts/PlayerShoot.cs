using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    private Vector3 direction;
    private float startTime;

    private void Update() {
        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        transform.rotation =  Quaternion.Euler (new Vector3(0f,0f,angle + 90f ));
    }
 
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void Launch() {
        if(Time.time >= startTime + 0.5f) {
            startTime = Time.time;
            Debug.Log("Launching Fireball");
            GameObject e = Instantiate(Resources.Load("Prefabs/Player/fireball") as GameObject);
            e.transform.position = transform.position;
            e.transform.up =  transform.up;
        }
    }
    
}
