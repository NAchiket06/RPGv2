using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Saving;

namespace RPG.Cinematics
{

    public class CinematicTrigger : MonoBehaviour,ISaveable
    {

        public bool isPlayed = false;

        

        private void OnTriggerEnter(Collider other) {

            if(other.gameObject.CompareTag("Player") && !isPlayed)
            {
                GetComponent<PlayableDirector>().Play();
                isPlayed = true;
            }
        }

        public object CaptureState()
        {
            Dictionary<string, bool> data = new Dictionary<string, bool>();
            data["played"] = isPlayed;
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, bool> data = (Dictionary<string, bool>)state;
            isPlayed = (data["played"]);
        }

    }
}