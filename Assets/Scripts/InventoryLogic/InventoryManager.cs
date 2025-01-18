using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CardType
{
    TileCard,
    StructureCard
}



public class InventoryManager : MonoBehaviour
{
    [ShowInInspector]
    public Dictionary<CardType, CardData> cardTypes = new Dictionary<CardType, CardData>();

    public List<CardData> drawnCards = new List<CardData>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
