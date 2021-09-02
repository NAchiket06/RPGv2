using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMapping;

        [Header("NavMesh Properties")]
        [SerializeField] float MaxNavmeshProjectionDistance = 1f;
        [SerializeField] float MaxNavMeshPathDistance = 40f;
        private void Awake()
        {
            health = GetComponent<Health>();
        }
        private void Start() 
        {
        }


        void Update()
        {

            if (InteractWithUI()) return;


            if(health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            
            if (InteractWithComponent()) return;


            if(InteractWithMovement()) return;


            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private static RaycastHit[] RaycastAllSorted()
        {

            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            float[] distances = new float[hits.Length]; 

            for(int i=0;i<hits.Length;i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances,hits);

            return hits;
        }


        private bool InteractWithMovement()
        {

            //

            Vector3 target;
            bool hasHit = RayCastNavmesh(out target);
            if (hasHit)
            {

                if (Input.GetMouseButton(0)){
                    GetComponent<Mover>().StartMoveAction(target,1f);
                }
                
                SetCursor(CursorType.Movement);

                return true;

            }
            return false;
        }


        /// <summary>
        /// raycasts to navmesh and calculates if the player can move to the cursor or not. 
        /// </summary>
        private bool RayCastNavmesh(out Vector3 target)
        {
            // target stores the point to which the player will move
            target = new Vector3();

            RaycastHit hit;

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            // if raycast does not hit anything, player cannot move there
            if (!hasHit) return false;


            NavMeshHit navMeshHit;

            //if cursor is on a specific point on the terrain which has no nav mesh available,
            //it checks whether the point has a nearby navmesh in range of MaxNavmeshProjectionDistance
            // and  if does not have, returns false
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point, out navMeshHit, MaxNavmeshProjectionDistance, NavMesh.AllAreas);

            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();

            // if the cursor has a navmesh, checks if the navmesh is reachable or not(top of mountain, 
            // top of building, etc.
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);

            if(!hasPath || path.status != NavMeshPathStatus.PathComplete)
            {
                return false;
            }

            // if the distance to destination is greater than the specified distance,
            // player cannot move there.
            if(GetPathLength(path) > MaxNavMeshPathDistance)
            {
                return false;
            }
            return true;
        }

        float GetPathLength(NavMeshPath path)
        {

            float total = 0;
            if (path.corners.Length < 2) return total;

            for(int i=0;i<path.corners.Length-1;i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
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