using System;
using UnityEngine;

[Serializable]
public enum SideState { Open, Closed }

[Serializable]
public enum Side { Top, Right, Bottom, Left }
[Serializable]
public class TileSide
{
    public Side Side
    {
        get { return _side; }
        set
        {
            switch (Side)
            {
                case Side.Top:
                    origin = new Vector3(0,0,2.5f);
                    break;
                case Side.Right:
                    origin = new Vector3(2.5f, 0,0);
                    break;
                case Side.Bottom:
                    origin = new Vector3(0, 0 - 2.5f);
                    break;
                case Side.Left:
                    origin = new Vector3(-2.5f, 0, 0);
                    break;
                default:
                    break;
            }
        }
    }
    public SideState State;

    public float radius = 0.5f;
    public float maxDistance;
    public Vector3 direction;
    public Vector3 origin;

    private Side _side;
    
    void SideCheck()
    {
        RaycastHit hitInfo;

        if (Physics.SphereCast(origin, radius, direction, out hitInfo, maxDistance))
        {
            Debug.Log($"Hit: {hitInfo.collider.gameObject.name}");
        }

        //Visualize the sphere case
        Debug.DrawRay(origin, direction * maxDistance, Color.red);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(origin + direction * maxDistance, radius);
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(origin, direction * maxDistance, Color.red);
        Gizmos.color = State == SideState.Open ? Color.green : Color.red;
        
        Gizmos.DrawWireSphere(origin + direction * maxDistance, radius);
    }
}
