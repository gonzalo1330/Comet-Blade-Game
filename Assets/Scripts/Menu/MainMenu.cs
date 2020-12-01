using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void PlayGame () {
        SceneManager.LoadSceneAsync ("Intro");
    }

    // these function will give the options menu the ability to switch between levels
    public void LoadLevel1 () {
        SceneManager.LoadSceneAsync ("Level1");
    }

    public void LoadLevel2 () {
        SceneManager.LoadSceneAsync ("Level2");
    }
    public void LoadBossLevel () {
        SceneManager.LoadSceneAsync ("BossLevel");
    }

    public void QuitGame () {
        Application.Quit ();
    }
}