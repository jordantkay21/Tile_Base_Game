using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour
{
    [ShowInInspector]
    public TileData CurrentTile
    {
        get 
        { 
            return _currentTile; 
        }

        set
        {
            _currentTile = value;
            SetTileVisuals();
            SyncSideData();
        }
    }
    [ShowInInspector]
    public CardData AttachedCard
    {
        get
        {
            return _attachedTileCard;
        }


        set
        {
            switch (value.cardType)
            {
                case CardType.TileCard:
                    _attachedTileCard = value;
                    CurrentTile = _attachedTileCard.tileData;
                    break;
                case CardType.StructureCard:
                    _attachedStructureCard = value;
                    SpawnStructurePrefab();
                    break;
                default:
                    break;
            }

        }
    }

    [Header("Tile Components")]
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;
    public MeshRenderer meshRenderer;
    public GameObject tileObj;

    [Header("Tile Data")]
    public Vector2Int position;
    [ReadOnly, SerializeField] TileData _currentTile;
    [ReadOnly, SerializeField] CardData _attachedTileCard;
    [ReadOnly, SerializeField] CardData _attachedStructureCard;

    [Header("Neighbor Tiles")]
    public TileHandler[] neighborTiles = new TileHandler[4];
    public TileHandler TopNeighbor;
    public TileHandler RightNeighbor;
    public TileHandler BottomNeighbor;
    public TileHandler LeftNeighbor;

    [Header("Side Detection Logic")]
    public TileSide[] sides = new TileSide[4];
    public Vector2Int[] sideDirections = new Vector2Int[]
    {
    new Vector2Int(1, 0),  // Top direction
    new Vector2Int(0, 1),  // Right direction
    new Vector2Int(-1, 0), // Bottom direction
    new Vector2Int(0, -1)  // Left direction
    };

    [ShowInInspector]
    public Dictionary<Vector2Int, TileSide> sideDirectionMap;


    private void OnEnable()
    {
        //Initialize the dictionary
        sideDirectionMap = new Dictionary<Vector2Int, TileSide>();

        //Populate the dictionary by matching elements
        for (int i = 0; i < sides.Length && i < sideDirections.Length; i++)
        {
            if (sides[i] != null)
            {
                sideDirectionMap[sideDirections[i]] = sides[i];
            }
        }
    }

    public void ReconfigureSideDirectionMap()
    {
        sideDirectionMap.Clear(); // Clear the current map

        // Rebuild the map with updated side directions
        for (int i = 0; i < sides.Length && i < sideDirections.Length; i++)
        {
            if (sides[i] != null)
            {
                sideDirectionMap[sideDirections[i]] = sides[i];
            }
        }
    }

    [Button("Detect Neighbors")]
    public void DetectNeighbors()
    {
        //Iterate through each side and determine if a path is present

        foreach (KeyValuePair<Vector2Int, TileSide> entry in sideDirectionMap)
        {
            Vector2Int direction = entry.Key;     //The direction vector
            TileSide side = entry.Value;              //The current side

            //Get the neighboring tile
            TileHandler neighbor = GridManager.Instance.GetTile(position + direction);

            if (neighbor != null && neighbor.CurrentTile.Tier == TileTier.Path)
            {
                Debug.Log($"Neighbor on {side} is a path tile: {neighbor.name}");
                side.SideDetection(this, out bool isMatching);

                if (isMatching)
                {
                    Debug.Log($"Tile Sides are matching.");
                }
                else
                {
                    Debug.Log("Tile Sides are not matching.");
                    RotateTile(90);
                }
            }
        }

    }

    public void RotateTile(int degrees)
    {
        transform.Rotate(0, degrees, 0); // Rotates the GameObject 90 degrees around the Y-axis

        RotateSideDirections(degrees); //Update side directions
        ReconfigureSideDirectionMap();
    }

    private void RotateSideDirections(int degrees)
    {
        // Number of 90-degree steps
        int steps = (degrees / 90) % 4;

        //Rotate directions
        for (int i = 0; i < steps; i++)
        {
            for (int j = 0; j < sideDirections.Length; j++)
            {
                Vector2Int dir = sideDirections[j];
                //Rotate 90 degrees clockwise
                sideDirections[j] = new Vector2Int(-dir.y, dir.x);
            }
        }
    }
    public void SyncSideData()
    {
        if (_currentTile.Tier != TileTier.Path) return;

        switch (_currentTile.Type)
        {
            case TileType.Path_Straight:
                SetSide(false, true, false, true);
                break;
            case TileType.Path_Turn:
                SetSide(false, false, true, true);
                break;
            case TileType.Path_Tee:
                SetSide(false, true, true, true);
                break;
            case TileType.Path_Cross:
                SetSide(true, true, true, true);
                break;
            default:
                break;
        }
    }

    private void SetSide(bool topOpen, bool rightOpen, bool bottomOpen, bool leftOpen)
    {
        sides[0].isOpen = topOpen;
        sides[1].isOpen = rightOpen;
        sides[2].isOpen = bottomOpen;
        sides[3].isOpen = leftOpen;
    }


    public CardData GetCard(CardType cardType)
    {
        switch (cardType)
        {
            case CardType.TileCard:
                return _attachedTileCard;
            case CardType.StructureCard:
                return _attachedStructureCard;
        }
        return null;
    }



    #region Tile Data Sync Logic
    private void SetTileVisuals()
    {
        meshFilter.mesh = _currentTile.Mesh;
        meshCollider.sharedMesh = _currentTile.Mesh;
        meshRenderer.material = _currentTile.Material;
        if (tileObj != null) Destroy(tileObj);
        tileObj = _currentTile.SpawnObj(transform);
    }

    private void SpawnStructurePrefab()
    {
        if (tileObj != null) Destroy(tileObj);
        tileObj = _attachedStructureCard.SpawnObj(transform);
    }
    #endregion
}
