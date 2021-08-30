using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAfterEffect : MonoBehaviour
{
    [SerializeField] GameObject ParentTarget;

    // Update is called once per frame
    void Update()
    {
        if(!GetComponent<ParticleSystem>().IsAlive())
        {
            if (ParentTarget != null)
            {
                Destroy(ParentTarget);
            }
            else Destroy(gameObject);
        }
    }
}
