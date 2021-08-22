using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using RPG.Saving;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour,IAction,ISaveable
    {
     
        [Header("Attack Parameters")]
        float timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] float TimeBetweenAttacks = 1.5f;

        [Header("Weapons")]
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform LeftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] Weapon currentWeapon = null;
        [SerializeField] string DefaultWeaponName = "Unarmed";

        [Header("Target Details")]
        Health target;


        private void Start()
        {
            Weapon weapon = Resources.Load<Weapon>(DefaultWeaponName);
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            else
            {
                EquipWeapon(weapon);
            }
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

        public void EquipWeapon(Weapon weapon)
        {
            if(weapon == null)
            {
                return;
            }
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();

            if (weapon.retunPrefab() != null)
            {
                weapon.Spawn(rightHandTransform,LeftHandTransform, animator);
            }
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
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.getRange();
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

        public bool CanAttack(GameObject combatTarget){

            if(combatTarget == null)
            {
                return false;
            }

            return combatTarget!= null && !combatTarget.GetComponent<Health>().IsDead();
        }
        //-------------------------------------------Animation Events------------------------------------------------------//
        public void Hit()
        {

           
            if(target == null)
            {
                return;
            }

            if (currentWeapon.IsProjectile())
            {
                currentWeapon.LaunchProjectile(LeftHandTransform, rightHandTransform, target);
            }
            else
            {
                target.TakeDamage(currentWeapon.getDamage());
            }
        }
        void Shoot()
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName) ;
            EquipWeapon(weapon);
        }
    }
}