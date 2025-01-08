using Sirenix.OdinInspector;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Generation Properties")]
    public GameObject tilePrefab;
    public int rows;
    public int columns;
    public float tileSize;
    [ShowInInspector]
    public GameObject[,] gridTiles;
    public GameObject centerTile;

    [Header("Tile Selection Properties")]
    public Camera raycastCamera;
    public LayerMask tileLayerMask;
    public Transform hoverIndicator;
    public Transform selectionIndicator;
    public GameObject hoveredTile;
    public GameObject selectedTile;
   

    private void Awake()
    {
        if (gridTiles == null)
        {
            ClearGrid();
            GenerateGrid();
        }
    }

    private void Update()
    {
        TileRaycast();
    }

    #region GridConfiguration
    [Button("Generate Grid")]
    public void GenerateGrid()
    {
        Transform Grid = new GameObject("Grid").transform;
        Grid.parent = transform;
        gridTiles = new GameObject[columns, rows];

        for (int row = 0; row < rows; row++)
        {
            GameObject rowContainer = new GameObject($"Row_{row}");
            rowContainer.transform.parent = Grid;

            for(int column = 0; column < columns; column++)
            {
                Vector3 position = new Vector3(column * tileSize, 0, row * tileSize);
                GameObject tileGO = Instantiate(tilePrefab, position, Quaternion.identity, rowContainer.transform);
                SetTileType(tileGO, Tile.TileType.Locked);

                tileGO.name = $"Tile({row},{column})";
                gridTiles[column, row] = tileGO;

            }
        }

        DetectCenterTile();
    }

    public void DetectCenterTile()
    {
        int centerRow = rows / 2;
        int centerColumn = columns / 2;

        if (rows % 2 == 0) centerRow -= 1;
        if (columns % 2 == 0) centerColumn -= 1;

        centerTile = gridTiles[centerColumn, centerRow];

        SetTileType(centerTile, Tile.TileType.Base);
    }

    [Button("Clear Grid")]
    public void ClearGrid()
    {
        if (transform.childCount > 0)
        {
            centerTile = null;
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
    #endregion

    #region Tile Selection Logic
    private void TileRaycast()
    {
        Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
        {
            GameObject hitObject = hit.collider.gameObject;

            if(hitObject != hoveredTile)
            {
                hoveredTile = hitObject;
            }

            if (Input.GetMouseButtonDown(0))
            {
                SelectTile(hitObject);
            }

        }
        else
        {
            hoveredTile = null;
        }

        HighlightHoverTile();
    }

    private void SelectTile(GameObject tile)
    {
        if(selectedTile != null)
        {
            ResetSelectedTile();
        }

        selectedTile = tile;

        HighlightSelectedTile();
    }
    private void HighlightHoverTile()
    {
        if(hoveredTile != null)
        {
            hoverIndicator.position = hoveredTile.transform.position;
            hoverIndicator.gameObject.SetActive(true);
        }
        else
        {
            hoverIndicator.gameObject.SetActive(false);
        }
    }
    private void HighlightSelectedTile()
    {
        selectionIndicator.position = selectedTile.transform.position;
        selectionIndicator.gameObject.SetActive(true);
    }
    private void ResetSelectedTile()
    {
        selectionIndicator.gameObject.SetActive(false);
    }

    private void SetTileType(GameObject tileGO, Tile.TileType newType)
    {
        Tile tile = tileGO.GetComponent<Tile>();
        tile.SetTileType(newType);
    }
    #endregion
}
