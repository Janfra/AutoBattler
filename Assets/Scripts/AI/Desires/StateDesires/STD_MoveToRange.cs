using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    [CreateAssetMenu(fileName = "New Move To Range Desire", menuName = "ScriptableObjects/Desires/States/Move To Range")]
    public class STD_MoveToRange : StateDesire
    {
        [SerializeField]
        private FloatReference rangeFromTarget;
        [SerializeField]
        private BattleUnitDataReferenceType selectedUnitType;
        private SharedBattleUnitData selectedUnitData;
        private Transform ownerTransform;

        public override bool IsValid()
        {
            return base.IsValid() && selectedUnitType != null;
        }

        public override void OnReplaceReferences(ReferenceReplacer replacer)
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

            ownerTransform = data.transform;
        }

        protected override void CalculateDesire()
        {
            BattleUnitData selectedUnit = selectedUnitData.Value;
            if (!selectedUnit.IsValid())
            {
                desireValue = 0;
                return;
            }

            Vector2 distance = selectedUnit.transform.position - ownerTransform.position;
            float maxRangeSqr = rangeFromTarget.Value;
            maxRangeSqr *= maxRangeSqr;

            // for now just yes or no for testing
            bool isOutOfRange = distance.sqrMagnitude > maxRangeSqr;
            float distanceValue = isOutOfRange ? 1 : 0;
            desireValue = bias * distanceValue;
        }
    }
}
