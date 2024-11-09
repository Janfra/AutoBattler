using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    [CreateAssetMenu(fileName = "New Tile Asset Data", menuName = "ScriptableObjects/Tile Asset Data")]
    public class TileAssetData : ScriptableObject, IValidated
    {
        public BattleTile TilePrefab;
        public float Width = 1.0f;
        public float Height = 1.0f;

        private void OnValidate()
        {
            Width = Mathf.Abs(Width);
            Height = Mathf.Abs(Height);
        }

        public BattleTile GetTileInstance(Transform parentTransform)
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

        public bool IsValid()
        {
            return TilePrefab != null && Width > 0.0f && Height > 0.0f;
        }
    }
}
