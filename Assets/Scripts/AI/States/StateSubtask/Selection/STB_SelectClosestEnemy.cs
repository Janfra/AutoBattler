using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    public class STS_SelectClosestEnemy : STS_TargetSelection<BattleUnitData>
    {
        private List<BattleUnitData> possibleTargets = new List<BattleUnitData>();

        public override void Initialize(TargetSelectionData data)
        {
            base.Initialize(data);
            BattleUnitData[] enemyUnits = selectionData.arenaData.GetEnemyUnitsData();
            selectionData.arenaData.SubscribeToNewEnemyEvent(UpdateTargets);

            if (enemyUnits.Length == 0)
            {
                Debug.LogWarning("There are no enemies to select target");
                return;
            }

            possibleTargets.AddRange(enemyUnits);
        }

        public override bool TryGetSelectedTarget(out BattleUnitData target)  
        {
            target = null;
            if (possibleTargets.Count <= 0)
            {
                return false;
            }

            float minDistance = Mathf.Infinity;
            Vector2 unitPosition = selectionData.ownerTransform.position;

            for (int i = possibleTargets.Count - 1; i >= 0; i--)
            {
                BattleUnitData possibleTarget = possibleTargets[i];
                if (!possibleTarget.IsValid() || !possibleTarget.IsAttackable())
                {
                    possibleTargets.RemoveAt(i);
                    continue;
                }

                Vector2 enemyTargetPosition = possibleTarget.transform.position;
                float sqrDistance = (enemyTargetPosition - unitPosition).sqrMagnitude;
                if (sqrDistance < minDistance)
                {
                    minDistance = sqrDistance;
                    target = possibleTarget;
                }
            }

            return target != null && target.transform != null;
        }

        public override void OnStateExited()
        {
            selectionData.arenaData.UnsubscribeToNewEnemyEvent(UpdateTargets);
            possibleTargets.Clear();
            selectionData = new TargetSelectionData();
        }

        private void UpdateTargets()
        {
            possibleTargets.Add(selectionData.arenaData.GetLatestEnemyAdded());
            Debug.Log("Enemy added to targets");
        }
    }
}
