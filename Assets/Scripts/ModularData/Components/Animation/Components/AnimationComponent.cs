using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularData;

[RequireComponent(typeof(Animator))]
public class AnimationComponent : MonoBehaviour, IRuntimeScriptableObject
{
    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected List<AnimationDataSetter> animationDataSetters;

    public void OnReplaceReferences(ReferenceReplacer<ScriptableObject, IRuntimeScriptableObject> replacer)
    {
        for (int i = 0; i < animationDataSetters.Count; i++)
        {
            var animationInstance = animationDataSetters[i];
            replacer.SetReference(ref animationInstance);
            animationDataSetters[i] = animationInstance;
        }
    }

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        foreach (AnimationDataSetter setter in animationDataSetters)
        {
            setter.Init(animator);
        }
    }
}
