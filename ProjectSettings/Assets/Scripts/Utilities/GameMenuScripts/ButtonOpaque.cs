using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpaque : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer s = gameObject.GetComponent<SpriteRenderer>();
        Color c = s.color;
        c.a -= 0.99f;
        s.color = c;
    }
}
