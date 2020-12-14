using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required to work with UI, e.g., Text
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    public static GameManager sTheGlobalBehavior = null; // Single pattern
    public Player mHero = null;
    public Text coins = null;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private bool isPaused = false;

    void Awake() {
        GameManager.sTheGlobalBehavior = this; // Singleton pattern
        Debug.Assert(coins != null); // Assume setting in the editor!
        Debug.Assert(mHero != null);
    }
    // Update is called once per frame
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
        coins.text = mHero.CoinStatus();
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