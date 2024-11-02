using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float runSpeedMultiplier = 2f;
    public float jumpForce = 5f;
    public float cameraSpeed = 2f;
    public float mouseSensitivity = 100f;
    public float fallThreshold = -10f; // Batas y untuk respawn jika jatuh

    private Rigidbody rb;
    private Renderer playerRenderer;

    public Transform cameraTransform;
    public Transform respawnPoint; // Titik respawn

    private float rotationX = 0f;
    private float rotationY = 0f;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();
        Cursor.lockState = CursorLockMode.Locked;

        // Debug untuk memastikan respawnPoint telah diatur
        if (respawnPoint == null)
        {
            Debug.LogError("Respawn Point belum diatur! Pastikan untuk mengisi respawnPoint di Inspector.");
        }
    }

    void Update()
    {
        // Memanggil fungsi untuk mengecek jika pemain jatuh
        CheckFall();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        Vector3 right = cameraTransform.TransformDirection(Vector3.right);
        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized;

        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= runSpeedMultiplier;
        }

        rb.MovePosition(transform.position + movement * currentSpeed * Time.deltaTime);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeColor();
        }

        RotateCamera();
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void ChangeColor()
    {
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        playerRenderer.material.color = randomColor;
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        rotationY += mouseX;

        cameraTransform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void CheckFall()
    {
        // Mengecek jika posisi y player di bawah fallThreshold
        if (transform.position.y < fallThreshold)
        {
            Debug.Log("Player jatuh! Respawning...");
            Respawn();
        }
    }

    private void Respawn()
    {
        if (respawnPoint != null)
        {
            Debug.Log("Respawn dipanggil. Memindahkan player ke titik respawn.");
            transform.position = respawnPoint.position;
            rb.velocity = Vector3.zero;
        }
        else
        {
            Debug.LogError("Respawn gagal. Respawn Point belum diatur di Inspector.");
        }
    }
}
