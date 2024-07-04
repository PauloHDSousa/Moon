using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerWeaponVisuals weaponVisualController;

    public void ReloadIsOver()
    {
        weaponVisualController.ReturnWeightToOne();
    }

    public void AnimationIsOver()
    {
        weaponVisualController.ReturnWeightToOne();
    }
}
