using AutoBattler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaData : MonoBehaviour
{
    [SerializeField]
    private PathfindRequester pathfindRequester;
    [SerializeField]
    private ArenaBattleUnitsData enemyUnits;
    [SerializeField]
    private ArenaBattleUnitsData friendlyUnits;

    public BattleUnitData[] GetEnemyUnitsData()
    {
        return enemyUnits.ToArray();
    }

    public BattleUnitData[] GetFriendlyUnitsData()
    {
        return friendlyUnits.ToArray();
    }

    public void SubscribeToNewEnemyEvent(ArenaBattleUnitsData.valueAdded valueAdded)
    {
        enemyUnits.OnValueAdded += valueAdded;
        Debug.Log($"Added listener to enemy units list");
    }

    public void UnsubscribeToNewEnemyEvent(ArenaBattleUnitsData.valueAdded valueAdded)
    {
        enemyUnits.OnValueAdded -= valueAdded;
    }

    public BattleUnitData GetLatestEnemyAdded()
    {
        if (enemyUnits == null || enemyUnits.Count == 0)
        {
            return null;
        }

        return enemyUnits.GetValueAtIndex(enemyUnits.Count - 1);
    }
}
