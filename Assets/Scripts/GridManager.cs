using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public static event Action<GameObject> OnTileSelected;
    public static event Action OnTileDeselected; 
   

    private void Awake()
    {
        if (gridTiles == null)
        {
            ClearGrid();
            GenerateGrid();
        }
    }

    private void OnEnable()
    {
        InputManager.OnHover += HandleHover;
        InputManager.OnSelect += HandleSelect;
    }

    private void OnDisable()
    {
        InputManager.OnHover -= HandleHover;
        InputManager.OnSelect -= HandleSelect;
    }

    #region Inputs
    private void HandleHover(Vector2 screenPos)
    {
        Ray ray = raycastCamera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, tileLayerMask))
        {
            hoveredTile = hit.collider.gameObject;
        }
        else
        {
            hoveredTile = null;
        }

            HighlightHoverTile();
    }

    private void HandleSelect()
    {
        if (IsPointerOverUIElement()) return;

        if (hoveredTile != null)
        {
            SelectTile(hoveredTile);
        }
        else
        {
            DeselectTile();
        }
    }

    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    #endregion

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
    private void SelectTile(GameObject tile)
    {
        if(selectedTile != null)
        {
            ResetSelectedTile();
        }

        selectedTile = tile;

        HighlightSelectedTile();

        //Notify the UI
        OnTileSelected?.Invoke(tile);
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
    private void DeselectTile()
    {
        if (selectedTile != null)
        {
            ResetSelectedTile();
            selectedTile = null;

            // Notify UI of deselection
            OnTileDeselected?.Invoke();
        }
    }

    private void SetTileType(GameObject tileGO, Tile.TileType newType)
    {
        Tile tile = tileGO.GetComponent<Tile>();
        tile.SetTileType(newType);
    }

    public void ChangeTileType(Tile.TileType newType)
    {
        if(selectedTile != null)
        {
            Tile tile = selectedTile.GetComponent<Tile>();

            tile.SetTileType(newType);
        }
    }
    #endregion


}
