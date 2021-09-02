using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using GameDevTV.Utils;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour,ISaveable
    {
        float MaxHp;
        LazyValue<float> HealthPoints;
        [SerializeField] float RegenRate = 0.25f;

        [SerializeField] TakeDamageEvent takeDamage;

        [System.Serializable]
        public class TakeDamageEvent:UnityEvent<float>
        {
        }

        [SerializeField] UnityEvent onDie;

        public bool isDead = false;
        public bool IsDead()
        {
            return isDead;
        }

        private void Awake()
        {
            MaxHp = GetComponent<BaseStats>().GetStat(Stat.Health);
            HealthPoints = new LazyValue<float>(GetInitialHealth);

        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            HealthPoints.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += OnLevelUp;

        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= OnLevelUp;
        }

        void OnLevelUp()
        {
            MaxHp = GetComponent<BaseStats>().GetStat(Stat.Health);
            HealthPoints.value = MaxHp;
        }

        private void Update()
        {
            if (gameObject.CompareTag("Player"))
            {
                if (HealthPoints.value < MaxHp)
                    HealthPoints.value += RegenRate * Time.deltaTime;

                HealthPoints.value = Mathf.Clamp(HealthPoints.value, 0, MaxHp);
            }
        }

        public void TakeDamage(GameObject instigator, float damage){

            //print(gameObject.name + " took damage " + damage);
            HealthPoints.value = Mathf.Max(HealthPoints.value - damage,0f);
            takeDamage.Invoke(damage);

            if (HealthPoints.value == 0f && !isDead){
                onDie.Invoke();
                Dead();
                AwardExp(instigator);
            }
        }

        public float GetHealthPoints()
        {
            return HealthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return MaxHp;
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
            return HealthPoints;
        }

        public void RestoreState(object state)
        {
            HealthPoints.value = (float)state;
            if (HealthPoints.value == 0f && !isDead)
            { 
                Dead();
                Destroy(gameObject);
            }

            if(HealthPoints.value > 0 && isDead)
            {
                GetComponent<Animator>().SetTrigger("respawned");
                isDead = false; 
            }
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetCurrentHealth()
        {
            return HealthPoints.value;
        }

        private void AwardExp(GameObject instigator)
        {
            Experience exp = instigator.GetComponent<Experience>();
            if(exp == null)
            {
                return;
            }

            exp.GainExp(GetComponent<BaseStats>().GetStat(Stat.ExpReward));
        }

    }
}