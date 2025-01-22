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

    [ShowIf("Tier", TileTier.Path)]
    public TileSide TopSide = new TileSide();
    [ShowIf("Tier", TileTier.Path)]
    public TileSide RightSide = new TileSide();
    [ShowIf("Tier", TileTier.Path)]
    public TileSide BottomSide = new TileSide();
    [ShowIf("Tier", TileTier.Path)]
    public TileSide LeftSide = new TileSide();

    public GameObject SpawnObj(Transform parent = null)
    {
        Debug.Log("SpawnObj Called");
        if (TileObj == null) return null;

        GameObject spawnedObj = Instantiate(TileObj,parent, false);
        return spawnedObj;
    }
}
