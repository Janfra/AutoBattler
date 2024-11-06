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

        private void Start()
        {
            
        }

        private void SpawnGrid()
        {
            if (tileAssetData == null)
            {
                Debug.LogError($"Tile Asset Data inside Battle Grid is null, grid won't be generated in {name}");
                return;
            }

            if (!tileAssetData.IsValid())
            {
                throw new System.NullReferenceException($"Referenced Tile Asset Data ({tileAssetData.name}) inside Battle Grid component is invalid. Ensure that values have been properly set - Object Name: {name}");
            }

            // Don't offset the first one since its already centered, so -1;
            float xPositionCenteringOffset = -(tileAssetData.Width * (gridSize.x - 1)) * 0.5f;
            float yPositionCenteringOffset = -(tileAssetData.Height * (gridSize.y - 1)) * 0.5f;

            for (int width = 0; width < Mathf.Abs(gridSize.x); width++)
            {
                for (int height = 0; height < Mathf.Abs(gridSize.y); height++)
                {
                    BattleTile tileInstance = tileAssetData.GetTileInstance(transform);
                    if (tileInstance == null)
                    {
                        throw new System.NullReferenceException($"Instantiated Battle Tile inside Battle Grid component returned a null instance in SpawnGrid method - Object Name: {name}");
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
