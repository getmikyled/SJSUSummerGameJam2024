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

    [Header("Components")] 
    [SerializeField] private Rigidbody2D _rigidbody;

    // Pathing
    [Header("Pathing")] 
    [SerializeField] private Transform[] pathPoints;

    private Transform targetPathPoint;
    private int pathPointIndex = 0;
    private int pathPointDirection = 1;
    
    private EnemyState _enemyState = EnemyState.Default;
    public EnemyState enemyState => _enemyState;

    private bool detectedPlayer = false;
    private LayerMask playerLayer;

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
        targetPathPoint = pathPoints[pathPointIndex];
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    private void SetEnemyState(EnemyState newEnemyState)
    {
        EnemyState prevEnemyState = enemyState;
        _enemyState = newEnemyState;

        switch (prevEnemyState)
        {
            case EnemyState.Default:
                break;
            case EnemyState.Chasing:
                break;
        }
        
        switch (_enemyState)
        {
            case EnemyState.Default:
                break;
            case EnemyState.Chasing:
                break;
        }
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Update()
    {
        detectedPlayer = CheckForPlayerOverlap();
        OnUpdateEnemyState();
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
        if (detectedPlayer == false || PlayerCharacterController.instance.IsHiding())
        {
            // Check if reached current target path point
            if (Vector2.Distance(transform.position, targetPathPoint.position) < 0.05f)
            {
                NextTargetPathPoint();
            }
            
            // Move enemy along path
            Vector2 direction = GetDirectionTo(targetPathPoint);
            _rigidbody.velocity = direction * enemySpeed;
        }
        else
        {
            // Raycast to player, checking if player is in site
            
            // If raycast hit, enter chasing state
            SetEnemyState(EnemyState.Chasing);
        }
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    private void NextTargetPathPoint()
    {
        int nextPathIndex = pathPointIndex + pathPointDirection;

        if (nextPathIndex >= pathPoints.Length)
        {
            pathPointDirection = -1;
            targetPathPoint = pathPoints[pathPointIndex];
        }
        else if (nextPathIndex < 0)
        {
            pathPointDirection = 1;
            pathPointIndex += pathPointIndex;

            targetPathPoint = pathPoints[pathPointIndex];
        }
        else
        {
            pathPointIndex += pathPointDirection;
            targetPathPoint = pathPoints[pathPointIndex];
        }
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    private Vector2 GetDirectionTo(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        return direction.normalized;
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnUpdateChasingState()
    {
        // If the player is still with in detection
        if (detectedPlayer && PlayerCharacterController.instance.IsHiding() == false)
        {
            // Move enemy towards player
            Vector2 direction = GetDirectionTo(PlayerCharacterController.instance.transform);
            _rigidbody.velocity = direction * enemySpeed;
        }
        else
        {
            // Enter the default state
            SetEnemyState(EnemyState.Default);
            
            // Reset velocity
            _rigidbody.velocity = Vector2.zero;
        }
    }
}
