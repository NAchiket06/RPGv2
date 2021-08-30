using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Stats
{

    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI valueText;
        BaseStats baseStats;

        private void Start()
        {
            baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }



        void Update()
        {
            valueText.text = baseStats.CalculateLevel().ToString();
        }
    }

}