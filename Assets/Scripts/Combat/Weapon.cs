using RPG.Attributes;
using System;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName ="Wepons",menuName ="Weapons/Create New Weapon")]
    public class Weapon : ScriptableObject
    {

        [Header("Refrences")]
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] AnimatorOverrideController weaponOverride = null;

        [Header("Weapon Attributes")]
        [SerializeField] float damage = 30f;
        
        [Tooltip("The percent buff this weapon has which will be added to the base damage of the weapon.")]
        [SerializeField] float percentBonus = 0f;

        [SerializeField] float WeaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        [Header("Weapon Name")]
        const string weaponName = "Weapon";

        public void Spawn(Transform LeftHand, Transform RightHand, Animator animator)
        {

            DestroyOldWeapon(RightHand, LeftHand);
            if (weaponPrefab != null)
            {
                Transform handTransform = GetTransform(LeftHand, RightHand);
                GameObject weapon = Instantiate(weaponPrefab, handTransform);
                weapon.name = weaponName;
            }

            var overrideContoller = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (weaponOverride != null)
            {
                animator.runtimeAnimatorController = weaponOverride;
            }
            else if(overrideContoller != null)
            {
                animator.runtimeAnimatorController = overrideContoller.runtimeAnimatorController;
            }
            
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null) 
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if(oldWeapon == null)
            {
                return;
            }

            oldWeapon.name = "Destroying";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform LeftHand, Transform RightHand)
        {
            Transform hand;
            if (!isRightHanded)
            {
                hand = LeftHand;
            }
            else
            {
                hand = RightHand;
            }
            return hand;
        }

        public bool IsProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform LHand, Transform RHand,Health target,GameObject instigator,float StatDamage)
        {
            Projectile proj = Instantiate(projectile, GetTransform(LHand, RHand).position, GetTransform(LHand, RHand).rotation);
            proj.SetTarget(target, instigator,StatDamage);
        }
        
        public float getDamage()
        {
            return damage;
        }

        public float getRange()
        {
            return WeaponRange;
        }

        public float GetPercentageBonus()
        {
            return percentBonus;
        }

        public GameObject retunPrefab()
        {
            return weaponPrefab;
        }

    }
}