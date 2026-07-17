using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{

    public GameObject uI;
    private bool CameraMovement;
    public SceneFader sceneFader;
    public string menuSceneName = "Main Menu";
    private float previousTimeScale = 1f;
    void Update() {
        if(!InputBridge.keyboardInputDisabled && Input.GetKeyDown(KeyCode.Escape)){
            TogglePause();
        }
    }

    public void TogglePause(){
        NodeUI.instance.Hide();
        GameManager.paused = !uI.activeSelf;
        uI.SetActive(GameManager.paused);
        
        if(GameManager.paused){
            if (Shop.instance)
            {
                Shop.instance?.CancelBuild();
            }
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            AudioManager.instance.PauseAllSounds();
            if(Camera.main.GetComponent<CameraController>() != null)
            {
                CameraMovement = CameraController.movement;
            }

            CameraController.movement = false;
        } else{
            AudioManager.instance.ResumeAllSounds();
            Time.timeScale = previousTimeScale;
            CameraController.movement = CameraMovement;
        }
        JSBridge.SendPauseToggledSignal(GameManager.paused);
    }

    public void Retry(){
        TogglePause();
        AudioManager.instance.StopAllSounds();
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu(){
        TogglePause();
        AudioManager.instance.StopAllSounds();
        sceneFader.FadeTo(menuSceneName);
    }

    public void SetPaused()
    {
        if (!GameManager.paused)
        {
            TogglePause();
        }
    }
}
