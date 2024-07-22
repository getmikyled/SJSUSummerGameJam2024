using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-/////////////////////////////////////////////////////////////////////////////////////////////////
/// 
public enum PlayerState
{
    Default = 0,
    
    Hiding = 1
}

///-/////////////////////////////////////////////////////////////////////////////////////////////////
/// 
public partial class PlayerCharacterController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float _speed = 5f;

    [Header("Components")] 
    [SerializeField] private Rigidbody2D _rigidbody;

    private PlayerState playerState = PlayerState.Default;
    
    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Update()
    {
        OnUpdatePlayerMovement();
        
        // PlayerInteractionController
        OnUpdateTryInteract();
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    private void SetPlayerState(PlayerState newPlayerState)
    {
        PlayerState prevPlayerState = playerState;
        playerState = newPlayerState;

        switch (prevPlayerState)
        {
            case PlayerState.Default:
            case PlayerState.Hiding:
                break;
        }

        switch (playerState)
        {
            case PlayerState.Default:
            case PlayerState.Hiding:
                break;
        }
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void OnUpdatePlayerMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector3(horizontalInput, verticalInput).normalized;

        _rigidbody.position += direction * (_speed * Time.deltaTime);
    }
}
