using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        enum CursorType
        {
            None,
            Movement,
            Combat
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMapping;
        private void Awake()
        {
            health = GetComponent<Health>();
        }
        private void Start() 
        {
        }


        void Update()
        {
            if(health.IsDead())
            {
                return;
            }
            if(InteractWithCombat()) return;
            if(InteractWithMovement()) return;

            SetCursor(CursorType.None);

        }

        private bool InteractWithCombat(){
            
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits){
                
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if(target == null) 
                {
                    continue;
                }

                if(!GetComponent<Fighter>().CanAttack(target.gameObject)){
                    continue;
                }

                if(Input.GetMouseButtonDown(0)){
                    GetComponent<Fighter>().Attack(target.gameObject);
                }

                SetCursor(CursorType.Combat);
                return true;
            }

            return false;
        }

        
        private bool InteractWithMovement()
        {

            RaycastHit hit;
            if (Physics.Raycast(GetMouseRay(), out hit))
            {

                if (Input.GetMouseButton(0)){
                    GetComponent<Mover>().StartMoveAction(hit.point,1f);
                }
                
                SetCursor(CursorType.Movement);

                return true;

            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping cursorMapping = GetCursorMapping(type);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping cmap in cursorMapping)
            {
                if (cmap.type == type)
                {
                    return cmap;
                }
            }

            return cursorMapping[0];
        }

    }
}