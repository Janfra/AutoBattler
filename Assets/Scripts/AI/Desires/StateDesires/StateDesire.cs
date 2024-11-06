using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class StateDesire : Desire<State>, IBlackboardVerifier
{
    public bool IsSameTarget(StateDesire other)
    {
        return desireTarget == other.Target;
    }

    public virtual void InitReferences(Blackboard data) { }

    public virtual bool IsBlackboardValidForState(Blackboard data) 
    { 
        if (!IsValid())
        {
            Debug.LogWarning("State desire has no target and its verifying for references");
            return true;
        }

        return Target.IsBlackboardValidForState(data); 
    }
}
