using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularData;

public class AnimationComponent : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected List<AnimationDataSetter> animationDataSetters;

    private void Awake()
    {
        foreach (AnimationDataSetter setter in animationDataSetters)
        {
            setter.Init(animator);
        }
    }
}
