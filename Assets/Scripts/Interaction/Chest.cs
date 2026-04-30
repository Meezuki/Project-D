using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Chest : MonoBehaviour, Interactable // Fixed: Interactable (no 'I')
{
    [SerializeField] private string _prompt = "Open Chest";

    public string InteractablePrompt => _prompt; // Fixed: InteractablePrompt

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Opening Chest"); // Fixed: Debug.Log("message")
        return true;
    }
}