using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class BattleArena : MonoBehaviour
    {
        [SerializeField]
        private BattleGrid grid;
        [SerializeField]
        private UnitCreation unitCreation;

        public void TrySpawnUnitAt(Vector2 position)
        {
            Vector2 spawnPosition;
            if(!grid.TryGetPositionClosestTilePosition(position, out spawnPosition))
            {
                return;
            }

            unitCreation.TrySpawnSelectedUnitAt(position);
        }

        public BattleGrid GetGrid()
        {
            return grid;
        }
    }
}
