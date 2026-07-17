
using UnityEngine;
public static class GameInput
{
    public static bool mouseDownThisFrame = false;

    public static Vector2 rawMousePosition;
    public static Vector2 mousePosition;
    public static MouseOverType mouseOverType;
    private static bool processedMouseDown = false;

    public static void SetRawMousePosition(float xRelative, float yRelative)
    {
        InputBridge.unitInput = new Vector2(xRelative, yRelative);
        rawMousePosition = new Vector2((int)(xRelative * Camera.main.pixelWidth), (int)(yRelative * Camera.main.pixelHeight));
    }

    public static void SetMouseDown()
    {   
        mouseDownThisFrame = true;
        processedMouseDown = false;
    }

    public static void SyncMousePosition()
    {
        mousePosition = rawMousePosition;
    }
    public static void FrameOver()
    {
        if (processedMouseDown && mouseDownThisFrame)
        {
            mouseDownThisFrame = false;
            processedMouseDown = false;
        } else if (mouseDownThisFrame)
        {
            processedMouseDown = true;
        }
        SyncMousePosition();
    }


    public static bool IsMouseClick()
    {
        if (mouseDownThisFrame)
        {
            processedMouseDown = true;
            return true;
        }
        return false;
    }

    public static bool IsMouseNonUIClick()
    {
        if (mouseDownThisFrame)
        {
            processedMouseDown = true;
        }
        return mouseDownThisFrame && !mouseOverType.Equals(MouseOverType.UI);
    }

    public static Vector3 getMousePositionVec3()
    {
        return new Vector3(mousePosition.x, mousePosition.y, 0);
    }

    public enum MouseOverType
    {
        UI, 
        OBJECT,
        NULL,
    }
}