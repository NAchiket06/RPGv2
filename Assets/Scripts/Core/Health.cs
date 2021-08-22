using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
namespace RPG.Core
{
    public class Health : MonoBehaviour,ISaveable
    {
        [SerializeField] float HealtHPoints = 100f;


        public bool isDead = false;
        public bool IsDead()
        {
            return isDead;
        }


        public void TakeDamage(float damage){
            HealtHPoints = Mathf.Max(HealtHPoints - damage,0f);

            if(HealtHPoints ==0f && !isDead){
                Dead();
            }
        }

        private void Dead()
        {
            GetComponent<Animator>().SetTrigger("dead");
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<CapsuleCollider>().isTrigger = true;
        }


        public object CaptureState()
        {
            return HealtHPoints;
        }

        public void RestoreState(object state)
        {
            HealtHPoints = (float)state;
            if (HealtHPoints == 0f && !isDead)
            { 
                Dead();
                Destroy(gameObject);
            }

            if(HealtHPoints > 0 && isDead)
            {
                GetComponent<Animator>().SetTrigger("respawned");
                isDead = false; 
            }
        }

    }
}