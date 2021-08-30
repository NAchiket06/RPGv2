using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {

        [Header("Character Data")]
        [Range(0,5)]
        [SerializeField] int StartingLevel = 1;
        [SerializeField] CharacterClass characterClass;

        [SerializeField] Progression progression = null;

        [SerializeField] GameObject LevelUpParticleEffect;

        [SerializeField] bool isPlayer = false;

        LazyValue<int> currentLevel;

        Experience exp;

        public event Action OnLevelUp;

        private void Awake()
        {
            exp = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            //currentLevel = CalculateLevel(); 
            currentLevel.ForceInit();
        }
        private void OnEnable()
        {
            if (exp != null)
            {
                exp.onExperinceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (exp != null)
            {
                exp.onExperinceGained -= UpdateLevel;
            }
        }

        private void Update()
        {

        }

        void UpdateLevel()
        {

            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                OnLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(LevelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return GetBaseStat(stat) + GetAdditiveModifier(stat);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, CalculateLevel()) + (1* GetPercentageModifier(stat)/100);
        }

      
        private float GetAdditiveModifier(Stat stat)
        {

            if (!isPlayer) return 0;

            float total = 0;
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifiers in provider.GetAdditiveModidfier(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }
        private float GetPercentageModifier(Stat stat)
        {
            if (!isPlayer) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetPercentageModifier(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }
        public int CalculateLevel()
        {

            Experience experience = GetComponent<Experience>();

            if (experience == null) return StartingLevel;

            float curExp = experience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExpToLevelUp, characterClass);
            for (int level = 1; level< penultimateLevel;level++)
            {
                float xpToLevelUp = progression.GetStat(Stat.ExpToLevelUp, characterClass, level);
                if(xpToLevelUp > curExp)
                {
                    return level;
                }

            }

            return penultimateLevel + 1;
        }

    }
}