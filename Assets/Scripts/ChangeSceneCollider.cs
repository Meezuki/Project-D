using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneCollider : MonoBehaviour
{
    [Header("Settings")]
    public string sceneToLoad;

    private void OnTriggerEnter(Collider other)

    {
        Debug.Log("Collider Hit");
        // Check if the object entering is the Player
        // (Make sure your Player has the tag "Player")
        if (other.CompareTag("Player"))
        {
            Debug.Log("Switching Scene...");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}