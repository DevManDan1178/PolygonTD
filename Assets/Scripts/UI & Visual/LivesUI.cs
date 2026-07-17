using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private  Text livesText;
    [SerializeField] private  Image livesBar;
    public void UpdateLives(){
        livesText.text = Mathf.Clamp(PlayerStats.Lives, 0, Mathf.Infinity).ToString() + "/" + PlayerStats.instance.startLives.ToString() +" HP";
        livesBar.fillAmount = (float)PlayerStats.Lives / PlayerStats.instance.startLives;
    }
}
