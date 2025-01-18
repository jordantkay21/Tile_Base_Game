using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTileType", menuName = "Tile Types/New Tile Type")]
public class TileData : ScriptableObject
{
    public TileType Type;
    public TileTier Tier;
    public Mesh Mesh;
    public Material Material;
    public GameObject TileObj;

    public GameObject SpawnObj(Transform parent = null)
    {
        Debug.Log("SpawnObj Called");
        if (TileObj == null) return null;

        GameObject spawnedObj = Instantiate(TileObj,parent, false);
        return spawnedObj;
    }
}
