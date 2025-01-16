using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour
{
    [Tooltip("0 = Mountain | 1 = Forest | 2 = Grassfield")]
    public GameObject[] resourceObjects = new GameObject[3];
    [Tooltip("0 = Barn")]
    public GameObject[] fertailizedStructures = new GameObject[1];
    [Tooltip("0 = Base | 1 = Storage | 2 = Market | 3 = Watchtower | 4 = Barracks")]
    public GameObject[] developedStructures = new GameObject[6];
    [Tooltip("0 = Flag | 1 = Cannon | 2 = GaurdTower | 3 = Barracks | 4 = House")]
    public GameObject[] pathObjects = new GameObject[1];

    [ShowInInspector]
    public Dictionary<TileType, GameObject[]> tileObjects = new Dictionary<TileType, GameObject[]>();

    [Button]
    public void ConfigureDictionary()
    {
        tileObjects.Clear();

       foreach (TileType type in Enum.GetValues(typeof(TileType)))
        {
            switch (type)
            {
                case TileType.Locked:
                    break;
                case TileType.Undefined:
                    break;
                case TileType.Resource:
                    tileObjects.Add(type, resourceObjects);
                    break;
                case TileType.Fertalized:
                    tileObjects.Add(type, fertailizedStructures);
                    break;
                case TileType.Developed:
                    tileObjects.Add(type, developedStructures);
                    break;
                case TileType.Path:
                    tileObjects.Add(type, pathObjects);
                    break;
                default:
                    break;
            }
        }
    }
}
