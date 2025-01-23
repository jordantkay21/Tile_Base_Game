using UnityEngine;

public class SideData
{
    public SideType side;
    public bool isOpen;
}

[System.Serializable]
public enum SideType
{
    Top,
    Right,
    Bottom,
    Left
}

public class TileSide : MonoBehaviour
{
    public SideType side;
    public bool isOpen;
}
