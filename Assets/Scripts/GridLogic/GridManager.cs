using Sirenix.OdinInspector;
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
        Debug.Log($"AddOuterRing | Outer Ring Calculating... ");

        #region STEP ONE: Retreive Current Grid Dimension
        // Get current dimensions
        int currentRows = gridTiles.GetLength(0);
        int currentColumns = gridTiles.GetLength(1);

        Debug.Log($"<color=black> AddOuterRing | STEP ONE: Retrieve Current Grid Dimensions " +
            $"\n currentRows = {currentRows} & currentColumns = {currentColumns} </color>");
        #endregion

        #region STEP TWO: Calculate New Grid Dimensions
        // New dimensions for the outer ring
        int newRows = currentRows + 2;
        int newColumns = currentColumns + 2;

        Debug.Log($"<color=black> AddOuterRing | STEP TWO: Calculate New Grid Dimensions " +
            $"\n newRows = {newRows} & newColumns = {newColumns} </color>");
        #endregion

        #region STEP THREE: Create a New Grid Array
        // Create a new grid with updated dimensions
        GameObject[,] newGridTiles = new GameObject[newRows, newColumns];

        Debug.Log($"<color=black> AddOuterRing | STEP THREE: Create a New Grid Array " +
            $"\n newGridTiles = {newGridTiles} </color>");
        #endregion

        #region STEP FOUR: Define Offsets for the Original Grid
        // Offset for translating old coordinates to new grid
        int rowOffset = 1;
        int columnOffset = 1;

        Debug.Log($"<color=black> AddOuterRing | STEP FOUR: Define Offsets for the Original Grid " +
            $"\n rowOffset = {rowOffset} & columnOffset = {columnOffset} </color>");
        #endregion

        #region STEP FIVE: Retrieve the Grid Parent
        // Reference to the grid parent
        Transform gridParent = transform.Find("Grid");


        if (gridParent == null)
        {
            Debug.LogError("Grid parent not found! Ensure the grid structure matches the initial setup.");
            return;
        }

        Debug.Log($"<color=black> AddOuterRing | STEP FIVE: Get the Grid Parent " +
            $"\n gridParent = {gridParent} </color>");
        #endregion

        #region STEP SIX: COPY EXISTING TILES TO THE NEW GRID
        Debug.Log($"<color=olive> AddOuterRing | STEP SIX: COPY EXISTING TILES TO THE NEW GRID " +
            $"\n Scan starting...");
        // Copy existing tiles to the new grid with offset
        for (int row = 0; row < currentRows; row++)
        {
            Debug.Log($"AddOuterRing/Step6 | Iterating through <color=aqua>currentRow {row}</color>");
            for (int column = 0; column < currentColumns; column++)
            {
                int newRow = row + rowOffset;
                int newColumn = column + columnOffset;

                Debug.Log($"AddOuterRing/Step6 | Transferring <color=aqua>tile ({row},{column})</color> " +
                    $"\n Tile({row},{column}) is now <color=aqua>Tile({newRow},{newColumn}</color>");

                newGridTiles[newRow, newColumn] = gridTiles[row, column];

                //TileHandler tHandler = newGridTiles[newRow, newColumn].GetComponent<TileHandler>();
                //tHandler.position = new Vector2Int(newRow, newColumn);
                //tHandler.gameObject.name = $"Tile({newRow},{newColumn})";

                //// Reassign the tile's parent to the correct new row
                //Transform newRowParent = gridParent.Find($"Row_{newRow}");
                //if (newRowParent == null)
                //{
                //    newRowParent = new GameObject($"Row_{newRow}").transform;
                //    newRowParent.parent = gridParent;
                //}
                //tHandler.transform.parent = newRowParent;
            }
        }

        Debug.Log($"<Color=olive> AddOuterRing | STEP SIX: COPY EXISTING TILES TO THE NEW GRID</color> " +
            $"\n <Color=purple>Existing Grid transfered to new grid with new coordinates </color>");
        #endregion

        #region STEP SEVEN: CREATE NEW ROWS AND SPAWN TILES
        Debug.Log($"<Color=olive> AddOuterRing | STEP SEVEN: CREATE NEW ROWS AND SPAWN TILES " +
            $"\n Begining to scan new grid...");
        // Create new rows and add tiles for the outer ring
        for (int newRow = 0; newRow < newRows; newRow++)
        {
            //Calculate the grid-relative row coordinate
            int gridRow = newRow - rowOffset;
            Debug.Log($"<color=#ff7f50>AddOuterRing/Step7 | <color=#ff7f50>grid-relative <b>row</b> coordinate is {gridRow}</color><color=aqua> (newRow({newRow}) - rowOffset({rowOffset}))</color></color>");

            //Find or create the correct row parent baed on grid-relative row coordinate
            Debug.Log($"AddOuterRing/Step7 | Attempting to Find parent of <color=aqua>row_{gridRow}</color>");
            Transform rowParent = gridParent.Find($"Row_{gridRow}");

            if (rowParent == null)
            {
                rowParent = new GameObject($"Row_{gridRow}").transform;
                rowParent.parent = gridParent;
                Debug.Log($"AddOuterRing/Step7 | <color=yellow>Row_{gridRow} was not found. Row created within hierarchy.</color>");
            }
            else
            {
                Debug.Log($"AddOuterRing/Step7 | <color=green>Row_{gridRow} was found!</color>");
            }

            for (int newColumn = 0; newColumn < newColumns; newColumn++)
            {
                Debug.Log($"AddOuterRing/Step7 | Scanning for tile at location <color=aqua>row({newRow}),column({newColumn})</color>");
                if (newGridTiles[newRow, newColumn] == null) // Only spawn new tiles
                {
                    // Calculate the grid-relative position
                    int gridColumn = newColumn - columnOffset;
                    Debug.Log($"AddOuterRing/Step7 | <color=#ff7f50> grid-relative <b>column</b> coordinate is {gridColumn}</color> <color=aqua>(newColumn({newColumn}) - columnOffset({columnOffset}))</color></color>");

                    Vector3 position = new Vector3(gridColumn * tileSize, 0, gridRow * tileSize);
                    GameObject tileGO = Instantiate(tilePrefab, position, Quaternion.identity, rowParent);

                    tileGO.name = $"Tile({newRow},{newColumn})";

                    TileHandler tHandler = tileGO.GetComponent<TileHandler>();
                    tHandler.position = new Vector2Int(newRow, newColumn);
                    tHandler.CurrentTile = GameManager.Instance.GetTileData(TileType.Locked);

                    newGridTiles[newRow, newColumn] = tileGO;
                    Debug.Log($"$AddOuterRing / Step7 | <color=aqua>Tile_{newRow},{newColumn}</color> has been created and added to the newGridTiles[]");
                }
            }
        }
        Debug.Log($"<Color=black> AddOuterRing | STEP SEVEN: CREATE NEW ROWS AND SPAWN TILES " +
    $"\n <color=purple>All new tiles have been created and added </color>");
        #endregion

        #region STEP EIGHT: UPDATE THE GRID REFERENCE
        // Update the grid reference
        gridTiles = newGridTiles;
        Debug.Log($"<Color=black> AddOuterRing | STEP EIGHT: UPDATE THE GRID REFERENCE");
        #endregion

        #region STEP NINE: CASCHE NEIGHBORS FOR ALL TILES
        // Cache neighbors for all tiles
        foreach (var tile in gridTiles)
        {
            if (tile != null)
            {
                TileHandler tHandler = tile.GetComponent<TileHandler>();
                tHandler.CacheNeighbors();
            }
        }
        Debug.Log($"<color=black> AddOuterRing | STEP NINE: CASCHE NEIGHBORS FOR ALL TILES</color>");
        #endregion
        Debug.Log("<color=fuchsia> AddOuterRing | Outer Ring has been completed with updated coordinates and neighbor reference updates.");

        #region STEP TEN: REFRESH TILE HIERARCHY
        RefreshTileHierarchy();
        #endregion
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

}
