using GameAI;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace AutoBattler
{
    [RequireComponent(typeof(BattleGrid), typeof(UnitManager))]
    public class BattleArena : MonoBehaviour
    {
        [SerializeField]
        private BattleGrid grid;
        [SerializeField]
        private UnitManager unitManager;

        private BattleTile[] debugPath;

        private void Awake()
        {
            if (grid == null)
            {
                grid = GetComponent<BattleGrid>();
            }

            if (unitManager == null)
            {
                unitManager = GetComponent<UnitManager>();
            }
        }

        private void Start()
        {
            unitManager.SetSelectedUnit();
            BattleTile tile;
            grid.TryGetPositionTile(Vector2.zero, out tile);
            unitManager.TrySpawnSelectedUnitAt(tile, grid.GetPathfindRequester(), false);   
        }

        public void TrySpawnSelectedUnitAt(Vector2 position)
        {
            if (!unitManager.HasSelectedUnit)
            {
                return;
            }

            BattleTile tile;
            if (!grid.TryGetPositionTile(position, out tile))
            {
                return;
            }

            // Testing pathfind requester
            GraphNodeHandle startTest = new GraphNodeHandle(12);
            debugPath = grid.GetPathfindRequester().GetPathFromTo(startTest, tile.pathfindHandler);
            unitManager.TrySpawnSelectedUnitAt(tile, grid.GetPathfindRequester());
        }

        public BattleGrid GetGrid()
        {
            return grid;
        }

        private void OnDrawGizmos()
        {
            if (debugPath != null)
            {
                Graph.OnGizmosDrawPath(debugPath, Color.white);
            }
        }
    }
}
