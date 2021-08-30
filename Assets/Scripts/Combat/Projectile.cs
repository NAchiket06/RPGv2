using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [Header("Projectile Parameters")]
        [SerializeField] float speed = 1;
        [SerializeField] bool isHoming = true;
        [SerializeField] float MaxLifetime = 10f;
        Health target = null;
        float damage = 0;

        GameObject instigator = null;
        [Header("Hit Effects")]
        [SerializeField] bool DidHit = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float LifeAfterImpact = 5f;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
            Destroy(gameObject, MaxLifetime);
        }

        void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            if (!isHoming && !DidHit)
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target,GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            DidHit = true;
            if (hitEffect != null)
            {
                GameObject hitParticles = Instantiate(hitEffect, transform.position, transform.rotation);
                Destroy(hitParticles, 5f);
            }
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            target.TakeDamage(instigator, damage);

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, LifeAfterImpact);
        }

    }
}