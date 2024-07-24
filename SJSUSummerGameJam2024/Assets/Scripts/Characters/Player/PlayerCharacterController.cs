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
    public static PlayerCharacterController instance;
    
    [Header("Properties")]
    [SerializeField] private float _speed = 5f;

    [Header("Components")] 
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private Rigidbody2D _rigidbody;
    

    private PlayerState playerState = PlayerState.Default;
    private HidingSpot hidingSpot = null;

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Update()
    {
        OnUpdatePlayerState();
        
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
                break;
            case PlayerState.Hiding:
                OnExitHidingState();
                break;
        }

        switch (playerState)
        {
            case PlayerState.Default:
                break;
            case PlayerState.Hiding:
                OnEnterHidingState();
                break;
        }
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnUpdatePlayerState()
    {
        switch (playerState)
        {
            case PlayerState.Default:
                OnUpdateDefaultState();
                break;
            case PlayerState.Hiding:
                OnUpdateHidingState();
                break;
        }
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    public void EnterHidingSpot(HidingSpot newHidingSpot)
    {
        SetPlayerState(PlayerState.Hiding);
        hidingSpot = newHidingSpot;
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnEnterHidingState()
    {
        // Deactivate the player sprite
        playerSprite.SetActive(false);
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnUpdateHidingState()
    {
        if (Input.GetKeyUp(interactionKey))
        {
            // Exit the hiding state
            SetPlayerState(PlayerState.Default);
        }
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnExitHidingState()
    {
        StartCoroutine(TriggerInteractionCoolDown());
        
        // Set the player's position to the exit spot
        transform.position = hidingSpot.exitSpot.position;
        
        hidingSpot = null;
        
        // Reactivate the player sprite
        playerSprite.SetActive(true);
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void OnUpdateDefaultState()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector3(horizontalInput, verticalInput).normalized;

        _rigidbody.position += direction * (_speed * Time.deltaTime);
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    public bool IsHiding()
    {
        return playerState == PlayerState.Hiding;
    }
}
