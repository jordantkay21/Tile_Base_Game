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
        // Get current dimensions
        int currentRows = gridTiles.GetLength(0);
        int currentColumns = gridTiles.GetLength(1);

        // New dimensions for the outer ring
        int newRows = currentRows + 2;
        int newColumns = currentColumns + 2;

        // Create a new grid with updated dimensions
        GameObject[,] newGridTiles = new GameObject[newRows, newColumns];

        // Offset for translating old coordinates to new grid
        int rowOffset = 1;
        int columnOffset = 1;

        // Reference to the grid parent
        Transform gridParent = transform.Find("Grid");

        if (gridParent == null)
        {
            Debug.LogError("Grid parent not found! Ensure the grid structure matches the initial setup.");
            return;
        }

        // Copy existing tiles to the new grid with offset
        for (int row = 0; row < currentRows; row++)
        {
            for (int column = 0; column < currentColumns; column++)
            {
                newGridTiles[row + rowOffset, column + columnOffset] = gridTiles[row, column];
            }
        }

        // Create new rows and add tiles for the outer ring
        for (int newRow = 0; newRow < newRows; newRow++)
        {
            //Calculate the grid-relative row coordinate
            int gridRow = newRow - rowOffset;

            //Find or create the correct row parent baed on grid-relative row coordinate
            Transform rowParent = gridParent.Find($"Row_{gridRow}");

            if (rowParent == null)
            {
                rowParent = new GameObject($"Row_{gridRow}").transform;
                rowParent.parent = gridParent;
            }

            for (int newColumn = 0; newColumn < newColumns; newColumn++)
            {
                if (newGridTiles[newRow, newColumn] == null) // Only spawn new tiles
                {
                    // Calculate the grid-relative position
                    int gridColumn = newColumn - columnOffset;

                    Vector3 position = new Vector3(gridColumn * tileSize, 0, gridRow * tileSize);
                    GameObject tileGO = Instantiate(tilePrefab, position, Quaternion.identity, rowParent);

                    tileGO.name = $"Tile({gridRow},{gridColumn})";

                    TileHandler tHandler = tileGO.GetComponent<TileHandler>();
                    tHandler.position = new Vector2Int(gridRow, gridColumn);
                    tHandler.CurrentTile = GameManager.Instance.GetTileData(TileType.Locked);

                    newGridTiles[newRow, newColumn] = tileGO;
                }
            }
        }

        // Update the grid reference
        gridTiles = newGridTiles;

        // Cache neighbors for all tiles
        foreach (var tile in gridTiles)
        {
            if (tile != null)
            {
                TileHandler tHandler = tile.GetComponent<TileHandler>();
                tHandler.CacheNeighbors();
            }
        }

        Debug.Log("Outer ring of tiles added successfully with proper coordinates.");
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

        if (gridTiles[row, column] == null)
        {
            //Debug.LogWarning($"No tile exists at position {tilePos}");
            return null;
        }

        return gridTiles[row, column].GetComponent<TileHandler>();
    }
}
