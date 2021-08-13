using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float HealtHPoints = 100f;
        bool isDead = false;

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
        }
    }
}