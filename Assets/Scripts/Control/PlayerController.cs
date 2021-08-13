using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Control{
    public class PlayerController : MonoBehaviour
    {

        void Update()
        {
            if(Input.GetMouseButton(0)){
                MoveToCursor();
            }
        }

        void MoveToCursor()
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); ;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GetComponent<Mover>().MoveTo(hit.point);
            }
        }
    }
}