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
    protected List<AnimatorDataSetter> animationDataSetters;
    private List<AnimatorDataSetter> onUpdateCheckers = new List<AnimatorDataSetter>();

    public void OnReplaceReferences(ReferenceReplacer<ScriptableObject, IRuntimeScriptableObject> replacer)
    {
        for (int i = 0; i < animationDataSetters.Count; i++)
        {
            var animationInstance = animationDataSetters[i];
            replacer.SetReference(ref animationInstance);
            if (animationInstance is IRuntimeScriptableObject runtimeReplace)
            {
                runtimeReplace.OnReplaceReferences(replacer);
            }

            animationDataSetters[i] = animationInstance;
        }
    }

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        foreach (AnimatorDataSetter setter in animationDataSetters)
        {
            setter.Init(animator);
            if (setter.IsPerFrameCheck)
            {
                onUpdateCheckers.Add(setter);
            }
        }
    }

    private void Update()
    {
        foreach (var dataSetter in onUpdateCheckers)
        {
            dataSetter.OnUpdate();
        }
    }

    private void OnDisable()
    {
        foreach (AnimatorDataSetter setter in animationDataSetters)
        {
            setter.Disabled();
        }
    }

    private void OnEnable()
    {
        foreach (AnimatorDataSetter setter in animationDataSetters)
        {
            setter.Enabled();
        }
    }
}
