using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IAttackable, IRuntimeScriptableObject, IDataProvider<int>
{
    public event IAttackable.OnDestroy Destroyed;

    [SerializeField]
    private GameEvent damageTakenEvent;
    [SerializeField]
    private GameEvent healthDepleted;

    [SerializeField]
    private SharedIntDataProvider healthProvider;

    [SerializeField]
    private IntReference maxHealth;
    [SerializeField]
    private IntReference health;

    private void Awake()
    {
        healthProvider.Value = this;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth.Value = newMaxHealth;
        health.Value = newMaxHealth;
    }

    public void SetMaxHealth(SharedValue<int> newMaxHealth, bool resetHealth = true)
    {
        maxHealth.SharedValueReference = newMaxHealth;
        if (resetHealth)
        {
            ResetHealth();
        }
        else
        {
            health.Value = Mathf.Min(newMaxHealth.Value, health.Value);
        }
    }

    public void ReplaceHealthReference(SharedValue<int> newHealth)
    {
        if (health.IsValid())
        {
            newHealth.Value = health.Value;
        }

        health.SharedValueReference = newHealth;
    }

    public void ResetHealth()
    {
        health.Value = maxHealth.Value;
    }

    public bool IsAttackable => health > 0;
    public void ReceiveAttack(AttackData attackData)
    {
        health.Value -= attackData.damage;

        if (health.Value <= 0)
        {
            Destroyed?.Invoke();
            Destroyed = null;
            healthDepleted?.Invoke();
        }
        else
        {
            damageTakenEvent?.Invoke();
        }
    }

    public void OnHealthDepleyed()
    {
        gameObject.SetActive(false);
    }

    public void OnReplaceReferences(ReferenceReplacer<ScriptableObject, IRuntimeScriptableObject> replacer)
    {
        if (replacer.HasBeenReplaced(this))
        {
            return;
        }

        replacer.SetReference(ref damageTakenEvent);
        replacer.SetReference(ref healthDepleted);
        replacer.SetReference(ref healthProvider);
    }

    // Health's provide data interface will just be used to return the current health
    public int OnProvideData()
    {
        return health.Value;
    }
}
