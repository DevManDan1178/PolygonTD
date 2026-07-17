using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CompleteLevel : MonoBehaviour
{
    public string menuSceneName = "Main Menu";
    public string nextLevel;
    public SceneFader sceneFader;
    public Button nextLevelButton;
    // Start is called before the first frame update
    void OnEnable(){
        if(nextLevel == "" || SceneManager.GetSceneByName(nextLevel) == null){ //disable button if it wont work
        
            nextLevelButton.enabled = false;
            nextLevelButton.GetComponent<Image>().color = new Color32 (200, 200, 200, 200);
        }
    }
    public void PlayNextLevel()
    {
        sceneFader.FadeTo(nextLevel);
    }
    public void ReturnToMenu()
    {
        AudioManager.instance.StopAllSounds();
        sceneFader.FadeTo(menuSceneName);
    }
}
