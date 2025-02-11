using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DebugSettings", menuName = "Kayos/Debugging/DebugSettings", order = 2)]
public class DebugSettings : ScriptableObject
{
    [Header("Debug Tag Settings")]
    public List<DebugTag> debugTags = new List<DebugTag>();

    [Header("Debug Level Settings")]
    public Dictionary<DebugLevel, bool> debugLevels = new Dictionary<DebugLevel, bool>()
    {
        { DebugLevel.Verbose, true},
        {DebugLevel.Info, true },
        {DebugLevel.Warning, true },
        {DebugLevel.Error, true }
    };

    private void OnEnable()
    {
        AutoRegisterTags();
    }
    private void AutoRegisterTags()
    {
        debugTags.Clear();
        DebugTag[] allTags = Resources.LoadAll<DebugTag>("");
        foreach (DebugTag tag in allTags)
        {
            if(!debugTags.Any(t => t.TagName == tag.TagName))
            {
                debugTags.Add(tag);
            }
        }
    }

    public bool IsTagEnabled(string tag)
    {
        DebugTag debugTag = debugTags.Find(t => t.TagName == tag);
        return debugTag != null && debugTag.IsEnabled;
    }

    public bool IsDebugLevelEnabled(DebugLevel level)
    {
        return debugLevels.ContainsKey(level) && debugLevels[level];
    }

    public void EnableTag(string tag, bool state)
    {
        DebugTag debugTag = debugTags.Find(t => t.TagName == tag);
        if(debugTag != null)
        {
            debugTag.SetEnabled(state);
        }
    }

    public void SetDebugLevel(DebugLevel level, bool state)
    {
        if (debugLevels.ContainsKey(level))
        {
            debugLevels[level] = state;
        }
    }

    public Color GetTagColor(string tag)
    {
        DebugTag debugTag = debugTags.Find(t => t.TagName == tag);
        Color tagColor = Color.white;

        if (debugTag != null)
        {
            tagColor = debugTag.TagColor;
        }

        return tagColor;
    }
}
