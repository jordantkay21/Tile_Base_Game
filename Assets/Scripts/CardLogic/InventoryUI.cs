using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryUI : MonoBehaviour
{
    public CardMenu cardMenu;

    public void OnDrawCardButtonPressed()
    {
        Card newCard = DeckManager.Instance.DrawCard();
        cardMenu.PopulateMenu();
        cardMenu.PopulateCardData(newCard);
    }

    public void OpenCardMenu()
    {
        cardMenu.gameObject.SetActive(true);
        cardMenu.PopulateMenu();
    }

    public void CloseCardMenu()
    {
        cardMenu.gameObject.SetActive(false);
    }
}
