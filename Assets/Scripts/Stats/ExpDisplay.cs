using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Stats
{

    public class ExpDisplay : MonoBehaviour
    {
        [SerializeField]TextMeshProUGUI valeuText;
        Experience player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }



        void Update()
        {
            valeuText.text = player.GetPoints().ToString();
        }
    }

}