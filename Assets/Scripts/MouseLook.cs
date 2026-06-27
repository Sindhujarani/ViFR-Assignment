using UnityEngine;

/// <summary>
/// Moves the camera based on mouse input.
/// Simulates VR head movement on desktop.
/// </summary>
public class MouseLook : MonoBehaviour
{
    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;

    private float xRotation = 0f;

    private void Start()
    {
        // Always reset rotation to face forward on game start
        xRotation = 0f;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.parent.rotation = Quaternion.Euler(0f, 0f, 0f);

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X")
                       * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y")
                       * mouseSensitivity * Time.deltaTime;

        // Vertical rotation (up and down) with clamp
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation to camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (left and right)
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}