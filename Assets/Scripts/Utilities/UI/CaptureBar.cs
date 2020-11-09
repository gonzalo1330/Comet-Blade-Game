using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureBar : MonoBehaviour
{
    Vector3 hideObject = new Vector3(-1000f, -1000f, 0);

    public List<Image> captureSlots = new List<Image>();
    public List<GameObject> captured = new List<GameObject>();

    Color deselected = new Color(1f, 1f, 1f, 0.45f);
    public int selected;

    // Start is called before the first frame update
    void Start()
    {
        selected = 0;
        foreach(Image im in captureSlots)
        {
            im.sprite = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RenderSlots();
    }

    public void CaptureObject(GameObject toCapture, Vector3 dir, Vector3 launchOrg)
    {
        if(captured.Count > 2)
        {
            Launch(0, dir, launchOrg);
        }
        captured.Add(toCapture);
        toCapture.transform.position = hideObject;
    }

    void RenderSlots()
    {
        for(int ndx = 0; ndx < captureSlots.Count; ndx++)
        {
            if(ndx < captured.Count)
            {
                captureSlots[ndx].sprite = captured[ndx].GetComponent<SpriteRenderer>().sprite;
            } else
            {
                captureSlots[ndx].sprite = null;
                captureSlots[ndx].color = Color.clear;
            }
            
            if(selected == ndx)
            {
                captureSlots[ndx].color = deselected;
            } else
            {
                captureSlots[ndx].color = Color.white;
            }
        }
        foreach(GameObject g in captured)
        {
            g.transform.position = hideObject;
            g.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void SelectObject(int sel)
    {
        selected = sel;
        if (selected >= captured.Count)
        {
            selected = 0;
        }
    }

    public void Launch(int toLaunch, Vector3 direction, Vector3 origin)
    {
        Vector2 launchDir = new Vector2(direction.x, direction.y);
        GameObject launched;
        if(captured.Count > 0)
        {
            if (toLaunch < 0)
            {
                launched = captured[selected];
                captured.RemoveAt(selected);
            }
            else
            {
                launched = captured[0];
                captured.RemoveAt(0);
            }
            if(launched != null)
            {
                selected--;
                if (selected < 0) selected = 0;
                launched.transform.position = origin - 0.25f* direction;
                Rigidbody2D body = launched.GetComponent<Rigidbody2D>();
                if(body != null)
                {
                    body.AddForce(launchDir * 10f, ForceMode2D.Impulse);
                }
            }
        }
        
    }

    //TODO: prevent from placing inside level geometry
    public void Place(Vector3 pos)
    {
        GameObject launched;
        pos.z = 0;
        if (captured.Count > 0)
        {
            launched = captured[selected];
            captured.RemoveAt(selected);
            
            if (launched != null)
            {
                launched.transform.position = pos;
            }
        }
    }

    public void IncrementSlot()
    {
        selected++;
        if (selected >= captured.Count)
        {
            selected = 0;
        }
    }
}
