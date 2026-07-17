using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
public class RealInputReader : MonoBehaviour
{
    
    private Vector3 prevMousePosition = new Vector3(0,0,0);
    void Awake()
    {
        foreach (var module in FindObjectsByType<BaseInputModule>())
        {
            module.enabled = false;
        }
    }
    
    void Update()
    {
        if (!InputBridge.instance)
        {
            UIInput.Process();
        }
        if (InputBridge.realInputReaderDisabled)
        {
            return;
        }
        float x = Input.mousePosition.x/Screen.width;
        float y = Input.mousePosition.y/Screen.height;

        if (Input.GetMouseButtonDown(0)){
            InputBridge.PointerDown(x, y);
        }
        if (prevMousePosition != Input.mousePosition)
        {
            InputBridge.MouseMove(x, y);
            prevMousePosition = Input.mousePosition;
        }
    }

    private void LateUpdate()
    {
        if (InputBridge.instance)
        {
            return;
        }
        GameInput.FrameOver();
    }

}