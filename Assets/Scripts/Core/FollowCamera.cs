using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Vector3 offset;
        [SerializeField] GameObject player;

        private void LateUpdate() {

            if(player != null)
                transform.position = player.transform.position + offset;
            else{
                Debug.Log("No player on FollowCamera");
            }

        }   
    }
}