using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static float Money;
    public static int Lives;
    public int startMoney = 75;
    public int startLives = 25;
    public static int Rounds;
    public static PlayerStats instance;
    void Start()
    {   instance = this;
        Money = startMoney;
        Lives = startLives;
        Rounds = 0;
        UpdateStats();
    }
    //use if(FindObjectOfType<PlayerStats>() != null) FindObjectOfType<PlayerStats>().UpdateStats(); to update stats in other scripts
    public void UpdateStats()
    {
        //check if game ended, refresh LivesUI, refresh MoneyUI
        if(FindAnyObjectByType<GameManager>() != null) FindAnyObjectByType<GameManager>().CheckEndGame();
        if(FindAnyObjectByType<LivesUI>()!= null) FindAnyObjectByType<LivesUI>().UpdateLives();
        if(FindAnyObjectByType<MoneyUI>()!= null) FindAnyObjectByType<MoneyUI>().UpdateMoney();
    }
}
