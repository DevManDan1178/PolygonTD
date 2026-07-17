using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    public Image img;
    [HideInInspector]
    public static bool fadingIn = false;
    [HideInInspector]
    public static bool fadingOut = false;
    public AnimationCurve fadeCurve;
    void Start() {
        if(!fadingIn)StartCoroutine(FadeIn()); //added if
    }

    public void FadeTo(string scene){
        Time.timeScale = 1;
        AudioManager.instance.UpdateSoundTimeScale();
        JSBridge.SendSceneChangeSignal(scene);
        StartCoroutine(FadeOut(scene));
    }
    IEnumerator FadeIn(){
        if(fadingIn) yield return null;
        fadingIn = true;
        fadingOut = false;
        float time = 1;

        while (time >= 0)
        {
            time -= Time.deltaTime;
            float alpha = fadeCurve.Evaluate(time);
            img.color = new Color (0.1f, 0.1f, 0.1f, alpha);
            yield return 0;
        }
    }
    IEnumerator FadeOut(string scene){
        if(fadingOut) yield return null; //aded
        float time = 0;
        fadingOut = true;
        fadingIn = false;
        
        while (time < 1)
        {
            time += Time.deltaTime;
            float alpha = fadeCurve.Evaluate(time);
            img.color = new Color (0.1f, 0.1f, 0.1f, alpha);
            yield return 0;
        }
        WaveSpawner.EnemiesAlive = 0;
        SceneManager.LoadScene(scene);
        FadeIn();
    }
}
