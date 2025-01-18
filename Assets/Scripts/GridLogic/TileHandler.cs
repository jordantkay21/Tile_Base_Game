using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour
{
    public TileData[] tileTypes = new TileData[9];

    public TileData currentTile;
    public CardData attachedCard;

    [Header("Tile Components")]
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;
    public MeshRenderer meshRenderer;
    public GameObject tileObj;

    private void Awake()
    {
        currentTile = GetTileData(TileType.Locked);
        SetTileVisuals();
    }

    public void DefineTileType(TileType newType)
    {
        Debug.Log($"Defining {transform.name} to {newType}");
        currentTile = GetTileData(newType);

        if (currentTile != null)
            SetTileVisuals();
        else
            Debug.Log($"{name}'s currentTile is null");
    }

    private void SetTileVisuals()
    {
        meshFilter.mesh = currentTile.Mesh;
        meshCollider.sharedMesh = currentTile.Mesh;
        meshRenderer.material = currentTile.Material;
        if (tileObj != null) Destroy(tileObj);
        tileObj = currentTile.SpawnObj(transform);
    }

    public void SetCard(CardData card)
    {
        attachedCard = card;
    }

    private TileData GetTileData(TileType tileType)
    {
        foreach (TileData type in tileTypes)
        {
            if (type.Type == tileType) 
                return type;
        }

        return null;    
    }
}
