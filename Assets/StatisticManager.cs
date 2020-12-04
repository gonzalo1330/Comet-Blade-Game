using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required to work with UI, e.g., Text

public class StatisticManager : MonoBehaviour {
    public string path = "Assets/statistics.txt";

    public Text level1Status = null;
    public Text level2Status = null;
    public Text level3Status = null;

    public Text level1Coin = null;
    public Text level2Coin = null;
    public Text level3Coin = null;

    public Text level1Respawn = null;
    public Text level2Respawn = null;
    public Text level3Respawn = null;

    public string[] LevelStatus;
    public int[] CoinCount;
    public int[] RespawnCount;

    // Start is called before the first frame update
    void Start () {
        LevelStatus = new string[3];
        CoinCount = new int[3];
        RespawnCount = new int[3];

        // will read in the current scene
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene ();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (sceneName == "GameMenu") {
            // clears all content from the text file
            System.IO.File.WriteAllText (path, string.Empty);
        } else {
            ReadDataFile ();
        }
    }

    // Update is called once per frame
    void Update () {
        level1Status.text = GetLevelStatus ("status", 0);
        level2Status.text = GetLevelStatus ("status", 1);
        level3Status.text = GetLevelStatus ("status", 2);

        level1Coin.text = GetLevelStatus ("coin", 0);
        level2Coin.text = GetLevelStatus ("coin", 1);
        level3Coin.text = GetLevelStatus ("coin", 2);

        level1Respawn.text = GetLevelStatus ("respawn", 0);
        level2Respawn.text = GetLevelStatus ("respawn", 1);
        level3Respawn.text = GetLevelStatus ("respawn", 2);
    }

    public string GetLevelStatus (string condition, int i) {
        if (condition == "coin") {
            return CoinCount[i].ToString ();
        } else if (condition == "respawn") {
            return RespawnCount[i].ToString ();
        } else {
            return LevelStatus[i];
        }
    }

    // function that will read the data in the .txt file
    void ReadDataFile () {
        int count = 0;
        var sr = File.OpenText (path);
        var line = sr.ReadLine ();
        for (int i = 0; i < 3; i++) {
            string[] words = line.Split (' ');
            ChangeStat (count, words);
            line = sr.ReadLine ();
            count++;
        }
    }

    // function that will update the array of data in the current object
    void ChangeStat (int i, string[] words) {
        LevelStatus[i] = words[0];
        if (words[0] == "Skipped") {
            CoinCount[i] = 0;
            RespawnCount[i] = 0;
        } else {
            // convert string to int
            int coinCount = System.Int32.Parse (words[1]);
            int respawnCount = System.Int32.Parse (words[2]);
            // now we can set the int[] to be the int that was converted above
            CoinCount[i] = coinCount;
            RespawnCount[i] = respawnCount;
        }
    }
}