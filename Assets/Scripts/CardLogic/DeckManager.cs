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
    [Tooltip("All Available Cards")]
    public List<Card> cardPool = new List<Card>();
    
    [Tooltip("Parent Object for Card UI Elements")]
    public Transform cardUIParent;

    [Tooltip("Prefab to represent a card in the UI")]
    public GameObject cardUIPrefab;

    [Button("Draw Card")]
    public void DrawCard()
    {
        if (ResourceManager.Instance.SpendResource(ResourceManager.ResourceType.Coin, 10))
        {
            Card drawnCard = GetRandomCard();
            DisplayCard(drawnCard);
        }
        else
        {
            Debug.Log("Not enough coins to draw a card!");
        }
    }
    private Card GetRandomCard()
    {
        int randomIndex = Random.Range(0, cardPool.Count);
        return cardPool[randomIndex];
    }

    private void DisplayCard(Card card)
    {
        GameObject cardUI = Instantiate(cardUIPrefab, cardUIParent);
        cardUI.name = card.name;

        // Update card UI with card data
        CardUI cardUIComponent = cardUI.GetComponent<CardUI>();
        if (cardUIComponent != null)
        {
            cardUIComponent.SetCard(card);
        }

        Debug.Log($"Drew card: {card.name}");
    }
}
