using AutoBattler;
using GameAI;
using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField]
    private SharedBattleUnitData sharedUnitData;
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
    private Animator animator;

    [SerializeField]
    private UniqueRuntimeScriptableObjectInstancesComponent runtimeSOs;

    [SerializeField]
    private DesireStateMachine stateMachine;

    public void Initialise(ExtendedBattleUnitData unitData, PathfindRequester pathfindRequester, ArenaBattleUnitsData enemyUnits, ArenaBattleUnitsData friendlyUnits)
    {
        this.unitData = unitData;
        runtimeSOs.CreateInstanceOfScriptableObject(sharedUnitData).Value = unitData;

        SetMovementComponent(pathfindRequester);
        SetAttackComponent();
        SetHealthComponent();
        SetArenaData(pathfindRequester, enemyUnits, friendlyUnits);
        SetSpriteRenderer();

        if (unitData.unitDefinition.MaxSpeed.Value == 0 && unitData.unitDefinition.Damage.Value == 0)
        {
            stateMachine.enabled = false;
        }
    }

    public ExtendedBattleUnitData GetUnitData()
    {
        return unitData;
    }

    private void SetMovementComponent(PathfindRequester pathfindRequester)
    {
        unitMovement.Initialise(pathfindRequester, unitData.unitPathfindHandle);
        unitMovement.SetMaxSpeed(runtimeSOs.CreateInstanceOfScriptableObject(unitData.unitDefinition.MaxSpeed));
    }

    private void SetAttackComponent()
    {
        unitAttack.SetDamage(runtimeSOs.CreateInstanceOfScriptableObject(unitData.unitDefinition.Damage));
    }

    private void SetHealthComponent()
    {
        Health.ReplaceHealthReference(runtimeSOs.CreateInstanceOfScriptableObject(unitData.unitDefinition.Health));
        Health.SetMaxHealth(runtimeSOs.CreateInstanceOfScriptableObject(unitData.unitDefinition.MaxHealth));
    }

    private void SetArenaData(PathfindRequester pathfindRequester, ArenaBattleUnitsData enemyUnits, ArenaBattleUnitsData friendlyUnits)
    {
        arenaData.Initialise(pathfindRequester, enemyUnits, friendlyUnits);
    }

    private void SetSpriteRenderer()
    {
        animator.runtimeAnimatorController = unitData.unitDefinition.AnimatorController;
    }
}
