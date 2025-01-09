using UnityEngine;

public class IconGridPlacer : MonoBehaviour
{
    public GameObject[] icons; // Drag and drop your icons here
    public int columns = 4;    // Number of columns in the grid
    public float spacing = 1f; // Spacing between icons
    public Vector3 startPosition = Vector3.zero; // Start position for grid

    void Start()
    {
        int rows = Mathf.CeilToInt((float)icons.Length / columns); // Calculate rows
        Vector3 currentPos = startPosition;

        for (int i = 0; i < icons.Length; i++)
        {
            int row = i / columns;
            int col = i % columns;

            // Calculate position for each icon
            currentPos = new Vector3(startPosition.x + col * spacing, startPosition.y - row * spacing, startPosition.z);
            icons[i].transform.position = currentPos;
        }
    }
}
