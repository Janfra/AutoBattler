using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRuntimeScriptableObject
{
    public void OnReplaceReferences(ReferenceReplacer<ScriptableObject, IRuntimeScriptableObject> replacer);
}
