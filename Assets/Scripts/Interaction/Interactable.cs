using System;
using UnityEngine;

public interface Interactable
{
    string InteractablePrompt { get; }
    bool Interact(Interactor interactor);
}