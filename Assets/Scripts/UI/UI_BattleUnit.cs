using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleUnit : MonoBehaviour
{
    [SerializeField]
    private RuntimeScriptableObjectInstancesComponent dataHolder;
    [SerializeField]
    private SharedBattleUnitData unitDataAccessor;
    [SerializeField]
    private Slider healthBar;

    private SharedInt health;
    private SharedInt maxHealth;

    private void Awake()
    {
        if (healthBar == null)
        {
            return;
        }

        if (unitDataAccessor == null)
        {
            return;
        }

        if (dataHolder == null)
        {
            return;
        }
    }

    private void Start()
    {
        SharedBattleUnitData unitData = dataHolder.GetCreatedInstanceOfScriptableObject(unitDataAccessor);
        health = dataHolder.GetCreatedInstanceOfScriptableObject(unitData.Value.unitDefinition.Health);
        maxHealth = dataHolder.GetCreatedInstanceOfScriptableObject(unitData.Value.unitDefinition.MaxHealth);

        healthBar.maxValue = maxHealth.Value;
        healthBar.value = health.Value;
    }

    public void UpdateHealth()
    {
        healthBar.value = health.Value;
    }
}
