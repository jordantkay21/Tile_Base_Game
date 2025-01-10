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
 
    public Tile selectedTile;

    [Header("Tile Panel Configuration")]
    public GameObject tileDataPanel;
    public TMP_Text tileTypeText;
    public Button unlockButton;
    public List<IconButton> iconButtons;




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
            if (selectedTile.isLocked == false)
            {
                Debug.Log($"{selectedTile} has changed to {newType}");
                selectedTile.SetTileType(newType);
            }
            else
            {
                Debug.Log("Tile must be unlocked in order to change its type!");
            }
        }
        else
        {
            Debug.Log("Selected Tile is Null");
        }

        UpdateUI(selectedTile.gameObject);
    }

    private void UpdateUI(GameObject tileObject)
    {
        Debug.Log("TilePanel UI Updated");
        selectedTile = tileObject.GetComponent<Tile>();
        if (selectedTile != null)
        {
            tileDataPanel.SetActive(true);
            tileTypeText.text = $"Type: {selectedTile.currentType.TileType}";
            unlockButton.gameObject.SetActive(selectedTile.currentType.TileType == Tile.TileType.Locked);

            foreach(IconButton icon in iconButtons)
            {
                icon.button.interactable = selectedTile.currentType.TileType != Tile.TileType.Locked;
            }
        }
    }

    public void UnlockSelectedTile()
    {
        if (selectedTile != null && selectedTile.currentType.TileType == Tile.TileType.Locked)
        {
            selectedTile.UnlockTile();
            unlockButton.interactable = false;
        }

        UpdateUI(selectedTile.gameObject);
    }
}
