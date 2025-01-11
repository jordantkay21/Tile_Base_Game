using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    [Tooltip("Card Name")]
    public string name;

    [Tooltip("Card Icon")]
    public Sprite icon;

    [Tooltip("Tile Type Card is Linked to")]
    public Tile.TileType associatedTileType;
}

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    [Tooltip("All Available Cards")]
    [SerializeField] List<Card> cardPool = new List<Card>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Button("Draw Card")]
    public Card DrawCard()
    {
        if (!ResourceManager.Instance.SpendResource(ResourceManager.ResourceType.Coin, 10))
        {
            Debug.Log("Not enough coins to draw a card!");
            return null;
        }

        //Select a random card from the pool
        int randomIndex = Random.Range(0, cardPool.Count);
        Card drawnCard = cardPool[randomIndex];

        //Add to player's inventory
        InventoryManager.Instance.AddCard(drawnCard);

        //Return the drawn card (optional, for further processing)
        return drawnCard; 

    }
}
