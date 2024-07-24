using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-/////////////////////////////////////////////////////////////////////////////////////////////////
/// 
public enum EnemyState
{
    Default = 0,
        
    Chasing = 1
}

///-/////////////////////////////////////////////////////////////////////////////////////////////////
/// 
public class Enemy : MonoBehaviour
{
    [Header("Enemy Properties")] 
    [SerializeField] private float enemySpeed = 3;

    [SerializeField] private float detectionRadius = 2f;

    private EnemyState _enemyState = EnemyState.Default;
    public EnemyState enemyState => _enemyState;

    private bool detectedPlayer = false;
    private LayerMask playerLayer;

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Update()
    {
        detectedPlayer = CheckForPlayerOverlap();
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    private bool CheckForPlayerOverlap()
    {
        return Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnUpdateEnemyState()
    {
        switch (_enemyState)
        {
            case EnemyState.Default:
                OnUpdateDefaultState();
                break;
            case EnemyState.Chasing:
                OnUpdateChasingState();
                break;
        }
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnUpdateDefaultState()
    {
        
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnUpdateChasingState()
    {
        // If the player is still with in detection
        if (detectedPlayer)
        {
            // Move enemy towards player
        }
        else
        {
            // Enter the default state
            
        }
    }
}
