
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponAnimator", menuName = "Gun/WeaponAnimator")]

public class WeaponAnimatorSO : ScriptableObject
{
    public AnimatorOverrideController pistolAnimator;
    public AnimatorOverrideController rifleAnimator;
    public AnimatorOverrideController shotgunAnimator;
}
