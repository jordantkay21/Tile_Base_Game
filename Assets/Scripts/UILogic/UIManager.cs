using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [Header("Dynamic UI Elements")]
    public ConfirmationWindow confirmWindow;
    public TileCardPanel tileCardPanel;
    public TileMenu tileMenu;

    [Header("Tile Selection Logic")]
    public Transform hoverIndicator;
    public Transform selectionIndicator;
    

    private void OnEnable()
    {
        GameManager.OnTileHovered += HighlightHoverTile;
        GameManager.OnTileSelected += HighlightSelectedTile;
        GameManager.OnTileDefined += DeselectTile;
        GameManager.OnPathSpawned += OpenTileMenu;
    }

    private void OpenTileMenu(TileHandler tile, TileMenuType menu)
    {
        switch (menu)
        {
            case TileMenuType.Rotate:
                tileMenu.ShowRotateMenu(tile);
                break;
            default:
                break;
        }
    }

    #region Tile Selection Logic

    private void OpenUIMenu(GameObject tile)
    {
        TileHandler tHandler = tile.GetComponent<TileHandler>();
        //Debug.Log($"Selected tile: {tile.name} \nTile Type {tHandler.CurrentTile.Type}");

        switch (tHandler.CurrentTile.Type)
        {
            case TileType.Locked:
                confirmWindow.ShowConfirmation(
                "Do you wish to unlock the selected tile?",
            () => ConfirmTileSelection(tHandler, TileType.Undefined),
            () => DeselectTile()
            ) ;
                break;
            case TileType.Undefined:
                tileCardPanel.ShowResourceCards();
                break;
            case TileType.Grassfield:
                tileCardPanel.ShowFoundationCards();
                break;
            case TileType.Developed:
                tileCardPanel.ShowStructureDevelopCards();
                break;
            case TileType.Fertalized:
                tileCardPanel.ShowStructureFertalizeCards();
                break;
            default:
                break;
        }
    }

    public void ConfirmTileSelection(TileHandler tile, TileType newType)
    {
        tile.CurrentTile = GameManager.Instance.GetTileData(newType);
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
        //Debug.Log("Deselecting Tile");
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
