using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

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
    public double offset = 4;
    public int expansionCount = 0;

    private void Awake()
    {
        if (Instance == null)
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

            for (int column = 0; column < columns; column++)
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

        foreach (var tile in gridTiles)
        {
            TileHandler tHandler = tile.GetComponent<TileHandler>();

            tHandler.CacheNeighbors();
        }

        DetectCenterTile();
        SpawnPathAroundCenter();
        SpawnResourcesAroundCenter();

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

    public void SpawnPathAroundCenter()
    {
        TileHandler tHandler = centerTile.GetComponent<TileHandler>();

        foreach (var kvp in tHandler.cardinalNeighboringTilesMap)
        {
            TileHandler neighbor = kvp.Value;

            neighbor.CurrentTile = GameManager.Instance.GetTileData(TileType.Path_Straight);
        }
    }

    public void SpawnResourcesAroundCenter()
    {
        //Debug.Log("SpawnResourcesAroundCenter: Starting resource placement around center tile.");


        if (centerTile == null)
        {
            //Debug.LogError("SpawnResourcesAroundCenter: Center tile is null. Ensure DetectCenterTile() is called first.");
            return;
        }

        TileHandler centerTileHandler = centerTile.GetComponent<TileHandler>();
        if (centerTileHandler == null)
        {
            //Debug.LogError("SpawnResourcesAroundCenter: Center tile does not have a TileHandler component.");
            return;
        }

        //Debug.Log("SpawnResourcesAroundCenter: Retrieved center tile handler successfully.");

        TileType[] resourceTypes = { TileType.Grassfield, TileType.Grassfield, TileType.Mountain, TileType.Forest };
        //Debug.Log("SpawnResourcesAroundCenter: Shuffling resource types.");
        // Shuffle the resourceTypes array for randomness
        for (int i = resourceTypes.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            TileType temp = resourceTypes[i];
            resourceTypes[i] = resourceTypes[randomIndex];
            resourceTypes[randomIndex] = temp;
        }

        int resourceIndex = 0;

        //Debug.Log($"SpawnResourcesAroundCenter: Found {centerTileHandler.intercardinalNeighboringTilesMap.Count} intercardinal neighbors.");

        foreach (var kvp in centerTileHandler.intercardinalNeighboringTilesMap)
        {
            SideType side = kvp.Key;
            TileHandler neighbor = kvp.Value;

            if (resourceIndex >= resourceTypes.Length)
            {
                //Debug.Log("SpawnResourcesAroundCenter: All resources have been assigned.");
                break;
            }

            if (neighbor == null)
            {
                //Debug.LogWarning($"SpawnResourcesAroundCenter: Neighbor on side {side} is null.");
                continue;
            }

            // Log the current state of the neighbor tile
            //Debug.Log($"SpawnResourcesAroundCenter: Neighbor on side {side} has current tile type: {neighbor.CurrentTile.Type}");



            if (neighbor.CurrentTile.Type == TileType.Locked)
            {
                neighbor.CurrentTile = GameManager.Instance.GetTileData(resourceTypes[resourceIndex]);
                //Debug.Log($"SpawnResourcesAroundCenter: Assigned {resourceTypes[resourceIndex]} to neighbor on side {side}.");
                resourceIndex++;
            }
            else
            {
                Debug.Log($"SpawnResourcesAroundCenter: Neighbor on side {side} is not locked. Skipping.");
            }
        }

        //Debug.Log("SpawnResourcesAroundCenter: Resource placement completed.");
    }

    [Button("Add Outer Ring")]
    public void AddOuterRing()
    {
        Debug.Log($"AddOuterRing | Expanding grid...");

        if(expansionCount == 5)
        {
            Debug.Log("Can not expand further!");
            return;
        }

        #region STEP ONE: Retrieve Current Grid Dimensions
        int currentRows = gridTiles.GetLength(0);
        int currentColumns = gridTiles.GetLength(1);
        Debug.Log($"Current Grid: {currentRows} rows x {currentColumns} columns");
        #endregion

        #region STEP TWO: Calculate New Grid Dimensions
        int newRows = currentRows + 2;
        int newColumns = currentColumns + 2;
        GameObject[,] newGridTiles = new GameObject[newRows, newColumns];
        Debug.Log($"New Grid: {newRows} rows x {newColumns} columns");
        #endregion

        #region STEP THREE: Calculate Center Shift
        expansionCount++;

        int rowOffset = 1;
        int columnOffset = 1;
        Debug.Log($"Offset for old tiles: rowOffset={rowOffset}, columnOffset={columnOffset}");
        #endregion

        #region STEP FOUR: Copy Old Tiles into Expanded Grid
        for (int row = 0; row < currentRows; row++)
        {
            for (int column = 0; column < currentColumns; column++)
            {
                int newRow = row + rowOffset;
                int newColumn = column + columnOffset;
                newGridTiles[newRow, newColumn] = gridTiles[row, column];

                // Update tile metadata
                TileHandler tHandler = newGridTiles[newRow, newColumn].GetComponent<TileHandler>();
                tHandler.position = new Vector2Int(newRow, newColumn);
                tHandler.gameObject.name = $"Tile({newRow},{newColumn})";
            }
        }
        Debug.Log($"Existing tiles successfully transferred.");
        #endregion

        #region STEP FIVE: Generate Outer Ring Tiles
        Transform gridParent = transform.Find("Grid") ?? new GameObject("Grid").transform;

        #region Configure Position Offset
        switch (expansionCount)
        {
            case 1:
                offset = 4;
                break;
            case 2:
                offset = 3;
                break;
            case 3:
                offset = 2.66;
                break;
            case 4:
                offset = 2.5;
                break;
            case 5:
                offset = 2.4;
                break;
        }
        #endregion

        for (int row = 0; row < newRows; row++)
        {
            for (int column = 0; column < newColumns; column++)
            {
                if (newGridTiles[row, column] == null) // Only create new tiles
                {

                    Vector3 position = new Vector3((float)((column - (newColumns - 1) / offset) * tileSize), 0, (float)((row - (newRows - 1) / offset) * tileSize));
                    Debug.Log($"<color=aqua> Spawning new tile [{row},{column}] at... " +
                        $"\n X AXIS -> [{(column - (newColumns - 1) / offset) * tileSize}] | (column - (newColumns - 1) / offset) * tileSize) | ({column} - ({newColumns} - 1) / {offset}) * {tileSize})" +
                        $"\n Y AXIS -> 0 " +
                        $"\n Z AXIS -> [{(row - (newRows - 1) / offset) * tileSize}] | (row - (newRows - 1) / offset) * tileSize | ({row} - ({newRows} - 1) / {offset} * {tileSize}) | ");

                    GameObject tileGO = Instantiate(tilePrefab, position, Quaternion.identity, gridParent);
                    tileGO.name = $"Tile({row},{column})";

                    TileHandler tHandler = tileGO.GetComponent<TileHandler>();
                    tHandler.position = new Vector2Int(row, column);

                    bool shouldBeUndefined = Random.value < 0.25f;
                    tHandler.CurrentTile = GameManager.Instance.GetTileData((shouldBeUndefined ? TileType.Undefined : TileType.Locked));

                    newGridTiles[row, column] = tileGO;
                }
            }
        }
        Debug.Log($"Outer ring tiles successfully created.");
        #endregion

        #region STEP SIX: Update the Grid Reference & Refresh Hierarchy
        gridTiles = newGridTiles;
        RefreshTileHierarchy();
        Debug.Log($"Grid expansion completed.");
        #endregion

        DebugPrintGrid();
    }


    [Button("Get Tile")]
    public TileHandler GetTile(Vector2Int tilePos)
    {
        int row = tilePos.x;
        int column = tilePos.y;

        if (row < 0 || row >= gridTiles.GetLength(0) || column < 0 || column >= gridTiles.GetLength(1))
        {
            //Debug.LogWarning($"Tile position {tilePos} is out of bounds.");
            return null; // Return null if the position is invalid
        }

        if (gridTiles[row, column] == null)
        {
            //Debug.LogWarning($"No tile exists at position {tilePos}");
            return null;
        }

        return gridTiles[row, column].GetComponent<TileHandler>();
    }

    public void RefreshTileHierarchy()
    {
        Transform gridParent = transform.Find("Grid");

        if (gridParent == null)
        {
            Debug.LogError("Grid parent not found! Ensure the grid structure matches the initial setup.");
            return;
        }

        // Iterate through the entire grid and update hierarchy
        for (int row = 0; row < gridTiles.GetLength(0); row++)
        {
            // Ensure the row parent exists
            Transform rowParent = gridParent.Find($"Row_{row}");
            if (rowParent == null)
            {
                rowParent = new GameObject($"Row_{row}").transform;
                rowParent.parent = gridParent;
            }

            for (int column = 0; column < gridTiles.GetLength(1); column++)
            {
                if (gridTiles[row, column] != null)
                {
                    TileHandler tHandler = gridTiles[row, column].GetComponent<TileHandler>();

                    // Update tile position data
                    tHandler.position = new Vector2Int(row, column);
                    tHandler.gameObject.name = $"Tile({row},{column})";

                    // Reparent the tile correctly under the row parent
                    tHandler.transform.parent = rowParent;
                }
            }
        }

        Debug.Log("<color=green>Grid hierarchy and tile data successfully refreshed!</color>");
    }

    private void DebugPrintGrid()
    {
        string gridOutput = "\n";
        for (int row = gridTiles.GetLength(0) - 1; row >= 0; row--)
        {
            for (int col = 0; col < gridTiles.GetLength(1); col++)
            {
                gridOutput += gridTiles[row, col] != null ? "X " : ". ";
            }
            gridOutput += "\n";
        }
        Debug.Log(gridOutput);
    }

}
