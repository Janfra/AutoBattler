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

        public void TrySpawnSelectedUnitAt(Vector2 position)
        {
            if (!unitCreation.HasSelectedUnit)
            {
                return;
            }

            BattleTile tile;
            if(!grid.TryGetPositionTile(position, out tile))
            {
                return;
            }

            unitCreation.TrySpawnSelectedUnitAt(tile);
        }

        public BattleGrid GetGrid()
        {
            return grid;
        }
    }
}
