using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    [System.Serializable]
    public enum TileType
    {
        Locked,
        GrassPlains,
        Forest,
        Mountain,
        Base,
        LumberMill,
        Mine,
        Farm,
        Settlement,
        Barracks,
        Watchtower,
        Storage,
        Market,
        Path

    }
    [System.Serializable]
    public class TileData
    {
        public TileType TileType;
        public Mesh IconMesh;
    }
    [Header("Icon Components")]
    public GameObject iconGO;
    public MeshFilter iconMeshFilter;
    public MeshCollider iconMeshCollider;

    [Header("Tile Components")]
    public MeshRenderer tileMeshRenderer;
    public Material lockedMaterial;
    public Material unlockedMaterial;

    [Header("Tile Configuration")]
    public List<TileData> tileTypes = new List<TileData>();
    public bool isLocked = false;
    public TileData currentType;

    public void UnlockTile()
    {
        isLocked = false;
        SetTileType(TileType.GrassPlains);
    }

    public void SetTileType(TileType newType)
    {
        foreach (var type in tileTypes)
        {
            if (type.TileType != newType) continue;

            if (type.TileType == TileType.Locked)
            {
                tileMeshRenderer.material = lockedMaterial;
                isLocked = true;
            }
            else tileMeshRenderer.material = unlockedMaterial;


            currentType = type;
            if (type.IconMesh != null)
            {
                SetIconMesh(type.IconMesh);
                iconGO.SetActive(true);
            }
            else
            {
                iconGO.SetActive(false);
            }
        }
    }

    public void SetIconMesh(Mesh newMesh)
    {
        iconMeshFilter.mesh = newMesh;
        iconMeshCollider.sharedMesh = newMesh;
    }
    
}

