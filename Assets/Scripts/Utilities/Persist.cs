using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required to work with UI, e.g., Text
using UnityEngine.SceneManagement;

public class Persist : MonoBehaviour {
    static Persist ToPersist = null;
    bool TheOne = false;

    public int[] CoinCount;
    public int[] RespawnCount;

    // Start is called before the first frame update
    void Start () {
        if (!TheOne) {
            if (ToPersist == null) {
                ToPersist = this;
                TheOne = true;
                DontDestroyOnLoad (gameObject);
            } else {
                Destroy (gameObject);
            }
        }
        CoinCount = new int[3];
        RespawnCount = new int[3];

        CoinCount[0] = 100;
    }

    public void IncrementCoins () {
        // will read in the current scene
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene ();
        // Retrieve the name of this scene.
        string sceneName = currentScene.name;
        if (sceneName == "Level1") {
            CoinCount[0]++;
        } else if (sceneName == "Level2") {
            CoinCount[1]++;
        } else if (sceneName == "BossLevel") {
            CoinCount[2]++;
        }
    }

    public void IncrementDeaths () {
        // will read in the current scene
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene ();
        // Retrieve the name of this scene.
        string sceneName = currentScene.name;
        if (sceneName == "Level1") {
            RespawnCount[0]++;
        } else if (sceneName == "Level2") {
            RespawnCount[1]++;
        } else if (sceneName == "BossLevel") {
            RespawnCount[2]++;
        }
    }

    public string getLevel1 () {
        return CoinCount[0].ToString ();
    }

    public string getLevel2 () {
        return CoinCount[1].ToString ();
    }

    public string getLevel3 () {
        return CoinCount[2].ToString ();
    }
}