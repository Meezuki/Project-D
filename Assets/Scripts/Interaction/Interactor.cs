using System;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactionMask;

    [Header("Popup UI")]
    [SerializeField] private InteractionPopup _interactionPopup;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;

    private Interactable _currentInteractable;

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(
            _interactionPoint.position,
            _interactionPointRadius,
            _colliders,
            (int)_interactionMask
        );

        if (_numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<Interactable>();

            if (interactable != null)
            {
                // Tampilkan popup saat masuk range
                if (_currentInteractable != interactable)
                {
                    _currentInteractable = interactable;
                    _interactionPopup?.ShowPopup(interactable.InteractablePrompt);
                }

                // Trigger interact
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    interactable.Interact(this);
                }
            }
        }
        else
        {
            // Sembunyikan popup saat keluar range
            if (_currentInteractable != null)
            {
                _currentInteractable = null;
                _interactionPopup?.HidePopup();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}