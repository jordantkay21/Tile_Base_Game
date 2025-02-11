using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugTag", menuName = "Kayos/Debugging/DebugTag", order = 1)]
public class DebugTag : ScriptableObject
{
    [SerializeField] string tagName;
    [SerializeField] bool isEnabled = true;
    [SerializeField] Color tagColor;

    public string TagName => tagName;
    public bool IsEnabled => isEnabled;
    public Color TagColor => tagColor;

    public void SetEnabled(bool state) => isEnabled = state;
}
