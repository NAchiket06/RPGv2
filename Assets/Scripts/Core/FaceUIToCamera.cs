namespace RPG.Core
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;

    public class FaceUIToCamera : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;


        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward   ;
        }
    }

}