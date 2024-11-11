using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MovementStats
{
    public FloatReference speed;
    public FloatReference maxSpeed;
}

public struct MovementFrameData
{

}   

[Serializable]
public struct MovementTargetData
{
    [HideInInspector]
    public bool hasTarget;
    [SerializeField]
    public SharedVector2 targetPosition;
    [HideInInspector]
    public Vector2 directionToTarget;
    [HideInInspector]
    public float sqrDistanceToTarget;
}

public class MovementComponent : MonoBehaviour
{
    const float TARGET_DISTANCE_THRESHOLD = 0.09f;

    /// <summary>
    /// In code event, call only when the target has been reach and there is now no target
    /// </summary>
    public delegate void TargetReached();
    public event TargetReached OnTargetReached;

    /// <summary>
    /// Call only when a new target has been set and there was no target
    /// </summary>
    [SerializeField]
    protected GameEvent TargetSetEvent;

    /// <summary>
    /// Call only when the target has been reach and there is now no target
    /// </summary>
    [SerializeField]
    protected GameEvent TargetReachedEvent;

    [SerializeField]
    protected MovementStats movementStats;
    [SerializeField]
    protected MovementTargetData targetData;

    protected MovementFrameData cachedFrameData;
    protected Vector2 velocity;

    private void Start()
    {
        if (targetData.targetPosition && !targetData.hasTarget)
        {
            targetData.targetPosition.Value = Vector2.zero;
        }
    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        CalculateVelocity();
        ApplyVelocity();
    }

    public void SetMovementTarget(Vector2 newTarget)
    {
        targetData.targetPosition.Value = newTarget;
        if (targetData.hasTarget == false && TargetSetEvent != null)
        {
            TargetSetEvent.Invoke();
        }

        targetData.hasTarget = true;
    }

    protected virtual void CalculateVelocity()
    {
        if (!TrySetVelocityToTarget())
        {
            velocity = Vector2.zero;
        }
    }

    protected void ApplyVelocity()
    {
        transform.position += new Vector3(velocity.x, velocity.y);
    }

    protected bool TrySetVelocityToTarget()
    {
        if (targetData.hasTarget)
        {
            UpdateTargetData();
            if (HasTargetBeenReached())
            {
                TargetHasBeenReached();
            }
            else
            {
                SetVelocityToTarget();
            }
            return true;
        }

        return false;
    }

    private void SetVelocityToTarget()
    {
        float speedModifier = movementStats.speed.Value * Time.deltaTime;
        velocity = targetData.directionToTarget * speedModifier;
    }

    protected void UpdateTargetData()
    {
        Vector2 distance = targetData.targetPosition.Value - GetPosition2D();
        targetData.sqrDistanceToTarget = distance.sqrMagnitude;
        targetData.directionToTarget = distance.normalized;
    }

    private bool HasTargetBeenReached()
    {
        return targetData.sqrDistanceToTarget <= TARGET_DISTANCE_THRESHOLD;
    }

    private void TargetHasBeenReached()
    {
        targetData.hasTarget = false;

        if (OnTargetReached != null)
        {
            OnTargetReached.Invoke();
        }

        if (TargetReachedEvent != null)
        {
            TargetReachedEvent.Invoke();
        }
    }

    protected Vector2 GetPosition2D()
    {
        return (Vector2)transform.position;
    }
}
