using GameAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BattleTile : MonoBehaviour
    {
        public GraphNodeHandle PathfindHandler;
        
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        public SpriteRenderer SpriteRenderer => spriteRenderer;

        private void Awake()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }
    }
}
