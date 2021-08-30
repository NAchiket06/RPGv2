using UnityEngine;
using System.Collections;
using RPG.Saving;
using TMPro;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour,ISaveable
    {

        [SerializeField] float expPoints = 0;

        //public delegate void ExperienceGainedDelegate();
        public event Action onExperinceGained;
     
        public void GainExp(float exp)
        {
            expPoints += exp;
            onExperinceGained();
        }

        public float GetPoints()
        {
            return expPoints;
        }

        public object CaptureState()
        {
            return expPoints;
        }

        public void RestoreState(object state)
        {
            expPoints = (float)state;
        }
    }

}