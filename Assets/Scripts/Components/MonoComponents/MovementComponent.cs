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
    public FloatReference accelerationRate;
    public FloatReference deccelerationRate;
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

public class MovementComponent : MonoBehaviour, IRuntimeScriptableObject, IDataProvider<float>
{
    const float TARGET_DISTANCE_THRESHOLD = 0.005f;

    /// <summary>
    /// In code event, call only when the target has been reach and there is now no target
    /// </summary>
    public delegate void TargetUpdate();
    public event TargetUpdate OnTargetReached;
    public event TargetUpdate OnTargetCancelled;

    [SerializeField]
    private SharedFloatDataProvider speedProvider;
    [SerializeField]
    private SharedVector2 velocitySharedCopy;

    /// <summary>
    /// Call only when a new target has been set and there was no target
    /// </summary>
    [SerializeField]
    protected GameEvent targetSetEvent;

    /// <summary>
    /// Call only when the target has been reach and there is now no target
    /// </summary>
    [SerializeField]
    protected GameEvent targetReachedEvent;

    [SerializeField]
    protected MovementStats movementStats;
    [SerializeField]
    protected MovementTargetData targetData;

    protected MovementFrameData cachedFrameData;
    protected Vector2 velocity;

    private void Awake()
    {
        speedProvider.Value = this;
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

    public void SetMaxSpeed(float newSpeed)
    {
        movementStats.maxSpeed.Value = newSpeed;
        movementStats.speed.Value = Mathf.Min(newSpeed, movementStats.speed.Value);
    }

    public void SetMaxSpeed(SharedValue<float> newSpeed)
    {
        movementStats.maxSpeed.SharedValueReference = newSpeed;
        movementStats.speed.Value = Mathf.Min(newSpeed.Value, movementStats.speed.Value);
    }

    public void SetMovementTarget(Vector2 newTarget)
    {
        targetData.targetPosition = newTarget;
        if (targetData.hasTarget == false)
        {
            targetSetEvent?.Invoke();
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

            // For now only sharing movement velocity
            velocitySharedCopy.Value = velocity;
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
        float decceleration = (Time.deltaTime * movementStats.deccelerationRate.Value);
        movementStats.speed.Value = Mathf.Max(movementStats.speed.Value - decceleration, 0.0f);
    }

    private void Accelerate()
    {
        float acceleration = (Time.deltaTime * movementStats.accelerationRate.Value);
        movementStats.speed.Value = Mathf.Min(movementStats.speed.Value + acceleration, movementStats.maxSpeed.Value);
    }

    private bool HasTargetBeenReached()
    {
        return targetData.sqrDistanceToTarget <= TARGET_DISTANCE_THRESHOLD;
    }

    private void TargetHasBeenReached()
    {
        targetData.hasTarget = false;

        OnTargetReached?.Invoke();
        targetReachedEvent?.Invoke();
    }

    public void OnReplaceReferences(ReferenceReplacer<ScriptableObject, IRuntimeScriptableObject> replacer)
    {
        if (replacer.HasBeenReplaced(this))
        {
            return;
        }

        replacer.SetReference(ref targetSetEvent);
        replacer.SetReference(ref targetReachedEvent);
        replacer.SetReference(ref speedProvider);
        replacer.SetReference(ref velocitySharedCopy);
    }

    // Provides current speed data
    public float OnProvideData()
    {
        return movementStats.speed.Value;
    }
}
