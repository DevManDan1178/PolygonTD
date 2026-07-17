using UnityEngine;
using UnityEngine.UI;
public class CameraStateButton : MonoBehaviour
{
    public Text cameraStateText;
    void Update()
    {
        cameraStateText.text = CameraController.movement ? "FREE CAM" : "LOCKED CAM";
    }

    public void ProcessToggleCameraLock()
    {
        if (GameManager.paused)
        {
            return;
        }
        CameraController.movement = !CameraController.movement;
    }
}
