using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    [Serializable]
    public struct LinkedBlackboardReferences
    {
        public List<BlackboardReferenceType> AssociatedReferences;
    }

    public class BlackboardDI : BlackboardBase
    {
        [Serializable]
        public struct BlackboardTargetData
        {
            public BoardReferenceData reference;
            public List<BlackboardDETargetReferenceType> linkedTarget;
        }

        [SerializeField]
        protected List<BlackboardTargetData> targets;

        public override BoardReferenceData[] GetDataContainersCopy()
        {
            throw new NotImplementedException();
        }

        public override bool SetReferenceAt(int index, UnityEngine.Object reference)
        {
            throw new NotImplementedException();
        }
    }

    public interface IBlackboardDITarget
    {
        public void SetBlackboardReferences(LinkedBlackboardReferences references);
    }

}
