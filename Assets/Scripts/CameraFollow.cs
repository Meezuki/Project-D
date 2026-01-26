using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Karakter yang mau diikuti

    [Header("Settings")]
    [Range(1f, 10f)]
    public float smoothSpeed = 5f; // Kecepatan kamera mengejar (semakin kecil semakin lambat/smooth)

    private Vector3 offset; // Jarak antara kamera dan pemain

    // Camera limits
    [Header("X Axis Limits")]
    public bool enableLimit = true;
    public float minX = -10f;
    public float maxX = 20f;


    void Start()
    {
        // Hitung jarak awal antara kamera dan pemain saat game dimulai
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate() // LateUpdate dijalankan SETELAH pemain bergerak, biar tidak jitter/getar
    {
        if (target == null) return;

        // 1. Tentukan posisi tujuan (Posisi pemain + jarak offset)
        Vector3 targetPosition = target.position + offset;

        // 2. Gerakkan kamera secara halus (Smooth) dari posisi sekarang ke tujuan
        // Lerp = Linear Interpolation (Perpindahan bertahap)


        if (enableLimit)
        {
            float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition = new Vector3(clampedX, targetPosition.y, targetPosition.z);
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // 3. Terapkan posisi baru
        transform.position = smoothedPosition;
    }
}