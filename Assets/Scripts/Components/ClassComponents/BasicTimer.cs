using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BasicTimer
{
    public delegate void TimerCallback();
    protected event TimerCallback timerCallback;

    public float TargetDuration
    {
        get { return targetDuration; }
        set { targetDuration = Mathf.Clamp(value, 0.0f, Mathf.Infinity); }
    }

    public float ProgressValue
    {
        get { return Mathf.Clamp01(currentTime / targetDuration); }
    }

    [SerializeField]
    private float targetDuration;
    protected float currentTime = 0.0f;

    // Ticks the state timer until it reaches the target duration.
    // Returns: Has the timer duration been reached
    public virtual bool IsTimerTickOnTarget()
    {
        currentTime += Time.deltaTime;
        bool targetReached = currentTime >= targetDuration;
        if (targetReached)
        {
            currentTime = 0.0f;
            ConsumeTimerCallback();
        }
        return targetReached;
    }

    protected void ConsumeTimerCallback()
    {
        if (timerCallback == null)
        {
            return;
        }

        timerCallback.Invoke();
        timerCallback = null;
    }

    public void SubscribeToCallback(TimerCallback callback)
    {
        timerCallback += callback;
    }

    public void UnsubscribeToCallback(TimerCallback callback) 
    {
        timerCallback -= callback; 
    }
}
