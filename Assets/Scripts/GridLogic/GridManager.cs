using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public enum TileTier
{
    Predefined,
    Resource,
    Foundation,
    Path
}

[System.Serializable]
public enum TileType
{
    Locked,
    Undefined,
    Mountain,
    Forest,
    Grassfield,
    Fertalized,
    Developed,
    Path_Straight,
    Path_Turn,
    Path_Tee,
    Path_Cross
}
[System.Serializable]
public enum ResourceType
{
    Mountain,
    Forest,
    Grass
}
[System.Serializable]
public enum StructureType
{
    Base,
    Storage,
    Market,
    Watchtower,
    Barracks,
    Barn,
    LumberMill,
    Mine
}

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public LayerMask GridLayer;


    [Header("Grid Generation Properties")]
    public GameObject tilePrefab;
    public int rows = 3;
    public int columns = 3;
    public float tileSize = 5;

    [Header("Grid Details")]
    public GameObject[,] gridTiles;
    public GameObject centerTile;

    public bool gridInitilized = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Start()
    {
        GenerateInnerGrid();
    }

    public void GenerateInnerGrid()
    {
        Transform grid = new GameObject("Grid").transform;
        grid.parent = transform;
        gridTiles = new GameObject[columns, rows];

        for (int row = 0; row < rows; row++)
        {
            GameObject rowContainer = new GameObject($"Row_{row}");
            rowContainer.transform.parent = grid;

            for(int column = 0; column < columns; column++)
            {
                Vector3 position = new Vector3(column * tileSize, 0, row * tileSize);
                GameObject tileGO = Instantiate(tilePrefab, position, Quaternion.identity, rowContainer.transform);

                tileGO.name = $"Tile({row},{column})";

                TileHandler tHandler = tileGO.GetComponent<TileHandler>();
                tHandler.position = new Vector2Int(row, column);
                tHandler.CurrentTile = GameManager.Instance.GetTileData(TileType.Locked);

                gridTiles[row, column] = tileGO;
            }
        }

        foreach(var tile in gridTiles)
        {
            TileHandler tHandler = tile.GetComponent<TileHandler>();

            tHandler.CacheNeighbors();
        }

        DetectCenterTile();

        gridInitilized = true;
    }

    public void DetectCenterTile()
    {
        int centerRow = rows / 2;
        int centerColumn = columns / 2;

        if (rows % 2 == 0) centerRow -= 1;
        if (columns % 2 == 0) centerColumn -= 1;

        centerTile = gridTiles[centerColumn, centerRow];

        TileHandler tHandler = centerTile.GetComponent<TileHandler>();
        tHandler.CurrentTile = GameManager.Instance.GetTileData(TileType.Path_Cross);
        tHandler.AttachedCard = GameManager.Instance.GetStructureCard(StructureType.Base);
    }

    public TileHandler GetTile(Vector2Int tilePos)
    {
        int row = tilePos.x;
        int column = tilePos.y;

        if (row < 0 || row >= gridTiles.GetLength(0) || column < 0 || column >= gridTiles.GetLength(1))
        {
            //Debug.LogWarning($"Tile position {tilePos} is out of bounds.");
            return null; // Return null if the position is invalid
        }

        if (gridTiles[row,column] == null)
        {
            //Debug.LogWarning($"No tile exists at position {tilePos}");
            return null;
        }

        return gridTiles[row, column].GetComponent<TileHandler>();
    }
}
