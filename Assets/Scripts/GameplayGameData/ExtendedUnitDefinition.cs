using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Definition", menuName = "ScriptableObjects/Unit Definition")]
public class ExtendedUnitDefinition : UnitDefinition
{
    [SerializeField]
    private BattleUnit unitPrefab;
    public BattleUnit UnitPrefab { get => unitPrefab; }
}
