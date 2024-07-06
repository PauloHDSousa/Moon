using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] Rig rig;

    [Header("Guns")]
    [SerializeField] Transform[] guns;
    [SerializeField] Transform pistol;
    [SerializeField] Transform revolver;
    [SerializeField] Transform autoRifle;
    [SerializeField] Transform shotgun;
    [SerializeField] Transform sniperRifle;


    [Header("Left Hand IK")]
    [SerializeField] TwoBoneIKConstraint leftHandIK;
    [SerializeField] Transform leftHandIKTarget;
    [SerializeField] float rigIncreaseStep;
    [SerializeField] float leftHandIKIncreaseStep;

    bool rigShouldBeIncreased;
    bool leftHandIKShouldBeIncreased;
    bool isAnimating;

    private void Update()
    {
        if (!isAnimating)
        {
            CheckWeaponSwitch();
        }

        if (rigShouldBeIncreased)
        {
            rig.weight += rigIncreaseStep * Time.deltaTime;
            if (rig.weight >= 1)
                rigShouldBeIncreased = false;
        }

        if (leftHandIKShouldBeIncreased)
        {
            leftHandIK.weight += leftHandIKIncreaseStep * Time.deltaTime;
            if (leftHandIK.weight >= 1)
                leftHandIKShouldBeIncreased = false;
        }
    }

    public void PlayReloadAnimation()
    {
        animator.SetTrigger("Reload");
        PauseRig();
    }

    private void PauseRig()
    {
        rig.weight = 0.15f;
    }

    public void ReturnWeightToOne()
    {
        rigShouldBeIncreased = true;
        leftHandIKShouldBeIncreased = true;
        isAnimating = false; 
        animator.SetBool("GrabingWeapon", false);
    }

    private void PlayWeaponGrabAnimation(WeaponGrabType grabType)
    {

        leftHandIK.weight = 0;
        PauseRig();
        animator.SetFloat("WeaponGrabType", ((float)grabType));
        animator.SetTrigger("Grab");
        animator.SetBool("GrabingWeapon", true);
    }
    private void CheckWeaponSwitch()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            SwitchOnGun(pistol);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            SwitchOnGun(revolver);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            SwitchOnGun(autoRifle, 1, WeaponGrabType.BackGrab);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            SwitchOnGun(shotgun, 2, WeaponGrabType.BackGrab);
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            SwitchOnGun(sniperRifle, 3, WeaponGrabType.BackGrab);
        }
    }

    private void SwitchOnGun(Transform gun, int layerIndex = 1, WeaponGrabType weaponGrabType = 0)
    {
        isAnimating = true;
        SwitchOffGuns();
        gun.gameObject.SetActive(true);
        AttachLeftHand(gun);
        SwitchAnimatorLayer(layerIndex);
        PlayWeaponGrabAnimation(weaponGrabType);
    }

    private void SwitchOffGuns()
    {
        foreach (Transform gun in guns)
        {
            gun.gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand(Transform gun)
    {
        Transform target = gun.GetComponentInChildren<LeftHandTargetTransform>().transform;
        leftHandIKTarget.localPosition = target.transform.localPosition;
        leftHandIKTarget.localRotation = target.transform.localRotation;
    }

    void SwitchAnimatorLayer(int layerIndex)
    {
        for (int i = 1; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(layerIndex, 1);
    }
}

public enum WeaponGrabType
{
    SideGrag, BackGrab
}