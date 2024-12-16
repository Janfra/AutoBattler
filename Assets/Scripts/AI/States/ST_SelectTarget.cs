using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    [CreateAssetMenu(fileName = "New Select Target State", menuName = "ScriptableObjects/State/Select Target")]
    public class ST_SelectTarget : State
    {
        [SerializeField]
        private ArenaDataReferenceType arenaDataType;
        [SerializeField]
        private BattleUnitDataReferenceType selectedUnitType;
        private STS_SelectClosestEnemy selectionTask = new STS_SelectClosestEnemy();

        public override bool IsBlackboardValidForState(BlackboardBase data)
        {
            return data.ContainsKey(selectedUnitType) && data.ContainsKey(arenaDataType);
        }

        public override void OnReplaceReferences(BlackboardReferenceReplacer replacer)
        {
            if (replacer.HasBeenReplaced(this))
            {
                return;
            }

            replacer.SetReference(ref arenaDataType);
            replacer.SetReference(ref selectedUnitType);
        }

        public override void StateEntered()
        {
            ArenaData arenaData = blackboard.TryGetValue<ArenaData>(arenaDataType, null);
            if (arenaData == null)
            {
                return;
            }

            TargetSelectionData selectionData = new TargetSelectionData(arenaData, arenaData.transform);
            selectionTask.Initialize(selectionData);
        }

        public override void RunState()
        {
            SharedBattleUnitData sharedSelectionData = blackboard.TryGetValue<SharedBattleUnitData>(selectedUnitType, null);
            if (sharedSelectionData == null)
            {
                throw new NullReferenceException($"Unable to select a target in {GetType().Name} state, shared data container has not been set. - Object Name: {blackboard.name}");
            }

            BattleUnitData targetData;
            if (!selectionTask.TryGetSelectedTarget(out targetData))
            {
                return;
            }

            sharedSelectionData.Value = targetData;
        }

        public override void StateExited()
        {
            selectionTask.OnStateExited();
        }
    }
}
