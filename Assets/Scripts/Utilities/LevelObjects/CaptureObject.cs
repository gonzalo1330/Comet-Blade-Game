using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureObject : MonoBehaviour
{
    public bool CanCapture, IsLaunched, FlyStraight, IsBreakable, IsExplosive;

    Vector3 travDir = Vector3.right;
    Vector3 target = Vector3.zero;

    public Vector3 resetPoint;

    // Start is called before the first frame update
    void Start()
    {
        resetPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region GET_CAPTURE_PROPERTIES

    public bool Capturable() { return CanCapture; }
    public bool Launched() { return IsLaunched; }
    public bool FliesStraight() { return FlyStraight; }
    public bool Breaks() { return IsBreakable; }
    public bool Explodes() { return IsExplosive; }

    #endregion

    public void OnLaunch(Vector3 direction)
    {
        IsLaunched = true;

        Rigidbody2D body = gameObject.GetComponent<Rigidbody2D>();
        if (!FlyStraight)
        {
            if (body != null)
            {
                body.AddForce(direction * 20f, ForceMode2D.Impulse);
            }
        } else
        {
            travDir = direction;
        }
    }

    public void SetTarget(Vector3 targetPos)
    {
        target = targetPos;
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        //If player character, be pushed in response
        //If capture and hit by capture ray, disappear to open slot location
        if (IsLaunched)
        {
            if(collider.gameObject.layer == 8 || collider.gameObject.layer == 12)
            {
                if (IsBreakable)
                {
                    if (IsExplosive)
                    {
                        //do something to explode
                    }
                    Destroy(gameObject);
                }
                IsLaunched = false;
            }
            if (collider.gameObject.layer == 16)
            {
                Debug.Log("Collide kp");
                if (IsBreakable)
                {
                    Destroy(gameObject);
                }
                IsLaunched = false;
                transform.position = resetPoint;
            }
        } else
        {
            if(collider.gameObject.layer == 12)
            {
                CaptureObject cap = collider.gameObject.GetComponent<CaptureObject>();
                if(cap != null)
                {
                    if (cap.Launched())
                    {
                        if (IsBreakable)
                        {
                            if (IsExplosive)
                            {
                                //do something to explode
                            }
                            Destroy(gameObject);
                        }
                    }
                }
            }
            if(collider.gameObject.layer == 16)
            {
                Debug.Log("Collide kp");
                transform.position = resetPoint;
            }
        }
    }
}
