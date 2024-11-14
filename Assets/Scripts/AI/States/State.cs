using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAI
{
    public abstract class State : ScriptableObject, IBlackboardVerifier, IUniqueBlackboardReferencer
    {
        protected BlackboardBase blackboard;

        // Check that blackboard has the required references
        public abstract bool IsBlackboardValidForState(BlackboardBase data);

        public virtual void Init(BlackboardBase data)
        {
            blackboard = data;
        }

        // Update the state
        public abstract void RunState();

        public virtual void StateEntered() { }

        public virtual void StateExited() { }

        public abstract void OnReplaceReferences(ReferenceReplacer replacer);
    }

    public abstract class TimedState : State
    {
        [SerializeField]
        protected BasicTimer timer;
    }
}