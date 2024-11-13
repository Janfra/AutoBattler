using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Definition", menuName = "ScriptableObjects/Unit Definition")]
public class UnitDefinition : ScriptableObject
{
    [SerializeField]
    private string unitName;
    public string UnitName { get => unitName; }
}
