using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileUIManager : MonoBehaviour
{
    public TMP_Text tileTypeText;
    public GameObject tileDataPanel;
    public Button unlockButton;
    public Tile selectedTile;

    private void OnEnable()
    {
        GridManager.OnTileSelected += UpdateUI;
        GridManager.OnTileDeselected += () => tileDataPanel.SetActive(false);
    }

    private void OnDisable()
    {
        GridManager.OnTileSelected -= UpdateUI;
    }

    private void UpdateUI(GameObject tileObject)
    {
        selectedTile = tileObject.GetComponent<Tile>();
        if (selectedTile != null)
        {
            tileDataPanel.SetActive(true);
            tileTypeText.text = $"Type: {selectedTile.currentType.TileType}";
            unlockButton.interactable = selectedTile.currentType.TileType == Tile.TileType.Locked;
        }
    }

    public void UnlockSelectedTile()
    {
        if (selectedTile != null && selectedTile.currentType.TileType == Tile.TileType.Locked)
        {
            selectedTile.SetTileType(Tile.TileType.GrassPlains); // Example type
        }
    }
}
