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
    Foundation
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
    Barn
}

public class GridManager : MonoBehaviour
{
    [Header("Grid Generation Properties")]
    public GameObject tilePrefab;
    public int rows = 3;
    public int columns = 3;
    public float tileSize = 5;

    [Header("Grid Details")]
    public GameObject[,] gridTiles;
    public GameObject centerTile;

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

                gridTiles[column, row] = tileGO;
            }
        }
    }

    public void DetectCenterTile()
    {
        int centerRow = rows / 2;
        int centerColumn = columns / 2;

        if (rows % 2 == 0) centerRow -= 1;
        if (columns % 2 == 0) centerColumn -= 1;

        centerTile = gridTiles[centerColumn, centerRow];
    }
}
