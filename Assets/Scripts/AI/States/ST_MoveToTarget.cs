using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    [CreateAssetMenu(fileName = "New Move To Target State", menuName = "ScriptableObjects/State/Move To Target")]
    public class ST_MoveToTarget : State
    {
        [SerializeField]
        private PathfindMovementReferenceType movementType;
        [SerializeField]
        private BattleUnitDataReferenceType selectedUnitType;
        private SharedBattleUnitData selectedUnitData;
        private PathfindMovementComponent movementComponent;

        public override bool IsBlackboardValidForState(BlackboardBase data)
        {
            return data.ContainsKey(movementType) && data.ContainsKey(selectedUnitType);
        }

        public override void OnReplaceReferences(BlackboardReferenceReplacer replacer)
        {
            if (replacer.HasBeenReplaced(this))
            {
                return;
            }

            replacer.SetReference(ref movementType);
            replacer.SetReference(ref selectedUnitType);    
        }

        public override void StateEntered()
        {
            selectedUnitData = blackboard.TryGetValue<SharedBattleUnitData>(selectedUnitType, null);
            if (selectedUnitData == null)
            {
                throw new System.NullReferenceException($"Unable to get selected target in {GetType().Name} state, shared data container has not been set. - Object Name: {blackboard.name}");
            }

            movementComponent = blackboard.TryGetValue<PathfindMovementComponent>(movementType, null);
            if (movementComponent == null)
            {
                throw new System.NullReferenceException($"Unable to move to target in {GetType().Name} state, movement component is null. - Object Name: {blackboard.name}");
            }

            movementComponent.OnTargetReached += CanUpdateTarget;
            SetMovementTarget();
        }

        public override void RunState()
        {
            // Currently driven by events instead of frame update
        }

        public override void StateExited()
        {
            movementComponent.OnTargetReached -= CanUpdateTarget;
            movementComponent.ClearMovementTarget();
            selectedUnitData = null;
            movementComponent = null;
        }

        private void CanUpdateTarget()
        {
            SetMovementTarget();
        }

        private void SetMovementTarget()
        {
            ExtendedBattleUnitData selectedUnit = selectedUnitData.Value as ExtendedBattleUnitData;
            if (!selectedUnit.IsValid())
            {
                return;
            }

            movementComponent.SetPathfindTarget(selectedUnit.unitPathfindHandle.Value);
        }
    }
}