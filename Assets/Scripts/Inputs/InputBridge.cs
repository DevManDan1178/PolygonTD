using System.Drawing;
using UnityEngine;

public class InputBridge : MonoBehaviour
{
    public static bool realInputReaderDisabled = false;
    public static bool keyboardInputDisabled = false;
    public static bool canQuit = true;
    public static InputBridge instance;
    public static Vector2 unitInput = new Vector2(0, 0);
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        gameObject.name = "InputBridge";
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void OnMouseMove(string data)
    {
        var p = data.Split(',');
        if (p.Length != 2)
        {
            Debug.LogError("Invalid lenth for inputs to OnMouseMove(\"x, y\")");
        }
        float x = float.Parse(p[0], System.Globalization.CultureInfo.InvariantCulture);
        float y = float.Parse(p[1], System.Globalization.CultureInfo.InvariantCulture);
        
        MouseMove(x, y);
    }

    public static void MouseMove(float x, float y)
    {
        unitInput = new Vector2(x, y);
        GameInput.SetRawMousePosition(x, y);
    }

    public void OnPointerDown(string data)
    {
         var p = data.Split(',');
        if (p.Length != 2)
        {
            Debug.LogError("Invalid lenth for inputs to OnMouseMove(\"x, y\")");
        }
        float x = float.Parse(p[0], System.Globalization.CultureInfo.InvariantCulture);
        float y = float.Parse(p[1], System.Globalization.CultureInfo.InvariantCulture);

        PointerDown(x, y);      
    }
    public static void PointerDown(float x, float y)
    {
        GameInput.SetRawMousePosition(x, y);
        GameInput.SetMouseDown();
       
        // UI check + execution

        GameInput.mouseOverType = UIInput.ProcessClick(GameInput.rawMousePosition);
    }

    private void LateUpdate()
    {
        GameInput.FrameOver();
    }

    private void Update()
    {
        UIInput.Process();
    }

    public void SetCanQuit(string quittingEnabled)
    {
        if (quittingEnabled != "true" && quittingEnabled != "false"){
            Debug.LogWarning("InputBridge.SetCanQuit called with invalid argument :" + quittingEnabled + " is neither \"true\" nor \"false\"");
            return;
        }
        canQuit = quittingEnabled == "true";
    }

    public void SetRealInputReaderDisabled(string readingDisabled)
    {
        if (readingDisabled != "true" && readingDisabled != "false"){
            Debug.LogWarning("InputBridge.SetRealInputReaderDisabled called with invalid argument :" + readingDisabled + " is neither \"true\" nor \"false\"");
            return;
        }
        realInputReaderDisabled = readingDisabled == "true";
    }

    public void SetKeyboardInputDisabled(string keyboardDisabled)
    {
        if (keyboardDisabled != "true" && keyboardDisabled != "false"){
            Debug.LogWarning("InputBridge.SetKeyboardInputDisabled called with invalid argument :" + keyboardDisabled + " is neither \"true\" nor \"false\"");
            return;
        }
        keyboardInputDisabled = keyboardDisabled == "true";
    }
}