using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{   
    public string levelToLoad;
    public SceneFader sceneFader;
    void Awake() {
        AudioManager.instance.Play("Lobby");
    }
    public void Play(){
        sceneFader.FadeTo(levelToLoad);
    }
    public void Quit(){
        if (!InputBridge.canQuit)
        {
            Debug.Log("Request for Quit, but InputBridge has it disabled");
            return;
        }
        Debug.Log("Exiting");
        Application.Quit();
    }
}
