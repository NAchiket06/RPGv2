using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        [SerializeField] float RespawnTime = 3f;

        private void Start()
        {
           // weapon = transform.GetChild(0).transform.GetComponent<Weapon>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(RespawnTime));
                //Destroy(gameObject);
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            EnablePickup(false);
            yield return new WaitForSeconds(seconds);
            EnablePickup(true);
        }

        private void EnablePickup(bool show)
        {
            GetComponent<Collider>().enabled = show;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(show);
            }
        }
    }
}
