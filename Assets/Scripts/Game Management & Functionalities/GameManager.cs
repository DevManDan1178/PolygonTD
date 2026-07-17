using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public static bool paused = false;
    public int levelIndex;
    [HideInInspector] public static bool gameEnded;
    public GameObject gameOverUI;
    public GameObject victoryUI;
    void Start() {
        gameEnded = false;
        AudioManager.instance.StopAllSounds();
        AudioManager.instance.PlayLevelMusic(levelIndex);
        JSBridge.SendLevelStartingSignal(levelIndex);
    }
    public void CheckEndGame(){
        if(gameEnded) return;
        if(PlayerStats.Lives <= 0)
        {   
            LoseLevel();
        }
    }

    void LoseLevel()
    {   
        JSBridge.SendLevelLostSignal(levelIndex);
        NodeUI.instance.Hide();
        Time.timeScale = 1;
        gameEnded = true;
        AudioManager.instance.StopAllSounds();
        AudioManager.instance.Play("Defeat");     
        gameOverUI.SetActive(true); 
    }

    public void WinLevel()
    {   
        JSBridge.SendLevelClearedSignal(levelIndex);
        NodeUI.instance.Hide();
        Time.timeScale = 1;
        gameEnded = true;
        UnlockNextLevel();

        StartCoroutine(DelayVictoryUI());
    }

    private void UnlockNextLevel()
    {   
        int levelToUnlock = levelIndex + 2; //+1 for level number, +1 for next level
        if(PlayerPrefs.GetInt("levelReached", 1) < levelToUnlock) {
            PlayerPrefs.SetInt("levelReached", levelToUnlock);
            JSBridge.SendPlayerLevelProgressionSignal(levelToUnlock);
        }
    }

    IEnumerator DelayVictoryUI(){
        yield return new WaitForSeconds(1f);     
        AudioManager.instance.StopAllSounds();
        AudioManager.instance.Play("Victory");    
        victoryUI.SetActive(true);
    }
}
