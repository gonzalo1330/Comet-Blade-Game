using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PauseHandler : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private bool isPaused = false;

    void Update() {

        if (isPaused) {
            ActivateMenu();
        }
        else {
            DeactivateMenu();
        }
    }
    public void OnEscapeInput(InputAction.CallbackContext context) {
        if (context.started)
        Debug.Log("Escaped Clicked");
    }

    public void ActivateMenu() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    public void DeactivateMenu() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        AudioListener.pause = false;
    }
}