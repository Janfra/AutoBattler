using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlackboardVerifier
{
    public bool IsBlackboardValidForState(Blackboard data);
}
