using GameAI;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace AutoBattler
{
    public class BattleArena : MonoBehaviour
    {
        [SerializeField]
        private BattleGrid grid;
        [SerializeField]
        private UnitCreation unitCreation;

        private BattleTile[] debugPath;

        private void Start()
        {   
            // Testing pathfind requester
            GraphNodeHandle startTest = new GraphNodeHandle(0);
            GraphNodeHandle endTest = new GraphNodeHandle(24);
            debugPath = grid.GetPathfindRequester().GetPathFromTo(startTest, endTest);
        }

        public void TrySpawnSelectedUnitAt(Vector2 position)
        {
            if (!unitCreation.HasSelectedUnit)
            {
                return;
            }

            BattleTile tile;
            if (!grid.TryGetPositionTile(position, out tile))
            {
                return;
            }

            unitCreation.TrySpawnSelectedUnitAt(tile);
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
