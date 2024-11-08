using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class BattleGrid : MonoBehaviour
    {
        [SerializeField]
        protected TileAssetData tileAssetData;
        [SerializeField]
        protected Vector2 gridSize = Vector2.one;
        [SerializeField]
        protected Vector2 tileOffset = Vector2.zero;

        protected List<BattleTile> tiles = new List<BattleTile>();

        private void Awake()
        {
            SpawnGrid();
        }

        private void SpawnGrid()
        {
            if (tiles.Count > 0)
            {
                throw new System.MethodAccessException($"Invalid call of {nameof(SpawnGrid)} method, grid has already been spawned. This should only be used once.");
            }

            if (tileAssetData == null)
            {
                Debug.LogError($"Tile Asset Data inside Battle Grid is null, grid won't be generated in {name}");
                return;
            }

            if (!tileAssetData.IsValid())
            {
                throw new System.NullReferenceException($"Referenced Tile Asset Data ({tileAssetData.name}) inside Battle Grid component is invalid. Ensure that values have been properly set - Object Name: {name}");
            }

            GameObject gridContainer = new GameObject("Tiles Container");
            gridContainer.transform.parent = transform;
            gridContainer.transform.position = transform.position;

            // Don't offset the first one since its already centered, so -1;
            float xPositionCenteringOffset = -(tileAssetData.Width * (gridSize.x - 1)) * 0.5f;
            float yPositionCenteringOffset = -(tileAssetData.Height * (gridSize.y - 1)) * 0.5f;

            for (int width = 0; width < Mathf.Abs(gridSize.x); width++)
            {
                for (int height = 0; height < Mathf.Abs(gridSize.y); height++)
                {
                    BattleTile tileInstance = tileAssetData.GetTileInstance(gridContainer.transform);
                    if (tileInstance == null)
                    {
                        throw new System.NullReferenceException($"Instantiated Battle Tile inside Battle Grid component returned a null instance in {nameof(SpawnGrid)} method - Object Name: {name}");
                    }

                    tileInstance.name = $"Tile X{width} - Y{height}";
                    tiles.Add(tileInstance);

                    float xPosition = (width * tileAssetData.Width) + xPositionCenteringOffset + (width * tileOffset.x);
                    float yPosition = (height * tileAssetData.Height) + yPositionCenteringOffset + (height * tileOffset.y);
                    tileInstance.transform.position += new Vector3(xPosition, yPosition);
                }
            }
        }
    }
}
