using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    [CreateAssetMenu(fileName = "New Attack Target State", menuName = "ScriptableObjects/State/Attack Target")]
    public class ST_AttackTarget : State
    {
        private BasicTimer timer = new BasicTimer();
        [SerializeField]
        private AttackComponentReferenceType attackType;
        [SerializeField]
        private BattleUnitDataReferenceType selectedUnitType;
        private SharedBattleUnitData selectedUnitData;
        private AttackComponent attackComponent;

        public override bool IsBlackboardValidForState(BlackboardBase data)
        {
            return data.ContainsKey(selectedUnitType) && data.ContainsKey(attackType);
        }

        public override void StateEntered()
        {
            selectedUnitData = blackboard.TryGetValue<SharedBattleUnitData>(selectedUnitType, null);
            if (selectedUnitData == null)
            {
                throw new System.NullReferenceException($"Unable to target enemy to attack in {GetType().Name} state, shared data container has not been set. - Object Name: {blackboard.name}");
            }

            attackComponent = blackboard.TryGetValue<AttackComponent>(attackType, null);
            if (selectedUnitData == null)
            {
                throw new System.NullReferenceException($"Unable to attack enemy in {GetType().Name} state, attack component is null. - Object Name: {blackboard.name}");
            }

            selectedUnitData.Value.attackable.Destroyed += RemoveTarget;

            timer.ResetProgress();
            timer.TargetDuration = attackComponent.GetAttackSpeed();
            timer.SubscribeToCallback(AttackTarget);
        }

        public override void RunState()
        {
            timer.IsTimerTickOnTarget(false);
        }

        public override void StateExited()
        {
            timer.UnsubscribeToCallback(AttackTarget);
            selectedUnitData.Value.attackable.Destroyed -= RemoveTarget;
            selectedUnitData = null;
            attackComponent = null;
        }

        private void AttackTarget()
        {
            BattleUnitData selectedUnit = selectedUnitData.Value;
            if (!selectedUnit.IsValid() || !selectedUnit.IsAttackable())
            {
                return;
            }

            Debug.Log($"Attacked {selectedUnit.attackable} with an attack that deals {attackComponent.GetDamage()} damage");
            attackComponent.TryAttack(selectedUnit.attackable);
        }

        private void RemoveTarget()
        {
            selectedUnitData.Value = new BattleUnitData();
        }
    }
}