using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


namespace GameAI
{
    [Serializable]
    public abstract class BaseStateMachine<StateContainer> : MonoBehaviour
    {
        [SerializeField]
        protected State entryState;
        [SerializeField]
        protected StateContainer[] availableStates;
        [SerializeField]
        protected Blackboard blackboard;

        protected StateContainer currentStateData;
        protected bool isEnabled = false;

        private void Start()
        {
            BakeData();
            if (entryState == null)
            {
                return;
            }

            SetState(entryState);
            isEnabled = true;
        }

        private void Update()
        {
            if (isEnabled && currentStateData != null)
            {
                RunCurrentState();
                AttemptToTransition();
            }
        }

        public abstract bool TryGetStateDataFromState(State targetState, out StateContainer stateData);

        public abstract void SetState(State targetState);

        public abstract void RunCurrentState();

        public abstract void AttemptToTransition();

        public virtual void BakeData() { }
    }
}