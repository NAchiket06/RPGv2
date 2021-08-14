using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{

    public class CinematicTrigger : MonoBehaviour
    {

        bool isPlayed = false;
        private void OnTriggerEnter(Collider other) {

            if(other.gameObject.CompareTag("Player") && !isPlayed)
            {
                GetComponent<PlayableDirector>().Play();
                isPlayed = true;
            }
        }
    }
}