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
        private LayerMask tileLayer;

        [SerializeField]
        protected Vector2 gridSize = Vector2.one;
        [SerializeField]
        protected Vector2 tileOffset = Vector2.zero;

        protected List<BattleTile> tiles = new List<BattleTile>();
        private Vector2[] cornerPositions = new Vector2[4];
        private bool canCheckGridArea = false;

        private void Awake()
        {
            SpawnGrid();
        }

        /// <summary>
        /// Checks if a point is inside of the grid area, makes some assumptions knowing how the grid is spawned
        /// </summary>
        /// <param name="position">The position to check if is inside the grid</param>
        /// <returns>Is the given position inside the grid</returns>
        public bool IsPositionWithinGrid(Vector2 position)
        {
            if (!canCheckGridArea)
            {
                Debug.LogWarning($"Grid area has not been setup, returning false to {nameof(IsPositionWithinGrid)} in {GetType().Name} component. - Object Name: {name}");
                return false;
            }

            // Assumptions:
            // 1. We are a square / rectangle
            // 2. Corners were already setup so we can assume the directions and their position in the array

            Vector2 A = cornerPositions[0];
            Vector2 AP = position - A;

            Vector2 B = cornerPositions[1];
            Vector2 AB = B - A;
            float scalarABAP = Vector2.Dot(AP, AB);

            // Is opposite of AB or is the projection longer than AB (checking for up and down)
            if (scalarABAP < 0) return false;
            if (scalarABAP > Vector2.Dot(AB, AB)) return false;

            Vector2 D = cornerPositions[3];
            Vector2 AD = D - A;
            float scalarADAP = Vector2.Dot(AP, AD);

            // Same as AB but with AD (checking for left and right)
            if (scalarADAP < 0) return false;
            if (scalarADAP > Vector2.Dot(AD, AD)) return false;

            return true;
        }

        public bool TryGetPositionTile(Vector2 position, out BattleTile tile)
        {
            tile = null;
            if (!IsPositionWithinGrid(position))
            {
                return false;
            }

            const float TILE_CHECK_RANGE = 0.1f;
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, TILE_CHECK_RANGE, tileLayer.value);
            if (hit)
            {
                tile = hit.collider.GetComponent<BattleTile>();
                return tile;
            }
            return false;
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
            int cornerIndexCount = 0;

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
                    
                    if (TrySetCornerPosition(new Vector2(width, height), tileInstance.transform.position, cornerIndexCount))
                    {
                        cornerIndexCount++;
                    }
                }
            }
        }

        private bool TrySetCornerPosition(Vector2 tileGridPosition, Vector2 tilePosition, int cornerIndex)
        {
            if (cornerIndex >= 4)
            {
                throw new System.IndexOutOfRangeException($"Try to set the corner position of the Battle Grid to an invalid index");
            }

            if ((tileGridPosition.x == 0 || tileGridPosition.x == gridSize.x - 1) && (tileGridPosition.y == 0 || tileGridPosition.y == gridSize.y - 1))
            {
                Vector2 gridPosition = new Vector2();
                gridPosition.x = transform.position.x;
                gridPosition.y = transform.position.y;

                // In this case it works since the grid has at least 4 tiles
                Vector2 directionToTile = tilePosition - gridPosition;

                float tempPos = directionToTile.y;
                directionToTile.y = 0;
                float widthScalar = Vector2.Dot(directionToTile.normalized, Vector2.right);

                directionToTile.y = tempPos;
                directionToTile.x = 0;
                float heightScalar = Vector2.Dot(directionToTile.normalized, Vector2.up);

                Vector2 cornerPosition = Vector2.zero;
                cornerPosition.x = tilePosition.x + ((tileAssetData.Width * 0.5f) * widthScalar);
                cornerPosition.y = tilePosition.y + ((tileAssetData.Height * 0.5f) * heightScalar);

                // Swap the position of the last 2 corners to make a clockwise corners shape in the array
                // This is done following the for loop order of application
                if (cornerIndex == 3)
                {
                    Vector2 temp = cornerPositions[2];
                    cornerPositions[3] = temp;
                    cornerPositions[2] = cornerPosition;
                    canCheckGridArea = true;
                }
                else
                {
                    cornerPositions[cornerIndex] = cornerPosition;
                }

                return true;     
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            var a = cornerPositions[0];
            var b = cornerPositions[1];
            var c = cornerPositions[2];
            var d = cornerPositions[3];

            Gizmos.color = Color.black;
            Gizmos.DrawLine(a, b);
            Gizmos.DrawLine(b, c);
            Gizmos.DrawLine(c, d);
            Gizmos.DrawLine(d, a);
        }
    }

}
