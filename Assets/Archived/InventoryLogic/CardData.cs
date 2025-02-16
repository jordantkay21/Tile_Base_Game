using Sirenix.OdinInspector;
using UnityEngine;

namespace KayosStudios.Archived
{
    [CreateAssetMenu(fileName = "NewTileCard", menuName = "Card Types/New Tile Card")]
    public class CardData : ScriptableObject
    {
        #region Basic Card Settings
        [FoldoutGroup("Tile Card Settings")]
        [EnumButtons]
        public CardType cardType;
        [FoldoutGroup("Tile Card Settings")]
        public string cardName;
        [FoldoutGroup("Tile Card Settings")]
        public Sprite Icon;
        #endregion

        #region Tile Card Settings
        [ShowIf("cardType", CardType.TileCard)]
        public TileData tileData;

        #endregion

        #region Structure Card Settings
        [ShowIf("cardType", CardType.StructureCard)]
        [BoxGroup("Structure Card Settings")]
        public StructureType structureType;
        [ShowIf("cardType", CardType.StructureCard)]
        [BoxGroup("Structure Card Settings")]
        public GameObject structureObj;
        #endregion

        public GameObject SpawnObj(Transform parent = null)
        {
            //Debug.Log("SpawnObj Called");
            if (structureObj == null) return null;

            GameObject spawnedObj = Instantiate(structureObj, parent, false);
            return spawnedObj;
        }

    }
}
