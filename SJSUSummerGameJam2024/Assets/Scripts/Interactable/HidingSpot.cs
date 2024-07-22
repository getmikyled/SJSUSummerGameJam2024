using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-/////////////////////////////////////////////////////////////////////////////////////////////////
/// 
public class HidingSpot : InteractableObject
{
    [SerializeField] private Transform _exitSpot;
    public Transform exitSpot => _exitSpot;

    public override void Interact()
    {
        PlayerCharacterController.instance.EnterHidingSpot(this);
    }
}
