using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    public class UniqueRuntimeScriptableObjectInstancesComponent : MonoBehaviour
    {
        [DynamicReferenceTypeConstraint(typeof(IUniqueRuntimeScriptableObjectReferenceType))]
        [SerializeField]
        private List<DynamicReference> runtimeReferencers;
        [SerializeField]
        private List<ScriptableObject> runtimeReferences;
        private Dictionary<ScriptableObject, ScriptableObject> referenceReplacements;

        private void Awake()
        {
            ReferenceReplacer<ScriptableObject, IUniqueRuntimeScriptableObject> runtimeSOReplacer = new ReferenceReplacer<ScriptableObject, IUniqueRuntimeScriptableObject>(CreateDictionary());
            foreach (var referencer in runtimeReferencers)
            {
                IUniqueRuntimeScriptableObject referencerObject = referencer.GetReference() as IUniqueRuntimeScriptableObject;
                if (referencerObject != null)
                {
                    referencerObject.OnReplaceReferences(runtimeSOReplacer);
                }
            }
        }

        public T GetCreatedInstanceOfScriptableObject<T>(T original) where T : ScriptableObject
        {
            InitialisedCheck();
            if (!referenceReplacements.ContainsKey(original))
            {
                return null;
            }

            var instance = referenceReplacements[original];
            if (instance is T t)
            {
                return t;
            }

            Debug.LogError($"Component {GetType().Name} in {name} does contain the given scriptable object, however the given type does not match. Input: {typeof(T).Name} - Is: {instance.GetType().Name}");
            return null;
        }

        public T CreateInstanceOfScriptableObject<T>(T original) where T : ScriptableObject
        {
            InitialisedCheck();
            if (referenceReplacements.ContainsKey(original))
            {
                Debug.LogWarning("Scriptable object given has already been instanced in game object");
                return referenceReplacements[original] as T;
            }

            return CreateRuntimeInstance(original) as T;
        }

        protected Dictionary<ScriptableObject, ScriptableObject> CreateDictionary()
        {
            referenceReplacements = new Dictionary<ScriptableObject, ScriptableObject>();
            foreach (var reference in runtimeReferences)
            {
                CreateRuntimeInstance(reference);
            }

            return referenceReplacements;
        }

        private ScriptableObject CreateRuntimeInstance(ScriptableObject originalAsset)
        {
            ScriptableObject uniqueReference = Instantiate(originalAsset);
            uniqueReference.name = originalAsset.name + " as Unique";
            referenceReplacements.Add(originalAsset, uniqueReference);

            return uniqueReference;
        }

        private void InitialisedCheck()
        {
            if (referenceReplacements == null)
            {
                throw new System.MethodAccessException($"Trying to get created instance of scriptable object before {name} has been initialised in {GetType().Name}");
            }
        }
    }
}
