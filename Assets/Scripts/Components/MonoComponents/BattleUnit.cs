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
    [SerializeField]
    private ArenaData arenaData;
}
