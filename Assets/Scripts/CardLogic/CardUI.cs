using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public TMP_Text cardNameText;
    public Image cardIconImage;

    public void SetCard(Card card)
    {
        cardNameText.text = card.name;
        cardIconImage.sprite = card.icon;
    }
}