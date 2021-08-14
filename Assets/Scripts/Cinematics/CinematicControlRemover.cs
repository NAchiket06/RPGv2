using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {

        GameObject player;

       void Start() 
       {
            GetComponent<PlayableDirector>().played += DisableControl;  
            GetComponent<PlayableDirector>().stopped += EnableControl;

            player = GameObject.FindGameObjectWithTag("Player");

        }

        void EnableControl(PlayableDirector playableDirector)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }

        void DisableControl(PlayableDirector playableDirector)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }
    }

}