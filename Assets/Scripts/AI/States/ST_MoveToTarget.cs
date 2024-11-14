using ModularData;
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

        public override void OnReplaceReferences(ReferenceReplacer replacer)
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
        }

        public override void RunState()
        {
            BattleUnitData selectedUnit = selectedUnitData.Value;
            if (!selectedUnit.IsValid())
            {
                return;
            }

            movementComponent.SetMovementTarget(selectedUnit.transform.position);
        }

        public override void StateExited()
        {
            selectedUnitData = null;
            movementComponent = null;
        }
    }
}