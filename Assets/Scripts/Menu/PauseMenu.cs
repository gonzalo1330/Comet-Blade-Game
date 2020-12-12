using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private bool isPaused = false;
    // Start is called before the first frame update

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("Esc Pressed");
            isPaused = !isPaused;
        }

        if (isPaused) {
            ActivateMenu();
        }
        else {
            DeactivateMenu();
        }
    }


    public void OnEscapeInput(InputAction.CallbackContext context) {
        Debug.Log("Escape");
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
