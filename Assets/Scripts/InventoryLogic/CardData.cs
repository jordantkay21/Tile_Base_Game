using UnityEngine;

[CreateAssetMenu(fileName = "NewTileCard", menuName = "Card Types/New Tile Card")]
public class CardData : ScriptableObject
{
    public CardType Type;
    public Sprite Icon;
}
