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


    public void Initialise(ExtendedBattleUnitData unitData, PathfindRequester pathfindRequester, ArenaBattleUnitsData enemyUnits, ArenaBattleUnitsData friendlyUnits)
    {
        this.unitData = unitData;
        unitMovement.SetPathfindNode(unitData.unitPathfindHandle);
        arenaData.Initialise(pathfindRequester, enemyUnits, friendlyUnits);
        sprite.sprite = unitData.unitDefinition.Sprite;
    }
}
