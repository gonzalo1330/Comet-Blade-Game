using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player") {
            if(gameObject.name == "LoadLevel2") {
                SceneManager.LoadScene("Level2");
            }
            else if(gameObject.name == "LoadLevel3") {
                SceneManager.LoadScene("BossLevel");
            }
            else if(gameObject.name == "GameOver") {
                SceneManager.LoadScene("GameMenu")
            }
            }
        }
    }
}
