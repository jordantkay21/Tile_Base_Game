using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KayosStudios.Archived
{
    [System.Serializable]
    public enum CardType
    {
        TileCard,
        StructureCard
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public TileData[] tileTypes = new TileData[9];
        public CardData[] structureTypes = new CardData[9];

        [Header("Tile Selection Properties")]
        public Camera raycastCamera;
        public LayerMask tileLayerMask;
        public GameObject hoveredTile;
        public GameObject selectedTile;

        [Header("Camera Properties")]
        public CameraHandler playerCameraControls;

        public static event Action<GameObject> OnTileSelected;
        public static event Action<GameObject> OnTileHovered;
        public static event Action OnTileDefined;
        public static event Action<TileHandler, TileMenuType> OnPathSpawned;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            playerCameraControls.enabled = false;
        }

        private void OnEnable()
        {
            InputManager.OnHover += HandleHover;
            InputManager.OnSelect += HandleSelect;
            InputManager.OnCameraControlActivate += HandleCameraActivation;

        }

        private void HandleCameraActivation(bool isActive)
        {
            if (playerCameraControls == null)
            {
                playerCameraControls = FindFirstObjectByType<CameraHandler>();

                if (playerCameraControls == null)
                {
                    Debug.LogError("CameraHandler is missing! Ensure it exists in the scene.");
                    return;
                }
            }

            playerCameraControls.enabled = isActive;
        }

        public void PathSpawn(TileHandler tile)
        {
            OnPathSpawned?.Invoke(tile, TileMenuType.Rotate);
        }

        #region Input Logic
        private void HandleHover(Vector2 screenPos)
        {
            Ray ray = raycastCamera.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, tileLayerMask))
            {
                hoveredTile = hit.collider.gameObject;
            }
            else
            {
                hoveredTile = null;
            }
            OnTileHovered?.Invoke(hoveredTile);
        }

        private void HandleSelect()
        {
            if (UIManager.Instance.isPointerOverUI) return;

            if (hoveredTile != null)
            {
                SelectTile(hoveredTile);
            }
            else
            {
                DeselectTile();
            }
        }
        #endregion

        #region Tile Selection Logic

        private void SelectTile(GameObject tile)
        {
            if (selectedTile != null)
            {
                DeselectTile();
            }

            selectedTile = tile;

            OnTileSelected.Invoke(tile);
        }

        private void DeselectTile()
        {
            if (selectedTile != null)
            {
                selectedTile = null;
            }
            OnTileSelected?.Invoke(null);
        }

        #endregion

        #region Tile Data Logic

        public void AttachCard(CardData newCard)
        {
            //Debug.Log($"{newCard.cardType} was clicked");
            if (selectedTile != null)
            {
                TileHandler tileData = selectedTile.GetComponent<TileHandler>();
                tileData.AttachedCard = newCard;
                OnTileDefined?.Invoke();
            }
            else
            {
                //Debug.Log("SelectedTile is NULL");
            }

        }

        public TileData GetTileData(TileType tileType)
        {
            foreach (TileData type in tileTypes)
            {
                if (type.Type == tileType)
                    return type;
            }

            return null;
        }

        public CardData GetStructureCard(StructureType structureType)
        {
            foreach (CardData card in structureTypes)
            {
                if (card.structureType == structureType)
                    return card;
            }

            return null;
        }

        #endregion
    }
}