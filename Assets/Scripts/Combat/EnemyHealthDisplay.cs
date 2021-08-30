using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using TMPro;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Health health;
        [SerializeField]TextMeshProUGUI value;
        [SerializeField] TextMeshProUGUI label;

        Fighter fighter;
        private void Awake()
        {
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }
        private void Start()
        {
            //value = GetComponent<TextMeshProUGUI>();
            value.enabled = false;
        }

        private void Update()
        {
            health = fighter.GetTarget();
            if (health != null)
            {
                label.enabled = true;
                value.enabled = true;
                value.text  = health.GetCurrentHealth().ToString() + " / " + health.GetMaxHealth().ToString();
            }
            else
            {
                value.enabled = false;
                label.enabled = false;
            }
        }
    }
}   