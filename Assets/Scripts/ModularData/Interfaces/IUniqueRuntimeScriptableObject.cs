using UnityEngine;

namespace ModularData
{
    public interface IUniqueRuntimeScriptableObject
    {
        public void OnReplaceReferences(ReferenceReplacer<ScriptableObject, IUniqueRuntimeScriptableObject> replacer);
    }
}
