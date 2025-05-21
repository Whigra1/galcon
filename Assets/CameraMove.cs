using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed = 100f; // Speed multiplier for camera movement

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Debug.Log("Resetting camera");
            transform.position = Vector3.back;
            return;
        }
        
        var horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right
        var verticalInput = Input.GetAxis("Vertical");     // W/S or Up/Down
        var movement = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

}
