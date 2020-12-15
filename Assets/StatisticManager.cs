using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required to work with UI, e.g., Text

public class StatisticManager : MonoBehaviour {
    public Persist GameStatistics = null;

    public Text level1Coin = null;
    public Text level2Coin = null;
    public Text level3Coin = null;

    public Text level1Respawn = null;
    public Text level2Respawn = null;
    public Text level3Respawn = null;

    // Start is called before the first frame update
    void Start () {
        Debug.Assert (GameStatistics != null);
    }

    // Update is called once per frame
    void Update () {
        level1Coin.text = GameStatistics.CoinCount[0].ToString ();
        /*
        evel2Coin.text = GetLevelStatus ("coin", 1);
        level3Coin.text = GetLevelStatus ("coin", 2);

        level1Respawn.text = GetLevelStatus ("respawn", 0);
        level2Respawn.text = GetLevelStatus ("respawn", 1);
        level3Respawn.text = GetLevelStatus ("respawn", 2);
        */
    }
}