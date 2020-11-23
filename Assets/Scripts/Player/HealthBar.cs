using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform bar;
    private float originalVal;
    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");
        bar.localScale = new Vector3(1.0f, 1.0f);
        originalVal = transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setSize(Vector3 size) {
        bar.localScale = size;
    }

    public void setColor(Color color) {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = color;
    }
}
