using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KayosStudios.Archived
{
    public class CardHandler : MonoBehaviour
    {
        public CardData cardData;

        public TMP_Text nameText;
        public Image iconImage;

        [Button("UpdateCard")]
        public void UpdateCard()
        {
            nameText.text = cardData.cardName;
            iconImage.sprite = cardData.Icon;
        }

    }
}