using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{

    Ray lastRay;
    
    void Start()
    {
        
    }


    void Update()
    {

        if(Input.GetMouseButton(0)){
            MoveToCursor();
        }
        updateAnimator();
    }



    void MoveToCursor(){

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); ;
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit)){
            GetComponent<NavMeshAgent>().SetDestination(hit.point);
        }
        
    }

    private void updateAnimator()
    {
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;

        GetComponent<Animator>().SetFloat("forwardSpeed",speed);

    }
}
