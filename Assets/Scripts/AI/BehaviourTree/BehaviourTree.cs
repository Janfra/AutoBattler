using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    public class BehaviourTree : MonoBehaviour
    {
        [SerializeField]
        protected Blackboard blackboard;
        [SerializeField]
        protected BTNode rootNode;

        private void Awake()
        {
            rootNode = new SequenceNode(blackboard);
        }
    }
}