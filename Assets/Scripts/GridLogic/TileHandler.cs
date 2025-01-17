using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour
{
    public TileData[] tileTypes = new TileData[9];

    public TileData currentTile;

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
        currentTile = GetTileData(newType);
        SetTileVisuals();
    }

    private void SetTileVisuals()
    {
        Debug.Log("Setting Tile Visuals");
        meshFilter.mesh = currentTile.Mesh;
        meshCollider.sharedMesh = currentTile.Mesh;
        meshRenderer.material = currentTile.Material;
        if (tileObj != null) Destroy(tileObj);
        tileObj = currentTile.SpawnObj(transform.position, transform);
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
