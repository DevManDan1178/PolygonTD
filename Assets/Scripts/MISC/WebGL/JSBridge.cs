using System.Runtime.InteropServices;
using UnityEngine;

public static class JSBridge
{
    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SendStringMessage(string message);
    [DllImport("__Internal")]
    private static extern void SignalLevelCleared(int levelNumber);
    [DllImport("__Internal")]
    private static extern void SignalLevelLost(int levelNumber);
    [DllImport("__Internal")]
    private static extern void SignalLevelStarting(int levelNumber);
    [DllImport("__Internal")]
    private static extern void SignalPauseToggled(int toggled); //0 for false, 1 for true
    [DllImport("__Internal")]
    private static extern void SignalSceneChange(string toSceneName);
    [DllImport("__Internal")]
    private static extern void SignalPlayerLevelProgression(int levelNumber);
    #endif
    private static int lastLevelProgression = -1;
    public static void SendLevelClearedSignal(int levelIndex)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            SignalLevelCleared(levelIndex + 1);
        #endif
            
    }

    public static void SendLevelLostSignal(int levelIndex)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            SignalLevelLost(levelIndex + 1);
        #endif
            
    }

    public static void SendLevelStartingSignal(int levelIndex)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            SignalLevelStarting(levelIndex + 1);
        #endif
            
    }

    public static void SendPauseToggledSignal(bool toggled)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            SignalPauseToggled(toggled ? 1 : 0);
        #endif
            
    }

    public static void SendStringMessageToJS(string message)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            SendStringMessage(message);
        #endif
    }

    public static void SendSceneChangeSignal(string newSceneName)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            SignalSceneChange(newSceneName);
        #endif
    }

    public static void SendPlayerLevelProgressionSignal(int levelNumber)
    {
        if (levelNumber == lastLevelProgression) { //Avoid sending redundant information 
            return;
        }
        #if UNITY_WEBGL && !UNITY_EDITOR
            SignalPlayerLevelProgression(levelNumber);
        #endif
        lastLevelProgression = levelNumber;
    }
}