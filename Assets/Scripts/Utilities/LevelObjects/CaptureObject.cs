using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureObject : MonoBehaviour
{
    public bool CanCapture, IsLaunched, FlyStraight, IsBreakable, IsExplosive, IsTracking;

    Vector3 travDir = Vector3.right;
    public Vector3 target = Vector3.zero;

    public Vector3 resetPoint;

    // Start is called before the first frame update
    void Start()
    {
        resetPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLaunched && IsTracking)
        {
            Vector3 targetDir = (target - transform.position).normalized;
            if(Vector3.Dot(travDir, targetDir) < 1)
            {
                travDir = Vector3.Lerp(travDir, targetDir, 0.05f);
                transform.up = travDir;
            }

            if((target - transform.position).magnitude < 5f)
            {
                IsTracking = false;
            }
        }
        if(IsLaunched && FlyStraight)
        {
            transform.position +=  0.4f * travDir;
        }
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
            transform.up = direction;
            if (body != null)
            {
                body.isKinematic = true;
                body.useFullKinematicContacts = true;
            }
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
                if (IsBreakable)
                {
                    Destroy(gameObject);
                }
                IsLaunched = false;
                transform.position = resetPoint;
            }
            if (FlyStraight)
            {
                Debug.Log("Missile collided");
                Rigidbody2D body = GetComponent<Rigidbody2D>();
                if(body != null)
                {
                    body.isKinematic = false;
                }
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
                transform.position = resetPoint;
            }
        }
    }
}
