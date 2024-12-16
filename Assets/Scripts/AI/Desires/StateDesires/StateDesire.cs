using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    [Serializable]
    public abstract class StateDesire : Desire<State>, IBlackboardVerifier, IUniqueBlackboardReferencer
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

        public virtual void CreateInstances() 
        {
            desireTarget = ScriptableObject.Instantiate(Target);
        }

        public virtual void OnReplaceReferences(BlackboardReferenceReplacer replacer) 
        {
            if (replacer.HasBeenReplaced(this))
            {
                return;
            }

            Target.OnReplaceReferences(replacer);
        }
    }
}