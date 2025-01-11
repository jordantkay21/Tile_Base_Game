using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardMenu : MonoBehaviour
{
    [Header("Deck Display Components")]
    [SerializeField] Transform cardContent;
    [SerializeField] GameObject cardUIPrefab;

    [Header("Card Data Components")]
    [SerializeField] Image cardIcon;
    [SerializeField] TMP_Text cardName;

    public void PopulateMenu()
    {
        //Clear exsisting cards in the UI
        foreach (Transform child in cardContent)
        {
            Destroy(child.gameObject);
        }

        List<Card> inventory = InventoryManager.Instance.GetCards();

        if(inventory.Count == 0)
        {
            Debug.Log("No cards to load");
            return;
        }

        //Add all drawn cards to the UI
        foreach (Card card in InventoryManager.Instance.GetCards())
        {
            GameObject cardGO = Instantiate(cardUIPrefab, cardContent);
            CardPrefabUI cardUI = cardGO.GetComponent<CardPrefabUI>();

            if(cardUI != null)
            {
                cardUI.SetCard(card);
            }
        }
    }

    public void PopulateCardData(Card card)
    {
        cardName.text = card.name;
        cardIcon.sprite = card.icon;
    }
}
