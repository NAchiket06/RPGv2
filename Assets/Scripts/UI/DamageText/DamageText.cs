namespace RPG.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;

    public class DamageText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;

        void Destroy()
        {
            Destroy(gameObject);
        }

        public void SetDamage(float damageValue)
        {
            damageValue = (int)damageValue;
            text.text = damageValue.ToString();
        }
    }

}