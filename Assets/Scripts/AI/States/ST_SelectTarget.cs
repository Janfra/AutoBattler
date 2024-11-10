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
        private BattleUnitReferenceType battleUnitType;
        [SerializeField]
        private MovementReferenceType movementType;
        private List<BattleUnitData> possibleTargets = new List<BattleUnitData>();

        public override bool IsBlackboardValidForState(BlackboardBase data)
        {
            return data.ContainsKey(movementType) && data.ContainsKey(battleUnitType);
        }

        public override void StateEntered()
        {
            possibleTargets.Clear();
            BattleUnit unit = blackboard.TryGetValue<BattleUnit>(battleUnitType, null);
            BattleUnitData[] enemyUnits = unit.GetEnemyUnitsData();
            unit.SubscribeToNewEnemyEvent(UpdateTargets);

            if (enemyUnits.Length == 0)
            {
                Debug.Log("There are no enemies to select target");
                    return;
            }

            possibleTargets.AddRange(enemyUnits);
        }

        public override void StateExited()
        {
            BattleUnit unit = blackboard.TryGetValue<BattleUnit>(battleUnitType, null);
            unit.UnsubscribeToNewEnemyEvent(UpdateTargets);
        }

        public override void RunState()
        {
            if (possibleTargets.Count <= 0)
            {
                return;
            }

            Movement unitMovement = blackboard.TryGetValue<Movement>(movementType, null);
            if (unitMovement == null)
            {
                return;
            }

            Vector2 unitPosition = unitMovement.transform.position;
            float minDistance = Mathf.Infinity;
            Vector2 targetPosition = unitPosition;

            for (int i = possibleTargets.Count - 1; i >= 0; i--)
            {
                if (possibleTargets[i].transform == null)
                {
                    possibleTargets.RemoveAt(i);
                    continue;
                }

                Vector2 enemyTargetPosition = possibleTargets[i].transform.position;
                float sqrDistance = (enemyTargetPosition - unitPosition).sqrMagnitude;
                if (sqrDistance < minDistance)
                {
                    minDistance = sqrDistance;
                    targetPosition = enemyTargetPosition;
                }
            }

            unitMovement.SetMovementTarget(targetPosition);
        }

        private void UpdateTargets()
        {
            BattleUnit unit = blackboard.TryGetValue<BattleUnit>(battleUnitType, null);
            possibleTargets.Add(unit.GetLatestEnemyAdded());
            Debug.Log("Enemy added to targets");
        }
    }
}
