using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField]
    private UnitDefinition unitData;
    [SerializeField]
    private MovementComponent unitMovement;
    [SerializeField]
    private AttackComponent unitAttack;
    [SerializeField]
    private ArenaData arenaData;
}
