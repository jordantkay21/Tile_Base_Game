using System;
using UnityEngine;
using UnityEngine.UI;

public class TileCardPanel : MonoBehaviour
{
    public GameObject panel;
    public GameObject resourceCards;
    public GameObject foundationCards;

    public Button MountainButton;
    public Button ForestButton;
    public Button GrassButton;
    public Button DevelopedButton;
    public Button FertalizedButton;
    public Button StraightButton;
    public Button TurnButton;
    public Button TeeButton;
    public Button CrossButton;

    private Action onCardSelect;

    public void ShowResourceCards()
    {
        panel.SetActive(true);
        resourceCards.SetActive(true);
    }

    public void ShowFoundationCards()
    {
        panel.SetActive(true);
        foundationCards.SetActive(true);
    }

    public void HidePanel()
    {
        foundationCards.SetActive(false);
        resourceCards.SetActive(false);
        panel.SetActive(false);
    }
}
