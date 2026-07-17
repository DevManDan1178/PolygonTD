using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGameSpeed : MonoBehaviour
{
    public Button toggleButton;
    public Text toggleButtonText;
    // Start is called before the first frame update

    public void ToggleSpeed(){
        if(Time.timeScale == 1f){ Time.timeScale = 2f;
            toggleButton.GetComponent<Image>().color = new Color32(245,160,170,255);
            toggleButtonText.text = "Reset Speed";
        }else if(Time.timeScale == 2f){ Time.timeScale = 1f;
            toggleButton.GetComponent<Image>().color = new Color32(190,255,170,255);
            toggleButtonText.text = "Speed Up x2";
        }
        AudioManager.instance.UpdateSoundTimeScale();
    }

    void Update ()
    {
        if (!InputBridge.keyboardInputDisabled && (Input.GetKeyDown(KeyCode.LeftShift)) && !GameManager.paused)
        {
            ToggleSpeed();
        }
    }
}
