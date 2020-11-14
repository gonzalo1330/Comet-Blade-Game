using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CaptureBehavior : MonoBehaviour
{
    public CaptureBar TheBar = null;
    public GameObject director = null;

    private GameObject toCapture = null;
    Vector3 direction;

    private int fingerID = -1;
    Camera mCam = null;

    
    // Start is called before the first frame update
    void Start()
    {
        mCam = Camera.main;

        Debug.Assert(TheBar != null);
        Debug.Assert(direction != null);
        Debug.Assert(mCam != null);
        DirPointer();
    }

    // Update is called once per frame
    void Update()
    {
        DirPointer();
        SelectCapturedObject();
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(fingerID)) return;
            CaptureObject();
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject(fingerID)) return;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                TheBar.Place(mCam.ScreenToWorldPoint(Input.mousePosition));
            } else
            {
                TheBar.Launch(-1,direction,director.transform.position);
            }
        }
    }

    void DirPointer()
    {
        Vector3 dirTarget = mCam.ScreenToWorldPoint(Input.mousePosition);
        dirTarget.z = 0;
        direction = (dirTarget - transform.localPosition).normalized;
        director.transform.localPosition = transform.localPosition + direction;
    }

    //we want to highlight objects which you can capture
    void Highlight()
    {
        RaycastHit2D cast = PointInDirection();
        if (cast.collider != null)
        {
            toCapture = PointInDirection().collider.gameObject;
        }

        if (toCapture != null)
        {
            if (toCapture.GetComponent<CrateBehavior>() != null)
            {
                if (toCapture.GetComponent<CrateBehavior>().Capturable())
                {
                    //put a highlight behind the object, or enable highlight effect
                }
            }
        }
        toCapture = null;
    }

    //change which captured object is selected
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

    void CaptureObject()
    {
        //additional check if in camera
        //aditional check if hit ground first
        RaycastHit2D cast = PointInDirection();
        if(cast.collider != null)
        {
            toCapture = PointInDirection().collider.gameObject;
        }
        
        if(toCapture != null) {
            if(toCapture.GetComponent<CrateBehavior>() != null)
            {
                if (toCapture.GetComponent<CrateBehavior>().Capturable())
                {
                    TheBar.CaptureObject(toCapture, direction, director.transform.position);
                }
            }
        }
        toCapture = null;
    }
    
    private RaycastHit2D PointInDirection()
    {
        Vector2 origin = new Vector2(transform.localPosition.x, transform.localPosition.y);
        Vector2 rayDir = new Vector2(direction.x, direction.y);
        return Physics2D.Raycast(origin, rayDir, Mathf.Infinity, LayerMask.GetMask("Objects"));
    }
}
