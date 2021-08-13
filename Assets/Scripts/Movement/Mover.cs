using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {

        Ray lastRay;

        void Update()
        {
            updateAnimator();
        }



        public void MoveTo(Vector3 Destination){
            GetComponent<NavMeshAgent>().SetDestination(Destination);

        }

        private void updateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("forwardSpeed",speed);

        }
    }
}
