using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "ScriptableObjects/Unit Data")]
public class UnitData : ScriptableObject
{
    [SerializeField]
    private string unitName;
    public string UnitName { get => unitName; }
}
