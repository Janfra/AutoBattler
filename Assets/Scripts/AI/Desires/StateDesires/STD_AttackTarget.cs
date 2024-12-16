using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    [CreateAssetMenu(fileName = "New Attack Target Desire", menuName = "ScriptableObjects/Desires/States/Attack Target")]
    public class STD_AttackTarget : StateDesire
    {
        [SerializeField]
        private FloatReference attackRange;
        [SerializeField]
        private BattleUnitDataReferenceType selectedUnitType;
        private SharedBattleUnitData selectedUnitData;

        public override bool IsValid()
        {
            return base.IsValid() && selectedUnitType != null;
        }

        public override void OnReplaceReferences(BlackboardReferenceReplacer replacer)
        {
            base.OnReplaceReferences(replacer);
            replacer.SetReference(ref selectedUnitType);
        }

        public override bool IsBlackboardValidForState(BlackboardBase data)
        {
            return base.IsBlackboardValidForState(data) && data.ContainsKey(selectedUnitType);
        }

        public override void InitReferences(BlackboardBase data)
        {
            selectedUnitData = data.TryGetValue<SharedBattleUnitData>(selectedUnitType, null);
            if (selectedUnitData == null)
            {
                throw new System.NullReferenceException($"Unable to calculate desire for {GetType().Name} desire, selected unit container has not been set. - Object name: {data.name}");
            }
        }

        protected override void CalculateDesire()
        {
            BattleUnitData selectedUnit = selectedUnitData.Value;
            if (selectedUnit == null || !selectedUnit.IsValid())
            {
                desireValue = 0;
            }

            // just attack if attackable for prototype
            float attackValue = 1;
            desireValue = bias * attackValue;
        }
    }
}