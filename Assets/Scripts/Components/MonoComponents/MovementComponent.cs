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
    public Vector2 targetPosition;
    [HideInInspector]
    public Vector2 directionToTarget;
    [HideInInspector]
    public float sqrDistanceToTarget;
}

public class MovementComponent : MonoBehaviour
{
    const float TARGET_DISTANCE_THRESHOLD = 0.005f;

    /// <summary>
    /// In code event, call only when the target has been reach and there is now no target
    /// </summary>
    public delegate void TargetUpdate();
    public event TargetUpdate OnTargetReached;
    public event TargetUpdate OnTargetCancelled;

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

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        CalculateVelocity();
        ApplyVelocity();
    }

    public void SetMaxSpeed(float newSpeed)
    {
        movementStats.maxSpeed.Value = newSpeed;
        movementStats.speed.Value = Math.Min(newSpeed, movementStats.speed.Value);
    }

    public void SetMaxSpeed(SharedValue<float> newSpeed)
    {
        movementStats.maxSpeed.SharedValueReference = newSpeed;
        movementStats.speed.Value = Math.Min(newSpeed.Value, movementStats.speed.Value);
    }

    public void SetMovementTarget(Vector2 newTarget)
    {
        targetData.targetPosition = newTarget;
        if (targetData.hasTarget == false && TargetSetEvent != null)
        {
            TargetSetEvent.Invoke();
        }

        targetData.hasTarget = true;
    }

    public void ClearMovementTarget()
    {
        targetData.hasTarget = false;
        OnTargetCancelled?.Invoke();
    }

    protected virtual void CalculateVelocity()
    {
        if (!TrySetVelocityToTarget())
        {
            velocity = Vector2.zero;
            Deccelerate();
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
                Accelerate();
            }
            return true;
        }

        return false;
    }

    protected void UpdateTargetData()
    {
        Vector2 distance = targetData.targetPosition - GetPosition2D();
        targetData.sqrDistanceToTarget = distance.sqrMagnitude;
        targetData.directionToTarget = distance.normalized;
    }

    protected Vector2 GetPosition2D()
    {
        return (Vector2)transform.position;
    }

    private void SetVelocityToTarget()
    {
        float currentSpeed = movementStats.speed.Value;
        float speedModifier = currentSpeed * Time.deltaTime;
        velocity = targetData.directionToTarget * speedModifier;
    }

    private void Deccelerate()
    {
        movementStats.speed.Value = Math.Max(movementStats.speed.Value - Time.deltaTime, 0.0f);
    }

    private void Accelerate()
    {
        movementStats.speed.Value = Math.Min(movementStats.speed.Value + Time.deltaTime, movementStats.maxSpeed.Value);
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
}
