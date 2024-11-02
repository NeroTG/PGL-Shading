// using UnityEngine;

// public class CameraFollow : MonoBehaviour
// {
//     public Transform player;       // Referensi ke transform player
//     public Vector3 offset;         // Offset kamera dari player
//     public float smoothSpeed = 0.125f; // Kecepatan smooth follow

//     void LateUpdate()
//     {
//         // Menentukan posisi target kamera
//         Vector3 desiredPosition = player.position + offset;
        
//         // Menghaluskan pergerakan kamera
//         Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
//         // Mengatur posisi kamera
//         transform.position = smoothedPosition;
//     }
// }
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;               // Referensi ke transform player
    public Vector3 offset;                 // Offset kamera dari player
    public float smoothSpeed = 0.125f;     // Kecepatan smooth follow
    public float minDistance = 1f;         // Jarak minimum (untuk FPP)
    public float maxDistance = 10f;        // Jarak maksimum (untuk TPP)
    public float mouseSensitivity = 2f;    // Sensitivitas rotasi kamera saat FPP

    private float distance;                // Jarak kamera saat ini
    private float rotationX = 0f;          // Rotasi vertikal
    private float rotationY = 0f;          // Rotasi horizontal

    void Start()
    {
        distance = maxDistance;            // Awal dengan jarak maksimum (TPP)
        Vector3 angles = transform.eulerAngles;
        rotationX = angles.x;
        rotationY = angles.y;
    }

    void LateUpdate()
    {
        // Input scroll mouse untuk mengatur jarak kamera
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            distance -= scrollInput * 5f;                       // Ubah jarak berdasarkan input scroll
            distance = Mathf.Clamp(distance, minDistance, maxDistance); // Batasi jarak
        }

        if (distance > minDistance)
        {
            // TPP Mode: Kamera mengikuti player dengan smooth
            Vector3 desiredPosition = player.position + offset - transform.forward * distance;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            transform.LookAt(player); // Pastikan kamera tetap melihat ke arah player
        }
        else
        {
            // FPP Mode: Kamera terkunci di posisi dekat dan hanya berputar berdasarkan mouse
            rotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationY += Input.GetAxis("Mouse X") * mouseSensitivity;

            rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Batasi rotasi vertikal

            transform.position = player.position + offset; // Tempatkan kamera di titik dekat player
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0); // Rotasi berdasarkan input mouse
        }
    }
}


