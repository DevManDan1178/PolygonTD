using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _mainCamera;

    [Header("Map background")]
    public GameObject levelBG;

    [Header("Camera movement")]
    public float panSpeed = 30f;
    public static bool movement = false;

    [Header("Camera zoom")]
    public float scrollSpeed = 5f;
    public float maxZoomIn = 5f;
    public float maxZoomOut = 15f;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Update() 
    {
        if (GameManager.gameEnded)
        {
            enabled = false;
            return;
        }
        if (GameManager.paused)
        {
            return;
        }

        // Toggle movement
        if (!InputBridge.keyboardInputDisabled && !InputBridge.keyboardInputDisabled && Input.GetKeyDown(KeyCode.Space))
            movement = !movement;

        if (!movement) return;

        // -------- ZOOM --------
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (!InputBridge.keyboardInputDisabled && Input.GetKey(KeyCode.Q)) {
             scroll -= .01f;
        }
        if (!InputBridge.keyboardInputDisabled && Input.GetKey(KeyCode.E)) {
                scroll += .01f;
        }
        float camZoom = _mainCamera.orthographicSize;

        camZoom -= scroll * 250 * scrollSpeed * Time.deltaTime;
        camZoom = Mathf.Clamp(camZoom, maxZoomIn, maxZoomOut);

        _mainCamera.orthographicSize = camZoom;

        // -------- PANNING --------
        Vector3 input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"),
            0f
        );

        Vector3 newPosition = _mainCamera.transform.position +
                              input.normalized * panSpeed * Time.deltaTime;

        // Apply movement + clamp to bounds
        _mainCamera.transform.position = LimitBounds(newPosition, camZoom);
    }

Vector3 LimitBounds(Vector3 camPosition, float orthographicCamSize)
{
    if (levelBG == null) return camPosition;

    Renderer rend = levelBG.GetComponent<Renderer>();
    if (rend == null) return camPosition;

    Bounds bounds = rend.bounds;

    float camHalfHeight = orthographicCamSize;
    float camHalfWidth = orthographicCamSize * _mainCamera.aspect;

    camPosition.x = Mathf.Clamp(camPosition.x,
        bounds.min.x + camHalfWidth,
        bounds.max.x - camHalfWidth);

    camPosition.y = Mathf.Clamp(camPosition.y,
        bounds.min.y + camHalfHeight,
        bounds.max.y - camHalfHeight);

    return camPosition;
}
}