using System;
using UnityEngine;
using UnityEngine.UI;

namespace KayosStudios.Archived
{
    public class TileCardPanel : MonoBehaviour
    {
        public GameObject panel;
        public GameObject resourceCards;
        public GameObject foundationCards;
        public GameObject structureDevelopCards;
        public GameObject structureFertalizeCards;


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

        public void ShowStructureDevelopCards()
        {
            panel.SetActive(true);
            structureDevelopCards.SetActive(true);
        }

        public void ShowStructureFertalizeCards()
        {
            panel.SetActive(true);
            structureFertalizeCards.SetActive(true);
        }

        public void HidePanel()
        {
            foundationCards.SetActive(false);
            resourceCards.SetActive(false);
            structureFertalizeCards.SetActive(false);
            structureDevelopCards.SetActive(false);
            panel.SetActive(false);
        }


    }
}