using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour
{
    public string nextLevel;
    Camera mCam;
    
    void Awake()
    {
        mCam = Camera.main;
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.gameObject.layer == 11)
        {
            if(!nextLevel.Equals(""))
            {
                //Fade to black
                SceneManager.LoadSceneAsync(nextLevel);
            }
        }
    }
}
