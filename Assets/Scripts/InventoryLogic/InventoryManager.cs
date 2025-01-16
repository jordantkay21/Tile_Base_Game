using UnityEngine;

public enum CardType
{
    Mountain,
    Forest,
    Grassfield,
    Fertailized,
    Developed,
    Path_Straight,
    Path_Turn,
    Path_Tee,
    Path_Cross
}
public class CardData
{
    public string Name;
    public CardType Type;
    public Sprite Icon;
}

public class InventoryManager : MonoBehaviour
{

    public CardData BaseCard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
