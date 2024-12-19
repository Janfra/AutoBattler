using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AutoBattler
{
    public enum ETileSpriteType
    {
        Middle,
        TopBorder,
        BottomBorder,
        RightBorder,
        LeftBorder,
        BottomLeftBorder,
        BottomRightBorder,
        TopLeftBorder,
        TopRightBorder,

        // Must be last, used to easily go over all the enum options
        [InspectorName("")]
        [HideInInspector]
        TILE_SPRITE_TYPE_COUNT
    }

    [Serializable]
    public struct TileSpriteData
    {
        public Sprite Sprite;
        public ETileSpriteType TilePosition;
    }

    [CreateAssetMenu(fileName = "New Tile Asset Data", menuName = "ScriptableObjects/Tile Asset Data")]
    public class TileAssetData : ScriptableObject, IValidated
    {
        public BattleTile TilePrefab;
        public TileSpriteData[] SpriteVariations;
        public float Width = 1.0f;
        public float Height = 1.0f;
        private Dictionary<ETileSpriteType, List<Sprite>> sortedSpriteVariations;

        private void OnValidate()
        {
            Width = Mathf.Abs(Width);
            Height = Mathf.Abs(Height);
        }

        private void OnEnable()
        {
            SortSpriteVariations();
        }

        public BattleTile GetTileInstance(Transform parentTransform)
        {
            BattleTile tileInstance = CreateInstance(parentTransform);
            if (tileInstance == null)
            {
                return null;
            }

            tileInstance.SpriteRenderer.sprite = GetRandomSpriteFromType(ETileSpriteType.Middle);
            return tileInstance;
        }

        public BattleTile GetTileInstance(Transform parentTransform, Vector2Int position, Vector2Int gridSize)
        {
            BattleTile tileInstance = CreateInstance(parentTransform);
            if (tileInstance == null)
            {
                return null;
            }

            if (IsTileAtBorderOfGrid(position, gridSize))
            {
                tileInstance.SpriteRenderer.sprite = GetRandomSpriteFromType(GetBorderSpriteTypeFromPosition(position, gridSize));
            }
            else
            {
                tileInstance.SpriteRenderer.sprite = GetRandomSpriteFromType(ETileSpriteType.Middle);
            }

            return tileInstance;
        }

        private bool IsTileAtBorderOfGrid(Vector2Int position, Vector2Int gridSize)
        {
            return position.x == 0 || position.x == gridSize.x - 1 || position.y == 0 || position.y == gridSize.y - 1;
        }

        private ETileSpriteType GetBorderSpriteTypeFromPosition(Vector2Int position, Vector2Int gridSize)
        {
            bool isAtBottom = position.y == 0;
            bool isAtTop = position.y == gridSize.y - 1;
            bool isAtLeft = position.x == 0;
            bool isAtRight = position.x == gridSize.x - 1;

            if (isAtBottom)
            {
                if (isAtLeft)
                {
                    return ETileSpriteType.BottomLeftBorder;
                }
                else if (isAtRight)
                {
                    return ETileSpriteType.BottomRightBorder;
                }

                return ETileSpriteType.BottomBorder;
            }
            else if (isAtTop)
            {
                if (isAtLeft)
                {
                    return ETileSpriteType.TopLeftBorder;
                }
                else if (isAtRight)
                {
                    return ETileSpriteType.TopRightBorder;
                }

                return ETileSpriteType.TopBorder;
            }

            if (isAtLeft)
            {
                return ETileSpriteType.LeftBorder;
            }
            else
            {
                return ETileSpriteType.RightBorder;
            }
        }

        private BattleTile CreateInstance(Transform parentTransform)
        {
            if (TilePrefab == null)
            {
                Debug.LogError($"Attempting to get a tile instance from a Tile Asset Data Scriptable Object without a tile prefab set - Asset Name: {name}");
                return null;
            }

            BattleTile tileInstance = Instantiate(TilePrefab, parentTransform);
            if (tileInstance == null)
            {
                throw new System.NullReferenceException($"Instantiated prefab in Tile Asset Data Scriptable Object returned null - Asset Name: {name}");
            }

            return tileInstance;
        }

        private Sprite GetRandomSpriteFromType(ETileSpriteType type)
        {
            if (sortedSpriteVariations == null)
            {
                throw new System.NullReferenceException($"{nameof(sortedSpriteVariations)} variable has not been initialised yet inside of {GetType().Name}");
            }

            List<Sprite> sprites = sortedSpriteVariations[type];
            return sprites[Random.Range(0, Mathf.Max(sprites.Count - 1, 0))];
        }

        private void SortSpriteVariations()
        {
            if (sortedSpriteVariations != null)
            {
                Debug.LogError("Sorted sprite variations for tiles have already been created");
                return;
            }

            sortedSpriteVariations = new Dictionary<ETileSpriteType, List<Sprite>>();

            for (int i = 0; i < (int)ETileSpriteType.TILE_SPRITE_TYPE_COUNT; i++)
            {
                sortedSpriteVariations[(ETileSpriteType)i] = new List<Sprite>();
            }

            foreach (var spriteVariation in SpriteVariations)
            {
                sortedSpriteVariations[spriteVariation.TilePosition].Add(spriteVariation.Sprite);
            }
        }

        public bool IsValid()
        {
            return TilePrefab != null && Width > 0.0f && Height > 0.0f;
        }
    }
}
