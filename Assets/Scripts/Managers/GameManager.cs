using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Tile Selection Properties")]
    public Camera raycastCamera;
    public LayerMask tileLayerMask;
    public GameObject hoveredTile;
    public GameObject selectedTile;

    public static event Action<GameObject> OnTileSelected;
    public static event Action<GameObject> OnTileHovered;
    public static event Action OnTileDefined;

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

    private void OnEnable()
    {
        InputManager.OnHover += HandleHover;
        InputManager.OnSelect += HandleSelect;

    }

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
            OnTileHovered?.Invoke(hoveredTile);
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

    #region Tile Selection Logic

    private void SelectTile(GameObject tile)
    {
        if(selectedTile != null)
        {
            DeselectTile();
        }

        selectedTile = tile;

        OnTileSelected.Invoke(tile);
    }

    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void DeselectTile()
    {
        if (selectedTile != null)
        {
            selectedTile = null;
        }
            OnTileSelected?.Invoke(null);
    }

    #endregion

    public void DefineTile(TileData newTile)
    {
        Debug.Log($"{newTile.Type} was clicked");
        if(selectedTile != null)
        {
            TileHandler tileData = selectedTile.GetComponent <TileHandler>();
            tileData.DefineTileType(newTile.Type);
            OnTileDefined?.Invoke();
        }
        else
        {
            Debug.Log("SelectedTile is NULL");
        }

    }
}
