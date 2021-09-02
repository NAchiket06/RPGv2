using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour,IRaycastable
    {
        [SerializeField] WeaponConfig weapon;
        [SerializeField] float RespawnTime = 3f;

        private void Start()
        {
           // weapon = transform.GetChild(0).transform.GetComponent<Weapon>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PickupWeapon(other.GetComponent<Fighter>());
                //Destroy(gameObject);
            }
        }

        private void PickupWeapon(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(RespawnTime));
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

        public bool HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButtonDown(0))
            {
                PickupWeapon(callingController.GetComponent<Fighter>());

            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
