using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour,IAction
    {
        
        [SerializeField] float WeaponRange = 2f;
        [SerializeField] float TimeBetweenAttacks = 1.5f;
        [SerializeField] float damage = 30f;
        float timeSinceLastAttack = Mathf.Infinity;
        Health target;


        private void Update()
        {

            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;

            if(target.IsDead()) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
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
            return Vector3.Distance(transform.position, target.transform.position) < WeaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
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
        public void Hit(){

            if(target == null)
            {
                return;
            }
            target.TakeDamage(damage);
        }
    }
}