using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CaptureBehavior : MonoBehaviour
{
    public CaptureBar TheBar = null;

    private GameObject toCapture = null;
    public GameObject director = null;
    Vector3 direction;

    private int fingerID = -1;
    Camera mCam = null;

    void Start()
    {
        mCam = Camera.main;

        Debug.Assert(TheBar != null);
        Debug.Assert(director != null);
        DirPointer();
    }

    void Update()
    {
        DirPointer();
        Highlight();
        SelectCapturedObject();
        CaptureLaunch();
    }

    #region CONTROLS

    void DirPointer()
    {
        Vector3 dirTarget = mCam.ScreenToWorldPoint(Input.mousePosition);
        dirTarget.z = 0;
        direction = (dirTarget - transform.localPosition).normalized;
        director.transform.localPosition = transform.localPosition + direction;
    }

    void SelectCapturedObject()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TheBar.SelectObject(0);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TheBar.SelectObject(1);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TheBar.SelectObject(2);
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            TheBar.IncrementSlot();
        }
    }

    void CaptureLaunch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(fingerID)) return;
            ObjectCapture();
        } else if (Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject(fingerID)) return;
            Launch(-1, direction, director.transform.position);
        }
    }

    #endregion

    #region POINTER_SUPPORT

    private RaycastHit2D PointInDirection()
    {
        Vector2 origin = new Vector2(transform.localPosition.x, transform.localPosition.y);
        Vector2 rayDir = new Vector2(direction.x, direction.y);
        return Physics2D.Raycast(origin, rayDir, Mathf.Infinity, LayerMask.GetMask("Objects"));
    }

    void Highlight()
    {
        GameObject toHighlight = null;
        RaycastHit2D cast = PointInDirection();
        if (cast.collider != null)
        {
            toHighlight = PointInDirection().collider.gameObject;
        }
        else
        {
            toHighlight = null;
        }

        if (toHighlight != null)
        {
            CaptureObject cap = toHighlight.GetComponent<CaptureObject>();
            if (cap != null)
            {
                if (cap.Capturable())
                {
                    //put a highlight behind the object, or enable highlight effect
                }
            }
        }
    }

    #endregion

    #region CAPTURE_SUPPORT

    void ObjectCapture()
    {
        //additional check if in camera
        RaycastHit2D cast = PointInDirection();
        if (cast.collider != null)
        {
            toCapture = PointInDirection().collider.gameObject;
        }
        else
        {
            toCapture = null;
        }

        if (toCapture != null)
        {
            CaptureObject cap = toCapture.GetComponent<CaptureObject>();
            if (cap != null)
            {
                if (cap.Capturable())
                {
                    if (TheBar.GetCount() > 2)
                    {
                        Launch(0, direction, director.transform.position);
                    }
                    cap.IsLaunched = false;
                    TheBar.OnCapture(toCapture);
                }
            }
        }
    }

    void Launch(int toLaunch, Vector3 direction, Vector3 origin)
    {
        Vector2 launchDir = new Vector2(direction.x, direction.y);
        GameObject launched = TheBar.OnLaunch(toLaunch);
        
        if (launched != null)
        {
            launched.transform.position = origin + 0.05f * direction;

            CaptureObject cap = launched.GetComponent<CaptureObject>();
            if(cap != null)
            {
                cap.OnLaunch(direction);
            }
        }
    }

    #endregion
}
