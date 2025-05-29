using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera cameraToZoom; // Assign your camera here
    public float zoomSpeed = 2f; // How fast the zoom happens
    public float minOrthographicSize = 5f; // Minimum zoom
    public float maxOrthographicSize = 90f; // Maximum zoom

    void Update()
    {
        // Get the scroll wheel input
        var scrollInput = Input.GetAxis("Mouse ScrollWheel");
        // Change the camera's orthographic size based on scroll input
        if (scrollInput == 0f) return;
        
        cameraToZoom.orthographicSize -= scrollInput * zoomSpeed;
        cameraToZoom.orthographicSize = Mathf.Clamp(cameraToZoom.orthographicSize, minOrthographicSize, maxOrthographicSize);
    }
}