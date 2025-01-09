using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileUIManager : MonoBehaviour
{
    [System.Serializable]
    public class IconButton
    {
        public Tile.TileType type;
        public Button button;
    }

    [Header("Tile Panel Configuration")]
    public GameObject tileDataPanel;
    public TMP_Text tileTypeText;
    public Button unlockButton;

    public List<IconButton> iconButtons;
    //public Button forestButton;
    //public Button mountainButton;
    //public Button lumberMillButton;
    //public Button mineButton;
    //public Button farmButton;
    //public Button settlementButton;
    //public Button barracksButton;
    //public Button watchtowerButton;
    //public Button storageButton;
    //public Button marketButton;
    //public Button pathButton;

    

    public Tile selectedTile;


    private void Start()
    {
        foreach(IconButton icon in iconButtons)
        {
            icon.button.onClick.AddListener(() => ChangeTileType(icon.type));
            Debug.Log($"Assigned {icon.button} for type: {icon.type}");
        }
    }

    private void OnEnable()
    {
        GridManager.OnTileSelected += UpdateUI;
        GridManager.OnTileDeselected += () => tileDataPanel.SetActive(false);
    }

    private void OnDisable()
    {
        GridManager.OnTileSelected -= UpdateUI;
    }

    public void ChangeTileType(Tile.TileType newType)
    {
        if (selectedTile != null)
        {
            Debug.Log($"{selectedTile} has changed to {newType}");
            selectedTile.SetTileType(newType); 
        }
        else
        {
            Debug.Log("Selected Tile is Null");
        }
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
            unlockButton.interactable = false;
        }
    }
}
