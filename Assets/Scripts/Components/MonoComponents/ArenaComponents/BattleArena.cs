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
                Vector3[] path = new Vector3[debugPath.Length];
                for (int i = 0; i < debugPath.Length; i++)
                {
                    path[i] = debugPath[i].transform.position;
                }
                Graph.OnGizmosDrawPath(path);
            }
        }
    }
}
