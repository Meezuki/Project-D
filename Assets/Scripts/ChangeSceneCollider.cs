using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneCollider : MonoBehaviour, Interactable
{
    [Header("Settings")]
    public string sceneToLoad;

    [SerializeField] private string _prompt = "Enter Door";
    public string InteractablePrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Switching Scene...");
        SceneManager.LoadScene(sceneToLoad);
        return true;
    }
}