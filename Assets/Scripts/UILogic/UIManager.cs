using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [Header("Dynamic UI Elements")]
    public ConfirmationWindow confirmWindow;
    public TileCardPanel tileCardPanel;

    [Header("Tile Selection Logic")]
    public Transform hoverIndicator;
    public Transform selectionIndicator;
    

    private void OnEnable()
    {
        GameManager.OnTileHovered += HighlightHoverTile;
        GameManager.OnTileSelected += HighlightSelectedTile;
        GameManager.OnTileDefined += DeselectTile;
    }


    #region Tile Selection Logic

    private void OpenUIMenu(GameObject tile)
    {
        TileHandler tileData = tile.GetComponent<TileHandler>();
        Debug.Log($"Selected tile: {tile.name} \nTile Type {tileData.currentTile.Type}");

        switch (tileData.currentTile.Type)
        {
            case TileType.Locked:
                confirmWindow.ShowConfirmation(
                "Do you wish to unlock the selected tile?",
            () => ConfirmTileSelection(tileData, TileType.Undefined),
            () => DeselectTile()
            ) ;
                break;
            case TileType.Undefined:
                tileCardPanel.ShowResourceCards();
                break;
            case TileType.Grassfield:
                tileCardPanel.ShowFoundationCards();
                break;
            default:
                break;
        }
    }

    public void ConfirmTileSelection(TileHandler tile, TileType newType)
    {
        tile.DefineTileType(newType);
        confirmWindow.HideConfirmation();
        DeselectTile();
    }



    private void HighlightHoverTile(GameObject tile)
    {
        if (IsPointerOverUIElement()) return;

        if (tile != null)
        {
            hoverIndicator.position = tile.transform.position;
            hoverIndicator.gameObject.SetActive(true);
        }
        else
        {
            hoverIndicator.gameObject.SetActive(false);
        }
    }

    private void HighlightSelectedTile(GameObject tile)
    {
        if (IsPointerOverUIElement()) return;

        if (tile != null)
        {
            selectionIndicator.position = tile.transform.position;
            selectionIndicator.gameObject.SetActive(true);

            OpenUIMenu(tile);
        }
        else
        {
            DeselectTile();
        }
        
    }
    private void DeselectTile()
    {
        Debug.Log("Deselecting Tile");
        tileCardPanel.HidePanel();
        confirmWindow.HideConfirmation();
        selectionIndicator.gameObject.SetActive(false);
    }

    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    #endregion
}
