using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleUnit : MonoBehaviour
{
    [SerializeField]
    private UniqueRuntimeScriptableObjectInstancesComponent dataHolder;
    [SerializeField]
    private SharedBattleUnitData unitDataAccessor;
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private Slider delayedHealthBar;

    private SharedInt health;
    private SharedInt maxHealth;
    private IEnumerator healthAnimation;

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
        delayedHealthBar.maxValue = maxHealth.Value;
        delayedHealthBar.value = health.Value;
    }

    public void UpdateHealth()
    {
        if (healthAnimation != null)
        {
            delayedHealthBar.value = healthBar.value;
            StopCoroutine(healthAnimation);
        }

        healthAnimation = AnimateHealthBar(health.Value);
        StartCoroutine(healthAnimation);
        healthBar.value = health.Value;
    }

    public IEnumerator AnimateHealthBar(int healthValue)
    {
        float progress = 0.0f;
        float startValue = healthBar.value;

        while (progress < 1.0f)
        {
            progress = Mathf.Min(1.0f, progress + Time.deltaTime);
            delayedHealthBar.value = Mathf.Lerp(startValue, healthValue, progress);
            yield return null;
        }

        healthAnimation = null;
        yield return null;
    } 
}
