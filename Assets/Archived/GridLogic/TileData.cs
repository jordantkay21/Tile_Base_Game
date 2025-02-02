using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace KayosStudios.Archived
{
    [CreateAssetMenu(fileName = "NewTileType", menuName = "Tile Types/New Tile Type")]
    public class TileData : ScriptableObject
    {
        public TileType Type;
        public TileTier Tier;
        public Mesh Mesh;
        public Material Material;
        public GameObject TileObj;

        [ShowInInspector, ShowIf("Tier", TileTier.Path)]
        [TitleGroup("Rotation Needs", "Rotations needed to connect open sides together")]
        [DictionaryDrawerSettings(KeyLabel = "Side", ValueLabel = "Rotations")]
        public List<RotationRequirement> rotationRequirements = new List<RotationRequirement>();

        public GameObject SpawnObj(Transform parent = null)
        {
            //Debug.Log("SpawnObj Called");
            if (TileObj == null) return null;

            GameObject spawnedObj = Instantiate(TileObj, parent, false);
            return spawnedObj;
        }
    }

    [System.Serializable]
    public class RotationRequirement
    {
        public SideType side;
        public int[] allowedRotations;
    }

    [System.Serializable]
    public enum SideType
    {
        Top,
        Right,
        Bottom,
        Left,
        TopRight,
        BottomRight,
        BottomLeft,
        TopLeft
    }

    [System.Serializable]
    public class SideDirection
    {
        public SideType type;
        public Vector2Int direction;
    }
}