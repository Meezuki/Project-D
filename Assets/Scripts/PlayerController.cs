using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5f;

    [Header("References")]
    public Rigidbody rb;
    public SpriteRenderer spriteRenderer;
    public Animator animator; // Opsional, jika nanti pakai animasi

    private Vector3 movement;

    void Update()
    {
        // 1. Input Processing (Dilakukan setiap frame)
        // Mengambil input WASD atau Arrow Keys
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Membuat Vector gerakan (X kiri-kanan, Z maju-mundur)
        // .normalized agar jalan miring (diagonal) tidak lebih cepat
        movement = new Vector3(moveX, 0f, moveZ).normalized;

        // 2. Flip Sprite (Agar wajah menghadap arah jalan)
        if (moveX < 0) // Jalan ke Kiri
        {
            spriteRenderer.flipX = true;
        }
        else if (moveX > 0) // Jalan ke Kanan
        {
            spriteRenderer.flipX = false;
        }

        // 3. Animation State (Opsional - untuk nanti)
        // Jika speed > 0, set animasi lari
        if (animator != null)
        {
            animator.SetBool("IsRunning", movement.magnitude > 0);
        }
    }

    void FixedUpdate()
    {
        // 3. Physics Movement (Dilakukan di update fisika)
        // Menggerakkan Rigidbody
        if (movement.magnitude > 0)
        {
            // Pindahkan posisi rigibody
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}