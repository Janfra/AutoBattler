using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wanted to tie it to state machines but I can constraint it to a template
public abstract class State : ScriptableObject, IBlackboardVerifier
{
    protected Blackboard blackboard;

    // Check that blackboard has the required references
    public abstract bool IsBlackboardValidForState(Blackboard data);

    public virtual void Init(Blackboard data)
    {
        blackboard = data;
    }

    // Update the state
    public abstract void RunState();

    public virtual void StateEntered() { }

    public virtual void StateExited() { }
}

public abstract class TimedState : State
{
    [SerializeField]
    protected BasicTimer timer;
}