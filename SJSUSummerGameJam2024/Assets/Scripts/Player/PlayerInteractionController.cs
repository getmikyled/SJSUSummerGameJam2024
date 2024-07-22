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
    [SerializeField] private float interactionCooldownDuration = 0.25f;
    [SerializeField] private KeyCode interactionKey = KeyCode.Space;

    private bool interactionCoolDownActive = false;

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
        return interactionCoolDownActive == false && playerState != PlayerState.Hiding;
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private InteractableObject GetClosestInteractableObject()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        
        // Track the closest interactable object
        InteractableObject closestInteractableObject = null;
        float closestDistance = Mathf.Infinity;
        
        // Iterate through nearby interactables
        foreach (Collider2D collider in nearbyColliders)
        {
            InteractableObject interactableObject = collider.GetComponent<InteractableObject>();
            if (interactableObject != null)
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
        }

        return closestInteractableObject;
    }

    private IEnumerator TriggerInteractionCoolDown()
    {
        interactionCoolDownActive = true;
        yield return new WaitForSeconds(interactionCooldownDuration);
        interactionCoolDownActive = false;
    }
}
