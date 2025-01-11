using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;

public class RenderTextureConverter : MonoBehaviour
{
    public RenderTexture renderTexture; // Assign your RenderTexture here
    public string fileName = "SavedTexture.png"; // Name of the saved file

    [Button("Save Render Texture")]
    public void SaveRenderTextureToFile()
    {
        // Step 1: Activate the RenderTexture
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;

        // Step 2: Create a new Texture2D with the same dimensions as the RenderTexture
        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // Step 3: Read the pixels from the RenderTexture into the Texture2D
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        // Step 4: Reset the active RenderTexture
        RenderTexture.active = activeRenderTexture;

        // Step 5: Encode the Texture2D into PNG format
        byte[] bytes = texture2D.EncodeToPNG();

        // Step 6: Save the PNG file
        string path = Path.Combine(Application.dataPath, fileName);
        File.WriteAllBytes(path, bytes);

        Debug.Log($"RenderTexture saved as {path}");

        // Clean up
        DestroyImmediate(texture2D);
    }
}

