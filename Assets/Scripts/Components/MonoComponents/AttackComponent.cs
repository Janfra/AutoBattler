using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackData
{
    public int damage;
}

public class AttackComponent : MonoBehaviour
{
    [SerializeField]
    private IntReference damage;
    [SerializeField]
    private IntReference attackSpeed;

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

public interface IAttackable
{
    // Potentially change it to pass it data to decide instead, but for now keep it simple
    public bool IsAttackable { get; }  
    public void ReceiveAttack(AttackData attackData);
}
