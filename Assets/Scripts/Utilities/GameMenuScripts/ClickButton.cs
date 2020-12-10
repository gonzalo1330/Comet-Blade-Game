using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour {
    public string path = "Assets/statistics.txt"; // path to the file
    private Scene scene;
    private bool open = false;

    void Start() {
        scene = SceneManager.GetActiveScene();
    }

    public void OnClickInput (InputAction.CallbackContext context) {
        if (context.started) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint (Mouse.current.position.ReadValue ());
            Vector2 mousePos2D = new Vector2 (mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast (mousePos2D, Vector2.zero);
            if (hit.collider != null) {
                if (hit.collider.gameObject.name == "PlayButton") {
                    SceneManager.LoadSceneAsync ("Intro");
                    // clears all content from the text file when the user plays again
                    System.IO.File.WriteAllText (path, string.Empty);
                    Debug.Log (hit.collider.gameObject.name + "Button Clicked");
                } else if (hit.collider.gameObject.name == "OptionsButton") {
                    SceneManager.LoadSceneAsync ("OptionsMenu");
                    Debug.Log (hit.collider.gameObject.name + "Button Clicked");
                } else if (hit.collider.gameObject.name == "QuitButton") {
                    Application.Quit ();
                    Debug.Log (hit.collider.gameObject.name + "Button Clicked");
                } else if (hit.collider.gameObject.name == "BackButton") {
                    SceneManager.LoadSceneAsync ("GameMenu");
                    Debug.Log (hit.collider.gameObject.name + "Button Clicked");
                } else if (hit.collider.gameObject.name == "MenuButton") {
                    GameObject cMenu = GameObject.Find("ControlsMenu");
                    if(cMenu != null) {
                        if(open == false) {
                            open = true;
                            cMenu.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else {
                            open = false;
                            cMenu.GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }
                    else {
                        Debug.Log("Bruh aint no sih here");
                    }
                    
                    Debug.Log (hit.collider.gameObject.name + "Button Clicked");
                }
            }
        }
    }
}