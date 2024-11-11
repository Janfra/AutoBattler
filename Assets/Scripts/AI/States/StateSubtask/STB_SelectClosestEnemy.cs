using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GameAI
{
    public class STS_SelectClosestEnemy : STS_TargetSelection<BattleUnitData>
    {
        private List<BattleUnitData> possibleTargets = new List<BattleUnitData>();

        public override void Initialize(TargetSelectionData data)
        {
            base.Initialize(data);

            possibleTargets.Clear();
            BattleUnitData[] enemyUnits = selectionData.arenaData.GetEnemyUnitsData();
            selectionData.arenaData.SubscribeToNewEnemyEvent(UpdateTargets);

            if (enemyUnits.Length == 0)
            {
                Debug.Log("There are no enemies to select target");
                return;
            }

            possibleTargets.AddRange(enemyUnits);
        }

        public override bool TryGetSelectedTarget(out BattleUnitData target)  
        {
            target = new BattleUnitData();
            if (possibleTargets.Count <= 0)
            {
                return false;
            }

            float minDistance = Mathf.Infinity;
            Vector2 unitPosition = selectionData.ownerTransform.position;

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
                    target = possibleTargets[i];
                }
            }

            return target.transform != null;
        }


        private void UpdateTargets()
        {
            possibleTargets.Add(selectionData.arenaData.GetLatestEnemyAdded());
            Debug.Log("Enemy added to targets");
        }
    }
}
