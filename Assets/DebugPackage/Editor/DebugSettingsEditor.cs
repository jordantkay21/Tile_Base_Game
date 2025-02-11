#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class DebugSettingsEditor : EditorWindow
{
    private DebugSettings debugSettings;
    private string newTagName = "";
    private Color newTagColor = Color.white;
    private Vector2 scrollPosition; // Stores scroll position

    [MenuItem("Tools/Debug Settings")]
    public static void ShowWindow()
    {
        GetWindow<DebugSettingsEditor>("Debug Settings");
    }

    private void OnEnable()
    {
        LoadOrCreateDebugSettings();
        DebugLogger.Initialize(debugSettings); // Auto-initialize DebugLogger
        Debug.Log("DebugLogger initialized with DebugSettings.");
    }

    private void LoadOrCreateDebugSettings()
    {
        debugSettings = Resources.Load<DebugSettings>("DebugSettings");

        if (debugSettings == null)
        {
            Debug.LogWarning("DebugSettings not found! Creating a new one...");

            string folderPath = "Assets/Resources";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                AssetDatabase.Refresh();
            }

            debugSettings = ScriptableObject.CreateInstance<DebugSettings>();
            AssetDatabase.CreateAsset(debugSettings, $"{folderPath}/DebugSettings.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("New DebugSettings asset created at: Assets/Resources/DebugSettings.asset");
        }
    }

    private void OnGUI()
    {
        if (debugSettings == null)
        {
            EditorGUILayout.LabelField("DebugSettings asset not found and could not be created.");
            return;
        }

        // Begin Scroll View
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        #region Create New Debug Tag
        EditorGUILayout.LabelField("Create New Debug Tag", EditorStyles.boldLabel);

        newTagName = EditorGUILayout.TextField("Tag Name", newTagName);
        newTagColor = EditorGUILayout.ColorField("Tag Color", newTagColor);

        EditorGUILayout.Space();

        if (GUILayout.Button("Create Debug Tag"))
        {
            CreateDebugTag(newTagName, newTagColor);
        }
        #endregion

        EditorGUILayout.Space();

        #region Debug Tag Settings
        EditorGUILayout.LabelField("Debug Tag Settings", EditorStyles.boldLabel);

        for (int i = debugSettings.debugTags.Count - 1; i >= 0; i--)
        {
            var tag = debugSettings.debugTags[i];

            EditorGUILayout.BeginHorizontal();

            bool newState = EditorGUILayout.Toggle(tag.IsEnabled, GUILayout.Width(20));
            EditorGUILayout.LabelField(tag.TagName, GUILayout.Width(150));

            if (newState != tag.IsEnabled)
            {
                tag.SetEnabled(newState);
                EditorUtility.SetDirty(tag);
            }

            Color newColor = EditorGUILayout.ColorField(tag.TagColor);
            if (newColor != tag.TagColor)
            {
                SerializedObject serializedTag = new SerializedObject(tag);
                serializedTag.FindProperty("tagColor").colorValue = newColor;
                serializedTag.ApplyModifiedProperties();
                EditorUtility.SetDirty(tag);
            }

            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                DeleteDebugTag(tag);
            }

            EditorGUILayout.EndHorizontal();
        }
        #endregion

        EditorGUILayout.Space();

        #region Debug Level Settings
        EditorGUILayout.LabelField("Debug Level Settings", EditorStyles.boldLabel);

        foreach (DebugLevel level in System.Enum.GetValues(typeof(DebugLevel)))
        {
            EditorGUILayout.BeginHorizontal();

            bool isEnabled = debugSettings.IsDebugLevelEnabled(level);

            bool newLevelState = EditorGUILayout.Toggle(isEnabled, GUILayout.Width(20));
            EditorGUILayout.LabelField(level.ToString(), GUILayout.Width(150));

            if (newLevelState != isEnabled)
            {
                debugSettings.SetDebugLevel(level, newLevelState);
            }

            EditorGUILayout.EndHorizontal();
        }
        #endregion

        EditorGUILayout.Space();

        #region Buttons
        if (GUILayout.Button("Save Settings"))
        {
            EditorUtility.SetDirty(debugSettings);
            AssetDatabase.SaveAssets();
        }
        #endregion

        // End Scroll View
        EditorGUILayout.EndScrollView();
    }

    private void CreateDebugTag(string tagName, Color tagColor)
    {
        if (string.IsNullOrWhiteSpace(tagName))
        {
            Debug.LogError("Tag name cannot be empty!");
            return;
        }

        string folderPath = "Assets/Resources/DebugTags";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
        }

        DebugTag newTag = ScriptableObject.CreateInstance<DebugTag>();
        SerializedObject serializedTag = new SerializedObject(newTag);
        serializedTag.FindProperty("tagName").stringValue = tagName;
        serializedTag.FindProperty("tagColor").colorValue = tagColor;
        serializedTag.ApplyModifiedProperties();

        string assetPath = $"{folderPath}/{tagName}.asset";
        AssetDatabase.CreateAsset(newTag, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        debugSettings.debugTags.Add(newTag);
        EditorUtility.SetDirty(debugSettings);

        Debug.Log($"Debug Tag '{tagName}' created successfully with color {tagColor}!");

        // Reset Input Fields
        newTagName = "";               // Clear the text field
        newTagColor = Color.white;      // Reset color to white

        //Force Unity to update the UI
        GUI.FocusControl(null);  // Clears focus from the text field
        Repaint();  // Forces UI refresh in the Editor window
    }

    private void DeleteDebugTag(DebugTag tag)
    {
        if (tag == null) return;

        string assetPath = AssetDatabase.GetAssetPath(tag);

        if (!string.IsNullOrEmpty(assetPath))
        {
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        debugSettings.debugTags.Remove(tag);
        EditorUtility.SetDirty(debugSettings);
        Debug.Log($"Debug Tag '{tag.TagName}' deleted successfully.");
    }
}
#endif
