using UnityEngine;

[CreateAssetMenu(fileName = "NewTileType", menuName = "Tile Types/New Tile Type")]
public class TileData : ScriptableObject
{
    public TileType Type;
    public Mesh Mesh;
    public Material Material;
    public GameObject TileObj;

    public GameObject SpawnObj(Vector3 position, Transform parent = null)
    {
        Debug.Log("SpawnObj Called");
        if (TileObj == null) return null;

        GameObject spawnedObj = Instantiate(TileObj,parent, false);
        return spawnedObj;
    }
}
