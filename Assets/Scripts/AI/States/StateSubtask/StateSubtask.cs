using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    public abstract class StateSubtask
    {
        // Cannot use StateEntered or a shared initialiser
        // since the requirement values change and I want to avoid having to set again the reference types
        public virtual void OnStateExited() { }
    }

    public struct TargetSelectionData
    {
        public ArenaData arenaData;
        public Transform ownerTransform;

        public TargetSelectionData(ArenaData arenaData, Transform ownerTransform)
        {
            this.arenaData = arenaData;
            this.ownerTransform = ownerTransform;
        }
    }

    /// <summary>
    /// Base class for any target selection logic
    /// </summary>
    /// <typeparam name="TargetType">Type of target to return in selection</typeparam>
    public abstract class STS_TargetSelection<TargetType> : StateSubtask
    {
        protected TargetSelectionData selectionData;

        public virtual void Initialize(TargetSelectionData data) { UpdateData(data); }
        public virtual void UpdateData(TargetSelectionData data) { selectionData = data; }
        public abstract bool TryGetSelectedTarget(out TargetType target);
    }
}
