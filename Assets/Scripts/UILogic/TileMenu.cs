using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

public enum TileMenuType
{
    Rotate,
}

public class TileMenu : MonoBehaviour
{
    public GameObject tileMenuCanvas;

    [FoldoutGroup("Rotate Menu")]
    public GameObject rotateMenu;
    [FoldoutGroup("Rotate Menu")]
    public TileHandler rotateTile;
    [FoldoutGroup("Rotate Menu")]
    public Button rm_accept;
    [FoldoutGroup("Rotate Menu")]
    public Button rm_rotate;
    public void ShowRotateMenu() => rotateMenu.SetActive(true);

    public void ShowRotateMenu(TileHandler tile)
    {
        tileMenuCanvas.transform.position = tile.gameObject.transform.position;
        rotateTile = tile;
        rotateMenu.SetActive(true);

        if (rm_accept != null)
        {
            Debug.Log("Listeners Added to Accept Button");
            rm_accept.onClick.AddListener(OnRotateAcceptClick);
        }

        if (rm_rotate != null)
        {
            Debug.Log("Listeners Added to Rotate Button");
            rm_rotate.onClick.AddListener(OnRotateClick);
        }
    }

    public void OnRotateClick()
    {
        Debug.Log("Rotate Tile Button Clicked");
        rotateTile.RotateTileToNextAngle(rotateTile.DetectNeighbors());
    }
    public void OnRotateAcceptClick()
    {
        Debug.Log("Rotate Menu Accept Button Clicked");
        HideRotateMenu();
        
    }
    private void HideRotateMenu()
    {
        rm_accept.onClick.RemoveAllListeners();
        rm_rotate.onClick.RemoveAllListeners();
        rotateTile = null;

        rotateMenu.SetActive(false);
    }

}
