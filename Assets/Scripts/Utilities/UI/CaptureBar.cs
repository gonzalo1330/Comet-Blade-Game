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
            g.transform.localPosition = hideObject;
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

    public void IncrementSlot()
    {
        selected++;
        if (selected >= captured.Count)
        {
            selected = 0;
        }
    }

    public void OnCapture(GameObject toCapture)
    {
        captured.Add(toCapture);
        toCapture.transform.position = hideObject;
    }

    public GameObject OnLaunch(int toLaunch)
    {
        GameObject g = null;

        if(captured.Count > 0)
        {
            if (toLaunch < 0)
            {
                g = captured[selected];
                captured.RemoveAt(selected);
            }
            else
            {
                g = captured[0];
                captured.RemoveAt(0);
            }
        }
        if (g != null) selected--;
        if (selected < 0) selected = 0;

        return g;
    }

    public int GetCount() { return captured.Count; }
}
