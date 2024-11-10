using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularData;

[RequireComponent(typeof(Animator))]
public class AnimationComponent : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected List<AnimationDataSetter> animationDataSetters;

    private void Awake()
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
