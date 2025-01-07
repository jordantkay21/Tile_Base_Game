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
    

    [Header("ICONS")]
    public Transform baseIcon;

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
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, rowContainer.transform);

                tile.name = $"Tile({row},{column})";
                gridTiles[column, row] = tile;
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

        baseIcon.position = centerTile.transform.position;
        baseIcon.gameObject.SetActive(true);
    }

    [Button("Clear Grid")]
    public void ClearGrid()
    {
        if (transform.childCount > 0)
        {
            baseIcon.gameObject.SetActive(false);
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
    #endregion
}
