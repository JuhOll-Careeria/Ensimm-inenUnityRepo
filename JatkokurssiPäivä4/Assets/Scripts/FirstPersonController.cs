using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour
{
    public float speed = 3.0f;       //  speed
    public float jumpHeight = 2.0f;      // Jump height
    public float mouseSensitivity = 2.0f; // Mouse sensitivity for looking around

    private float rotationX = 0f;        // Rotation around X axis (up/down)
    private Rigidbody rb;
    private Camera playerCamera;
    private Vector3 targetVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        // Lock cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = targetVelocity;
    }

    void HandleMouseLook()
    {
        // Look around using the mouse
        rotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);  // Prevent camera from rotating beyond vertical limits

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
    }

    void HandleMovement()
    {
        // Handle movement input (WASD or arrow keys)
        float moveDirectionX = Input.GetAxis("Horizontal") * speed;
        float moveDirectionZ = Input.GetAxis("Vertical") * speed;

        Vector3 move = transform.right * moveDirectionX + transform.forward * moveDirectionZ;

        // Apply movement to the Rigidbody
        targetVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && rb.linearVelocity.y > -0.01f && rb.linearVelocity.y < 0.01f)
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
    }
}