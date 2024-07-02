using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOverride : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void SetAnimations(AnimatorOverrideController animOverrideController)
    {
        animator.runtimeAnimatorController = animOverrideController;
    }
}
