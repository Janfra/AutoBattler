using AutoBattler;
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
        private BattleUnitReferenceType battleUnitType;
        [SerializeField]
        private BattleUnitDataReferenceType selectedUnitType;
        [SerializeField]
        private ArenaDataReferenceType arenaDataType;

        private ExtendedBattleUnitData ownerUnitData;
        private SharedBattleUnitData selectedUnitData;
        private ArenaData arenaData;

        public override bool IsValid()
        {
            return base.IsValid() && selectedUnitType != null && battleUnitType != null && arenaDataType != null;
        }

        public override void OnReplaceReferences(ReferenceReplacer replacer)
        {
            base.OnReplaceReferences(replacer);
            replacer.SetReference(ref battleUnitType);
            replacer.SetReference(ref selectedUnitType);
            replacer.SetReference(ref arenaDataType);
        }

        public override bool IsBlackboardValidForState(BlackboardBase data)
        {
            return base.IsBlackboardValidForState(data) && data.ContainsKey(selectedUnitType) && data.ContainsKey(arenaDataType);
        }

        public override void InitReferences(BlackboardBase data)
        {
            BattleUnit ownerUnit = data.TryGetValue<BattleUnit>(battleUnitType, null);
            if (ownerUnit == null)
            {
                throw new System.NullReferenceException($"Unable to calculate desire for {GetType().Name} desire, owner unit has not been set. - Object name: {data.name}");
            }

            selectedUnitData = data.TryGetValue<SharedBattleUnitData>(selectedUnitType, null);
            if (selectedUnitData == null)
            {
                throw new System.NullReferenceException($"Unable to calculate desire for {GetType().Name} desire, selected unit container has not been set. - Object name: {data.name}");
            }

            arenaData = data.TryGetValue<ArenaData>(arenaDataType, null);
            if (arenaData == null)
            {
                throw new System.NullReferenceException($"Unable to calculate desire for {GetType().Name} desire, arena data has not been set. - Object name: {data.name}");
            }

            ownerUnitData = ownerUnit.GetUnitData();
        }

        protected override void CalculateDesire()
        {
            ExtendedBattleUnitData selectedUnit = selectedUnitData.Value as ExtendedBattleUnitData;
            if (selectedUnit == null || !selectedUnit.IsValid())
            {
                desireValue = 0;
                return;
            }

            BattleTile targetUnitTile = arenaData.GetTileFromNode(selectedUnit.unitPathfindHandle.Value);
            if (targetUnitTile == null)
            {
                throw new System.NullReferenceException($"Unable to calculate desire for {GetType().Name} desire, selected unit tile is null. - Object name: {selectedUnit.transform.name}");
            }

            BattleTile ownerTile = arenaData.GetTileFromNode(ownerUnitData.unitPathfindHandle.Value);
            if (targetUnitTile == null)
            {
                throw new System.NullReferenceException($"Unable to calculate desire for {GetType().Name} desire, owner unit tile is null. - Object name: {ownerUnitData.transform.name}");
            }

            Vector2 distance = targetUnitTile.transform.position - ownerTile.transform.position;
            float maxRangeSqr = rangeFromTarget.Value;
            maxRangeSqr *= maxRangeSqr;

            // for now just yes or no for testing
            bool isOutOfRange = distance.sqrMagnitude > maxRangeSqr;
            float distanceValue = isOutOfRange ? 1 : 0;
            desireValue = bias * distanceValue;
        }
    }
}
