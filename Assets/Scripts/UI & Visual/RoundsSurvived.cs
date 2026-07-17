using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoundsSurvived : MonoBehaviour
{
    public Text roundsSurvivedText;
    void OnEnable() {
        if(roundsSurvivedText == null) roundsSurvivedText = gameObject.GetComponent<Text>();
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText(){
        roundsSurvivedText.text = "0";
        int round = 0;
        yield return new WaitForSeconds(0.6f);
        while (round < PlayerStats.Rounds)
        {
            round ++;
            roundsSurvivedText.text = round.ToString();
            yield return new WaitForSeconds(0.05f);
        }
    }
}
