using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{

    [CreateAssetMenu(fileName = "New Progression", menuName = "Stats Data/Progression", order = 0)]
    public class Progression : ScriptableObject
    {

        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            float[] levels = lookTable[characterClass][stat];
            if(levels.Length < level)
            {
                return 0;
            }
            return levels[level-1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();
            float[] levels = lookTable[characterClass][stat];
            return levels.Length;
        }
        private void BuildLookup()
        {
            if (lookTable != null) return;

            lookTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (ProgressionCharacterClass classs in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in classs.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                lookTable[classs.characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [Header("Character Class")]
            public CharacterClass characterClass;
            [Header("Character Stats")]
            public ProgressionStat[] stats;
            //public float[] Healths;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }

    }
}