﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required to work with UI, e.g., Text

public class GameManager : MonoBehaviour {
    public static GameManager sTheGlobalBehavior = null; // Single pattern
    public Player mHero = null;
    public Text coins = null;

    void Awake () {
        GameManager.sTheGlobalBehavior = this; // Singleton pattern
        Debug.Assert (mHero != null);
    }
}