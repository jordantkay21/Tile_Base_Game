using Sirenix.OdinInspector;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Properties")]
    [ShowInInspector]
    public int GridSize
    {
        get { return _gridSize; }
        set
        {
            if (value % 2 == 0) //If the value is even
            {
                value += 1; //Force it to be odd
                Debug.LogWarning("Grid size must be odd. Adjusting to: " + value);
            }

            _gridSize = value;
        }
    }
    public GameObject tilePrefab;
    public float tileSize;
    

    private int _gridSize;

    public void Start()
    {
        Debug.Log($"Final Grid Size: {_gridSize}");
    }
}
