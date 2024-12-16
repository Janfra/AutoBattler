using AutoBattler;
using GameAI;
using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField]
    private ExtendedBattleUnitData unitData;
    [SerializeField]
    private PathfindMovementComponent unitMovement;
    [SerializeField]
    private AttackComponent unitAttack;

    [SerializeField]
    private HealthComponent unitHealth;
    public HealthComponent Health => unitHealth;

    [SerializeField]
    private ArenaData arenaData;
    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private DesireStateMachine stateMachine;

    public void Initialise(ExtendedBattleUnitData unitData, PathfindRequester pathfindRequester, ArenaBattleUnitsData enemyUnits, ArenaBattleUnitsData friendlyUnits)
    {
        this.unitData = unitData;
        unitMovement.Initialise(pathfindRequester, unitData.unitPathfindHandle);
        unitMovement.SetMaxSpeed(unitData.unitDefinition.MaxSpeed);
        unitAttack.SetDamage(unitData.unitDefinition.Damage);
        arenaData.Initialise(pathfindRequester, enemyUnits, friendlyUnits);
        sprite.sprite = unitData.unitDefinition.Sprite;

        if (unitData.unitDefinition.MaxSpeed.Value == 0 && unitData.unitDefinition.Damage.Value == 0)
        {
            stateMachine.enabled = false;
        }
    }

    public ExtendedBattleUnitData GetUnitData()
    {
        return unitData;
    }
}
