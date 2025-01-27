using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour
{
    #region Properties
    [ShowInInspector]
    public TileData CurrentTile
    {
        get
        {
            return _currentTile;
        }

        set
        {
            _currentTile = value;
            SetTileVisuals();

            if (value.Tier == TileTier.Path)
            {
                RotateTileToNextAngle(DetectNeighbors());
                if (GridManager.Instance.gridInitilized)
                    GameManager.Instance.PathSpawn(this);
            }
        }
    }
    [ShowInInspector]
    public CardData AttachedCard
    {
        get
        {
            return _attachedTileCard;
        }


        set
        {
            switch (value.cardType)
            {
                case CardType.TileCard:
                    _attachedTileCard = value;
                    CurrentTile = _attachedTileCard.tileData;
                    break;
                case CardType.StructureCard:
                    _attachedStructureCard = value;
                    SpawnStructurePrefab();
                    break;
                default:
                    break;
            }

        }
    }
    #endregion

    #region Variables
    [Header("Tile Components")]
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;
    public MeshRenderer meshRenderer;
    public GameObject tileObj;

    [Header("Tile Data")]
    public Vector2Int position;
    [ReadOnly, SerializeField] TileData _currentTile;
    [ReadOnly, SerializeField] CardData _attachedTileCard;
    [ReadOnly, SerializeField] CardData _attachedStructureCard;

    [ShowInInspector, ReadOnly]
    public Dictionary<SideType, TileHandler> cardinalNeighboringTilesMap;
    [ShowInInspector, ReadOnly]
    public Dictionary<SideType, TileHandler> intercardinalNeighboringTilesMap;

    [ShowInInspector]
    public SideDirection[] cardinalDirectionMap = new SideDirection[4];
    [ShowInInspector]
    public SideDirection[] intercardinalDirectionMap = new SideDirection[4];

    private int currentRotation = 0;

    #endregion

    #region Detection Logic
    public void CacheNeighbors()
    {
        cardinalNeighboringTilesMap = new Dictionary<SideType, TileHandler>();

        foreach (SideDirection side in cardinalDirectionMap)
        {
            //Debug.Log($"{transform.name} | Attempting to detect neighbor on side {side.type}. " +
            //$"\n Checking Grid Corridinates: {position} + {side.direction} = {position + side.direction}");

            TileHandler neighbor = GridManager.Instance.GetTile(position + side.direction);

            if (neighbor == null)
            {
                //Debug.Log($"{transform.name} | No neighboring tile detected on side {side.type}");
                continue;
            }

            //Debug.Log($"OBJ: {transform.name} | {neighbor} was detected as a neighbor on the {side.type} side");
            cardinalNeighboringTilesMap.Add(side.type, neighbor);
        }
    }

    public SideType DetectNeighbors()
    {
        //Debug.Log("Detecting Paths");
        foreach (KeyValuePair<SideType, TileHandler> kvp in cardinalNeighboringTilesMap)
        {
            SideType side = kvp.Key;
            TileHandler neighbor = kvp.Value;

            if (neighbor == null) continue;

            if (neighbor.CurrentTile.Tier == TileTier.Path)
            {
                //Debug.Log($"OBJ: {transform.name} | {neighbor.name} has been detected as a path on the {side} side. Attempting to rotate.");
                return side;
            }
        }

        //Debug.LogWarning($"No path detected for {transform.name}");
        return SideType.Right;
    }
    #endregion

    #region Rotation Logic
    /// <summary>
    /// Rotates tile to the next allowed angle for a given SideType. Ensures that:
    /// <list type="bullet">
    /// <item><description>The tile only rotates to predefined angles.</description></item>
    /// <item><description>The rotation is relative to the tile's original orientation.</description></item>
    /// </list>
    /// </summary>
    /// <param name="tileData"></param>
    /// <param name="side"></param>
    public void RotateTileToNextAngle(SideType side)
    {

        //Find the RotationRequirment for the given side
        RotationRequirement requirement = CurrentTile.rotationRequirements.Find(req => req.side == side);

        //Ensure the requirement exsists
        if (requirement == null)
        {
            //Debug.LogError($"No rotation requirements found for side: {side}");
            return;
        }

        //Get the allowed rotations from the requirement
        int[] allowedRotations = requirement.allowedRotations;

        //Find the next rotation in the sequence
        int nextRotationIndex = (Array.IndexOf(allowedRotations, currentRotation) + 1) % allowedRotations.Length;
        int nextRotation = allowedRotations[nextRotationIndex];

        //Calculate the rotation delta
        int rotationDelta = (nextRotation - currentRotation + 360) % 360;

        //Apply the Rotation
        RotateTile(rotationDelta);

        //Update current rotation
        currentRotation = nextRotation;

        //Debug.Log($"Tile rotated to {currentRotation}° relative to original for side {side}.");
    }

    public void RotateTile(int degrees)
    {
        transform.Rotate(0, degrees, 0); // Rotates the GameObject 90 degrees around the Y-axis

        //Debug.Log($"Tile rotated by {degrees}°.");
    }
    #endregion

    public CardData GetCard(CardType cardType)
    {
        switch (cardType)
        {
            case CardType.TileCard:
                return _attachedTileCard;
            case CardType.StructureCard:
                return _attachedStructureCard;
        }
        return null;
    }



    #region Tile Data Sync Logic
    private void SetTileVisuals()
    {
        meshFilter.mesh = _currentTile.Mesh;
        meshCollider.sharedMesh = _currentTile.Mesh;
        meshRenderer.material = _currentTile.Material;
        if (tileObj != null) Destroy(tileObj);
        tileObj = _currentTile.SpawnObj(transform);
    }

    private void SpawnStructurePrefab()
    {
        if (tileObj != null) Destroy(tileObj);
        tileObj = _attachedStructureCard.SpawnObj(transform);
    }
    #endregion
}
