using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

///-/////////////////////////////////////////////////////////////////////////////////////////////////
/// 
public partial class PlayerCharacterController
{
    [Header("Interaction")] 
    [SerializeField] private KeyCode interactionKey = KeyCode.Space;
    
    private HashSet<InteractableObject> nearbyInteractableObjects = new HashSet<InteractableObject>();

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void OnTriggerEnter2D(Collider2D other)
    {
        InteractableObject interactableObject = other.GetComponent<InteractableObject>();
        if (interactableObject != null)
        {
            nearbyInteractableObjects.Add(interactableObject);
        }
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void OnTriggerExit2D(Collider2D other)
    {
        InteractableObject interactableObject = other.GetComponent<InteractableObject>();
        if (interactableObject != null)
        {
            nearbyInteractableObjects.Remove(interactableObject);
        }
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void OnUpdateTryInteract()
    {
        if (CanInteract() && Input.GetKeyUp(interactionKey))
        {
            InteractableObject closestInteractableObject = GetClosestInteractableObject();
            if (closestInteractableObject != null)
            {
                closestInteractableObject.Interact();
            }
        }
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private bool CanInteract()
    {
        return playerState != PlayerState.Hiding;
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private InteractableObject GetClosestInteractableObject()
    {
        // Track the closest interactable object
        InteractableObject closestInteractableObject = null;
        float closestDistance = Mathf.Infinity;
        
        // Iterate through nearby interactables
        foreach (InteractableObject interactableObject in nearbyInteractableObjects)
        {
            float distance = Vector2.Distance(interactableObject.transform.position, transform.position);

            // Check if this interactable is closer than the currently tracked one
            if (distance < closestDistance)
            {
                // Set new closest interactable object
                closestDistance = distance;
                closestInteractableObject = interactableObject;
            }
        }

        return closestInteractableObject;
    }
}
