using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction,ISaveable
    {
        [SerializeField] Transform target;

        [SerializeField] float MaxSpeed = 5f;

        NavMeshAgent agent;
        Health health;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            agent.enabled = !health.IsDead();
            updateAnimator();
        }

        public void StartMoveAction(Vector3 destination,float SpeedFraction){

            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination,SpeedFraction);
        }

        public void MoveTo(Vector3 Destination,float SpeedFraction){
            agent.SetDestination(Destination);
            if(!transform.gameObject.CompareTag("Player")){
                agent.speed = MaxSpeed * Mathf.Clamp(0, 1, SpeedFraction);
            }
            else{
                agent.speed = 6f;
            }
            agent.isStopped = false;

        }

        public void Cancel(){
            agent.isStopped = true;
        }
        private void updateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("forwardSpeed",speed);

        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;

        }
    }
}
