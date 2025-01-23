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
    public TileSide[] sides = new TileSide[4]; 

    [Header("Debugging Menu")]
    [ReadOnly, SerializeField] TileData _currentTile;
    [ReadOnly, SerializeField] CardData _attachedTileCard;
    [ReadOnly, SerializeField] CardData _attachedStructureCard;

    [Button("Sync Side Data")]
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



    #region Tile Visual Logic
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
    #endregion
}
