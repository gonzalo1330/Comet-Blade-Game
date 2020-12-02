using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ClickButton : MonoBehaviour {

    public void OnClickInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null) {
                
                Debug.Log(hit.collider.gameObject.name + " Clicked");
                SceneManager.LoadSceneAsync("Intro");
            }
        }
    }

}