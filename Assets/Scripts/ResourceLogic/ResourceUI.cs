using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    public TMP_Text woodText;
    public TMP_Text stoneText;
    public TMP_Text coinText;

    private void OnEnable()
    {
        ResourceManager.Instance.OnResourceUpdated += UpdateResourceUI;
    }

    private void OnDisable()
    {
        ResourceManager.Instance.OnResourceUpdated -= UpdateResourceUI;
    }

    private void UpdateResourceUI(int wood, int stone, int coin)
    {
        woodText.text = $"{wood}";
        stoneText.text = $"{stone}";
        coinText.text = $"{coin}";
    }
}
