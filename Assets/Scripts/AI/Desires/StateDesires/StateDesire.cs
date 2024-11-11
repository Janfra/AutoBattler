using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    [Serializable]
    public abstract class StateDesire : Desire<State>, IBlackboardVerifier
    {
        public bool IsSameTarget(StateDesire other)
        {
            return Target == other.Target;
        }

        public virtual void InitReferences(BlackboardBase data) { }

        public virtual bool IsBlackboardValidForState(BlackboardBase data)
        {
            if (!IsValid())
            {
                Debug.LogWarning("State desire has no target and its verifying for references");
                return true;
            }

            return Target.IsBlackboardValidForState(data);
        }
    }
}