using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {

        [SerializeField] float sphereRadius = 0.25f;
        int ChildCount;
        private void OnDrawGizmos() {
            ChildCount = transform.childCount;
            for(int i=0;i<ChildCount;i++){
                Gizmos.DrawSphere(getWaypoint(i),sphereRadius);
                int j = GetNextIndex(i);
                Gizmos.DrawLine(getWaypoint(i), getWaypoint(j));
            }

        }

        public int GetNextIndex(int i)
        {
            if(i < ChildCount-1){
                return i+1;
            }
            else return 0;
        }

        public Vector3 getWaypoint(int i){
            return transform.GetChild(i).position;
        }
    }

}