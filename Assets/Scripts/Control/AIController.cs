using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float ChaseDistance = 5f;
        [SerializeField] float TimeSinceLastSawPlayer, SuspicionTime = 5f;
        GameObject player;
        Fighter fighter;
        Health health;

        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float WaypointTolerance = 1f;
        int currentWaypointIndex;
        float timeSinceArrivedAtWaypoint;
        [SerializeField] float WaypointDwellTime = 5f;

        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.3f;

        Vector3 guardPosition;

        private void Start() 
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();

            guardPosition = transform.position;
        }

        private void Update()
        {

            if (health.IsDead())
            {
                return;
            }
            if (InAttackRange() && fighter.CanAttack(player))
            {
                TimeSinceLastSawPlayer = 0f;
                AttackBehaviour();
            }
            else if (TimeSinceLastSawPlayer < SuspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            TimeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {

            Vector3 nextPosition = guardPosition;

            if(patrolPath != null)
            {
                if(atWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;
                    cycleWaypoint();
                }

                nextPosition = getCurrentWaypoint();
            }
            //fighter.Cancel();

            if(timeSinceArrivedAtWaypoint > WaypointDwellTime){
                GetComponent<Mover>().StartMoveAction(nextPosition,patrolSpeedFraction);
            }
        }

        private Vector3 getCurrentWaypoint()
        {
            return patrolPath.getWaypoint(currentWaypointIndex);
        }

        private void cycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool atWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position,getCurrentWaypoint());
            return distanceToWaypoint < WaypointTolerance; 
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private bool InAttackRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < ChaseDistance;
        }


        private void OnDrawGizmosSelected()  
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position,ChaseDistance);   
        }
    }
}