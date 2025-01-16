using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [ShowInInspector]
    public Dictionary<CardData, TileData> tileDictionary = new Dictionary<CardData, TileData>();

    public TileData GetTile(CardData card)
    {
        if (tileDictionary.ContainsKey(card)) return tileDictionary[card];
        else return null;
    }
}
