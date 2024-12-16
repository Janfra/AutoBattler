using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    [CreateAssetMenu(fileName = "New Select State Desire", menuName = "ScriptableObjects/Desires/States/Select Target")]
    public class STD_SelectTarget : StateDesire
    {
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
            // If valid is already valid, no need to select a new target
            BattleUnitData selectedUnit = selectedUnitData.Value;
            if (selectedUnit == null)
            {
                desireValue = bias * 1;
                return;
            }

            int needTargetValue = selectedUnit.IsValid() && selectedUnit.IsAttackable() ? 0 : 1;
            desireValue = bias * needTargetValue;
        }
    }
}
