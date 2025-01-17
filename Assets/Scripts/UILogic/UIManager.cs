using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Dynamic UI Elements")]
    public ConfirmationWindow confirmWindow;

    [Header("Tile Selection Logic")]
    public Transform hoverIndicator;
    public Transform selectionIndicator;

    private void OnEnable()
    {
        GameManager.OnTileHovered += HighlightHoverTile;
        GameManager.OnTileSelected += HighlightSelectedTile;
    }


    #region Tile Selection Logic

    private void OpenUIMenu(GameObject tile)
    {
        TileHandler tileData = tile.GetComponent<TileHandler>();

        if(tileData.currentTile.Type == TileType.Locked)
        {
            Debug.Log($"Selected tile: {tile.name} \nTile Type {tileData.currentTile.Type}");
            confirmWindow.ShowConfirmation(
                "Do you wish to unlock the selected tile?",
            () => ConfirmTileSelection(tileData, TileType.Undefined),
            () => confirmWindow.HideConfirmation()
            ) ;
        }
    }

    private void ConfirmTileSelection(TileHandler tile, TileType newType)
    {
        tile.DefineTileType(newType);
        confirmWindow.HideConfirmation();
        DeselectTile();
    }

    private void HighlightHoverTile(GameObject tile)
    {
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
        selectionIndicator.gameObject.SetActive(false);
    }

    #endregion
}
