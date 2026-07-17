using UnityEngine;
using UnityEngine.UI;

public class DebugMouse : MonoBehaviour
{
    public Text positionText;
    public Text mouseDownText;

    private bool toggledClick = false;
    // Update is called once per frame
    void Update()
    {
   
        positionText.text = string.Format("{0} {2}\n{1} {3}", GameInput.mousePosition.x, GameInput.mousePosition.y, InputBridge.unitInput.x, InputBridge.unitInput.y);
        if (GameInput.IsMouseNonUIClick())
        {
            toggledClick = !toggledClick;
        }
        mouseDownText.text = (InputBridge.realInputReaderDisabled ? "R-Dis | " : "R-En | ") + (toggledClick ? "A" : "B");
    }
}
