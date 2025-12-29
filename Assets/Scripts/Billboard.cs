using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        // Membuat sprite menatap arah yang sama dengan kamera
        transform.forward = mainCamera.transform.forward;

        // OPSI ALTERNATIF: Jika ingin sprite tegak lurus tapi menatap kamera (gaya Doom/Octopath)
        // transform.rotation = Quaternion.Euler(0f, mainCamera.transform.rotation.eulerAngles.y, 0f);
    }
}