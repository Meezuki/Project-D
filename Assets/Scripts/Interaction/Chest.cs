using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, Interactable
{
    [SerializeField] private string _prompt = "Open Chest";
    [SerializeField] private int _goldReward = 20;
    
    private bool _isOpened = false;

    public string InteractablePrompt => _isOpened ? "Already Opened" : _prompt;

    public bool Interact(Interactor interactor)
    {
        if (_isOpened) return false;

        _isOpened = true;
        CurrencyManager.Instance.AddGold(_goldReward);
        Debug.Log("Chest opened! +" + _goldReward + " Gold");
        return true;
    }
}