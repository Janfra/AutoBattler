using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    [CreateAssetMenu(fileName = "New Select Target State", menuName = "ScriptableObjects/State/Select Target")]
    public class ST_SelectTarget : State
    {
        [SerializeField]
        private ArenaDataReferenceType arenaDataType;
        [SerializeField]
        private MovementReferenceType movementType;
        private STS_SelectClosestEnemy selectionTask = new STS_SelectClosestEnemy();

        public override bool IsBlackboardValidForState(BlackboardBase data)
        {
            return data.ContainsKey(movementType) && data.ContainsKey(arenaDataType);
        }

        public override void StateEntered()
        {
            ArenaData arenaData = blackboard.TryGetValue<ArenaData>(arenaDataType, null);
            if (arenaData == null)
            {
                return;
            }

            TargetSelectionData selectionData = new TargetSelectionData(arenaData, arenaData.transform);
            selectionTask.Initialize(selectionData);
        }

        public override void StateExited()
        {
            selectionTask.OnStateExited();
        }

        public override void RunState()
        {
            MovementComponent unitMovement = blackboard.TryGetValue<MovementComponent>(movementType, null);
            if (unitMovement == null)
            {
                return;
            }

            BattleUnitData targetData;
            if (!selectionTask.TryGetSelectedTarget(out targetData))
            {
                return;
            }

            unitMovement.SetMovementTarget(targetData.transform.position);
        }
    }
}
