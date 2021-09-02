using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Attributes;
using RPG.Saving;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour,IAction,ISaveable,IModifierProvider
    {
     
        [Header("Attack Parameters")]
        float timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] float TimeBetweenAttacks = 1.5f;

        [Header("Weapons")]
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform LeftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        
        [SerializeField] string DefaultWeaponName = "Unarmed";

        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;


        [Header("Target Details")]
        Health target;

        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
            
        }

        private void Start()
        {
            //WeaponConfig weapon = Resources.Load<WeaponConfig>(DefaultWeaponName);
            //if (currentWeaponConfig == null)
            //{
            //    currentWeaponConfig.ForceInit();
            //}
            //else
            //{
            //    EquipWeapon(weapon);
            //}

            //AttachWeapon(currentWeaponConfig);
            currentWeapon.ForceInit();
        }


        private void Update()
        {

            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;

            if(target.IsDead()) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position,1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();

            return weapon.Spawn(rightHandTransform, LeftHandTransform, animator);

        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);

            if(timeSinceLastAttack >= TimeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
            //start weapon trail
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeaponConfig.getRange();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            GetComponent<Mover>().Cancel();
            StopAttack();

        }

        private void StopAttack()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
        }
        public IEnumerable<float> GetAdditiveModidfier(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeaponConfig.getDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }


        public bool CanAttack(GameObject combatTarget){

            if(combatTarget == null)
            {
                return false;
            }

            return combatTarget!= null && !combatTarget.GetComponent<Health>().IsDead();
        }

        public Health GetTarget()
        {
            return target;
        }
        //-------------------------------------------Animation Events------------------------------------------------------//
        public void Hit()
        {

           
            if(target == null)
            {
                return;
            }
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if(currentWeapon.value != null)
            {
                currentWeapon.value.onHit();
            }
            
            if (currentWeaponConfig.IsProjectile())
            {
                currentWeaponConfig.LaunchProjectile(LeftHandTransform, rightHandTransform, target,gameObject,damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }
        void Shoot()
        {
            Hit();
        }

        //-------------------------------------------Save Load Functions------------------------------------------------------//

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName) ;
            EquipWeapon(weapon);
        }

        
    }
}