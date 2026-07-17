using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text roundsText;
    public SceneFader sceneFader;
    public string menuSceneName = "Main Menu";

    void OnEnable() 
    {
        roundsText.text = PlayerStats.Rounds.ToString();
    }

    public void Retry(){
        Time.timeScale = 1;
        AudioManager.instance.StopAllSounds();
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }
    public void Menu(){
        Time.timeScale = 1;
        AudioManager.instance.StopAllSounds();
        sceneFader.FadeTo(menuSceneName);
    }
}
