using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kayos.Tools.Debugger
{
    public enum DebugLevel
    {
        Verbose,
        Info,
        Warning,
        Error
    }

    public static class DebugLogger
    {
        private static DebugSettings debugSettings;

        public static void Initialize(DebugSettings settings)
        {
            debugSettings = settings;
        }

        public static void Log(string tag, string message, DebugLevel level = DebugLevel.Info)
        {
            if (debugSettings == null || debugSettings.IsTagEnabled(tag) && debugSettings.IsDebugLevelEnabled(level))
            {
                Color tagColor = debugSettings.GetTagColor(tag);
                string tagColorHex = "#" + ColorUtility.ToHtmlStringRGB(tagColor); //Convert to Hex string

                switch (level)
                {
                    case DebugLevel.Warning:
                        Debug.LogWarning($"[<color={tagColorHex}>{tag}</color>]<color=#fda010>[Warning] {message}</color>");
                        break;
                    case DebugLevel.Error:
                        Debug.LogError($"[<color={tagColorHex}>{tag}</color>]<color=#FD2A10>[Error] {message}</color>");
                        break;
                    case DebugLevel.Verbose:
                        Debug.Log($"[<color={tagColorHex}>{tag}</color>][Verbose] {message}");
                        break;
                    default:
                        Debug.Log($"[<color={tagColorHex}>{tag}</color>]<color=#10E3FD>[Info] {message}</color>");
                        break;
                }
            }
        }

        public static void ToggleTag(string tag, bool state)
        {
            if (debugSettings != null)
            {
                debugSettings.EnableTag(tag, state);
            }
        }
    }
}