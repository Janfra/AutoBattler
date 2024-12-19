using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour, IRuntimeScriptableObject
{
    [SerializeField]
    private GameEvent onAttacked;
    [SerializeField]
    private IntReference damage;
    [SerializeField]
    private FloatReference attackSpeed;

    public float GetAttackSpeed()
    {
        return attackSpeed.Value;
    }

    public int GetDamage()
    {
        return damage.Value;
    }

    public void SetDamage(int newDamage)
    {
        damage.Value = newDamage;
    }

    public void SetDamage(SharedValue<int> newDamage)
    {
        damage.SharedValueReference = newDamage;
    }

    public bool TryAttack(IAttackable target)
    {
        if (target == null)
        {
            return false;
        }
        
        if (!target.IsAttackable)
        {
            return false;
        }

        AttackData attackData = new AttackData();
        attackData.damage = damage.Value;
        target.ReceiveAttack(attackData);
        onAttacked?.Invoke();
        return true;
    }

    public void OnReplaceReferences(ReferenceReplacer<ScriptableObject, IRuntimeScriptableObject> replacer)
    {
        if (replacer.HasBeenReplaced(this))
        {
            return;
        }

        replacer.SetReference(ref onAttacked);
    }
}
