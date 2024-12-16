using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRuntimeScriptableObject
{
    public T GetRuntimeDuplicate<T>() where T : ScriptableObject;
}
