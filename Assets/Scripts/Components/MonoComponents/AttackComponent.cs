using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    [SerializeField]
    private IntReference damage;
    [SerializeField]
    private IntReference attackSpeed;

    public int GetAttackSpeed()
    {
        return attackSpeed.Value;
    }

    public int GetDamage()
    {
        return damage.Value;
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
        return true;
    }
}
