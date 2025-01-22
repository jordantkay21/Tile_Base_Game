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

    [ShowInInspector]
    public Dictionary<Side, TileSide> sides;

    [Header("Debugging Menu")]
    [ReadOnly, SerializeField] TileData _currentTile;
    [ReadOnly, SerializeField] CardData _attachedTileCard;
    [ReadOnly, SerializeField] CardData _attachedStructureCard;

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

    public void SyncSideData()
    {
        //Initialize the sides dictionary
        sides = new Dictionary<Side, TileSide>();

        //Loop through each Side in the enum
        foreach (Side side in Enum.GetValues(typeof(Side)))
        {
            TileSide tileSide = new TileSide();

            //Assign the TileSide based on the currentTile's corresponding side
            switch (side)
            {
                case Side.Top:
                    tileSide = _currentTile.TopSide;
                    break;
                case Side.Right:
                    tileSide = _currentTile.RightSide;
                    break;
                case Side.Bottom:
                    tileSide = _currentTile.BottomSide;
                    break;
                case Side.Left:
                    tileSide = _currentTile.LeftSide;
                    break;
            }

            if (tileSide != null)
            {
                tileSide.Side = side; // Ensure the Side enum is set correctly
                sides.Add(side, tileSide);
            }
        }


        Debug.Log("Sides synced with currentTile");
    }


    [Button("Update Visuals")]
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
}
