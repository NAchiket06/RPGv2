using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;

        [SerializeField] float MaxSpeed = 5f;
        NavMeshAgent agent;

        private void Start() {
            agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            agent.enabled = !GetComponent<Health>().IsDead();
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


    }
}
