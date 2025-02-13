using ModularData;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceReplacer<ReplacementType, ObjectReferenceType> where ReplacementType : ScriptableObject
{
    protected Dictionary<ReplacementType, ReplacementType> referenceReplaceData;
    protected List<ObjectReferenceType> replaced = new List<ObjectReferenceType>();

    public ReferenceReplacer(Dictionary<ReplacementType, ReplacementType> referenceReplaceData)
    {
        this.referenceReplaceData = referenceReplaceData;
    }

    public void SetReference<T>(ref T original) where T : ReplacementType
    {
        if (!ContainsReference(original)) return;
        if (referenceReplaceData[original] is T t)
        {
            original = t;
        }
        else
        {
            throw new System.ArgumentException($"{typeof(T).Name} is not the type of the reference set on the original reference type. It is set to: {referenceReplaceData[original].GetType().Name}");
        }
    }

    public bool HasBeenReplaced(ObjectReferenceType replacer)
    {
        if (replaced.Contains(replacer))
        {
            return true;
        }
        else
        {
            replaced.Add(replacer);
            return false;
        }
    }

    public bool ContainsReference(ReplacementType referenceType)
    {
        if (referenceType == null)
        {
            throw new System.ArgumentNullException($"{nameof(ContainsReference)} method does not take in null arguments, part of {GetType().Name} component");
        }

        return referenceReplaceData.ContainsKey(referenceType);
    }
}

public class RuntimeScriptableObjectInstancesComponent : MonoBehaviour
{
    [DynamicReferenceTypeConstraint(typeof(IRuntimeScriptableObjectReferenceType))]
    [SerializeField]
    private List<DynamicReference> runtimeReferencers;
    [SerializeField]
    private List<ScriptableObject> runtimeReferences;
    private Dictionary<ScriptableObject, ScriptableObject> referenceReplacements;

    private void Awake()
    {
        ReferenceReplacer<ScriptableObject, IRuntimeScriptableObject> runtimeSOReplacer = new ReferenceReplacer<ScriptableObject, IRuntimeScriptableObject>(CreateDictionary());
        foreach (var referencer in runtimeReferencers)
        {
            IRuntimeScriptableObject referencerObject = referencer.GetReference() as IRuntimeScriptableObject;
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
