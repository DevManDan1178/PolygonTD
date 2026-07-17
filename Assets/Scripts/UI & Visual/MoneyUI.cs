using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public Text moneyText;

    // Update is called once per frame
    public void UpdateMoney()
    {   float moneyRound = Mathf.Floor(PlayerStats.Money);
        moneyText.text = moneyRound.ToString();
    }
}
