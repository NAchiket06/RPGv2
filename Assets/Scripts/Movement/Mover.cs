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
        NavMeshAgent agent;

        private void Start() {
            agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            updateAnimator();
        }

        public void StartMoveAction(Vector3 destination){

            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 Destination){
            agent.SetDestination(Destination);
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
