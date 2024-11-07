using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    public interface IBlackboardVerifier
    {
        public bool IsBlackboardValidForState(BlackboardBase data);
    }
}