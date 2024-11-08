using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class BattleArena : MonoBehaviour
    {
        [SerializeField]
        private BattleGrid grid;

        #region Units Section

        [SerializeField]
        private List<string> ArenaUnitsToBe;

        #endregion

        public BattleGrid GetGrid()
        {
            return grid;
        }
    }
}
