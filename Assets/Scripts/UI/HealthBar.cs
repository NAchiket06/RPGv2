using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] float maxHealth,currentHealth;

    [SerializeField] Image healthImage;
    void Start()
    {
        maxHealth = transform.parent.GetComponent<Health>().GetMaxHealth();
        currentHealth = transform.parent.GetComponent<Health>().GetHealthPoints();
        onUpdate();
    }

    
    void Update()
    {
        
    }

    public void onUpdate()
    {
        maxHealth = transform.parent.GetComponent<Health>().GetMaxHealth();
        currentHealth = transform.parent.GetComponent<Health>().GetHealthPoints();

        float fraction = currentHealth / maxHealth;
        healthImage.rectTransform.localScale = new Vector3(fraction, 1, 1);
        if(fraction == 1 || fraction == 0)
        {
            GetComponentInChildren<Canvas>().enabled = false;
        }
        else
        {
            GetComponentInChildren<Canvas>().enabled = true;
        }

    }
}
