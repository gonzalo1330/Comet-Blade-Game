using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Required to work with UI, e.g., Text

public class GameManager : MonoBehaviour
{
    public static GameManager sTheGlobalBehavior = null;    // Single pattern
    public PlayerControl mHero = null;
    public Text coins = null;
    public Text stopwatch = null;

    void Awake() {
        GameManager.sTheGlobalBehavior = this;    // Singleton pattern
        Debug.Assert(coins != null);      // Assume setting in the editor!
        Debug.Assert(mHero != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        coins.text = mHero.CoinStatus();
        stopwatch.text = mHero.StopwatchTimer();
    }

    
}
