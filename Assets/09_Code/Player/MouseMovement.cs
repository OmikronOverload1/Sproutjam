using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100.0f; // Sensitivity of mouse movement
    [SerializeField] private Transform playerBody; // Reference to the player's body for horizontal rotation

    private float xRotation = 0.0f; // To keep track of vertical rotation

    void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the player body horizontally (yaw)
        playerBody.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically (pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp vertical rotation to prevent flipping
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
