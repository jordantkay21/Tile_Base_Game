using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTileCard", menuName = "Card Types/New Tile Card")]
public class CardData : ScriptableObject
{
    [Title("Card Details")]
    [EnumButtons]
    public CardType cardType;    
    public string cardName;
    public Sprite Icon;

    [ShowIf("cardType", CardType.TileCard)]
    public TileType tileType;
    [ShowIf("cardType", CardType.TileCard)]
    public TileTier tileTier;

    
    [ShowIf("cardType", CardType.StructureCard)]
    public StructureType structureType;
    
}
