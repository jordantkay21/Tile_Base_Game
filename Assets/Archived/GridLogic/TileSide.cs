using UnityEngine;

namespace KayosStudios.Archived
{
    public class SideData
    {
        public SideType side;
        public bool isOpen;
    }

    public class TileSide : MonoBehaviour
    {
        public SideType side;
        public bool isOpen;


        public void SideDetection(TileHandler TileParent, out bool isMatching)
        {
            Debug.Log($"TileSide {side} called");

            float radius = 0.5f;
            Vector3 position = transform.position;
            isMatching = false;

            //Get all colliders within the sphere
            Collider[] colliders = Physics.OverlapSphere(position, radius);

            Debug.Log($"GameObject Details: Name: {gameObject.name} \n Parent: {transform.parent} Grandparent: {transform.parent.parent}");

            foreach (Collider collider in colliders)
            {

                //Ignore the collider of this GameObject
                if (collider.gameObject == gameObject) continue;

                if (collider.GetComponent<TileHandler>())
                {
                    TileHandler tHandler = collider.GetComponent<TileHandler>();

                    if (tHandler == TileParent) continue;
                }

                if (collider.GetComponent<TileSide>())
                {
                    TileSide tSide = collider.GetComponent<TileSide>();
                    Debug.Log($"This side is open {isOpen} \n Neighboring side is open {tSide.isOpen}");
                    isMatching = ConnectionsOpen(tSide.isOpen);

                }

                Debug.Log($"Collider Details: \n Collided with: {collider.gameObject.name}");

            }

        }

        public bool ConnectionsOpen(bool detectedSide)
        {
            if (detectedSide)
            {
                if (detectedSide == isOpen) return true;
            }

            return false;
        }

        private void OnDrawGizmosSelected()
        {
            // Set up SphereCast parameters
            float radius = 0.5f;
            Vector3 origin = transform.position;

            // Gizmo Color for Visualization
            Gizmos.color = Color.green; // Use green for the SphereCast visualization

            // Draw the starting point of the SphereCast
            Gizmos.DrawWireSphere(origin, radius);
        }
    }
}