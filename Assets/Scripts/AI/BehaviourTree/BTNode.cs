using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;


namespace GameAI
{
    [Serializable]
    /// <summary>
    /// Execute can return one of three things
    /// </summary>
    public enum BTStatus
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    [Serializable]
    /// <summary>
    /// Base class. Sets the foundations for everything else
    /// </summary>
    public abstract class BTNode
    {
        protected Blackboard bb;
        public BTNode(Blackboard bb)
        {
            this.bb = bb;
        }

        public virtual void EnterNode() { }

        public virtual void ExitNode() { }

        public abstract BTStatus Execute();

        /// <summary>
        /// Reset should be overidden in child classes as and when necessary
        /// It should be called when a node is abruptly aborted before it can finish with a success or failure
        /// i.e the node was still RUNNING when it is aborted you need to gracefully handle it to avoid unintended bugs
        /// See DelayNode, CompositeNode and DecoratorNode for examples
        /// </summary>
        public virtual void Reset()
        {

        }
    }

    /// <summary>
    /// Base class for node that can take child nodes. Only meant to be used in subclasses like Selector and Sequence,
    /// but you can add other subclass types (e.g. RandomSelector, RandomSequence, Parallel etc.)
    /// </summary>
    public abstract class CompositeNode : BTNode
    {
        protected int CurrentChildIndex = 0;
        protected List<BTNode> children;
        public CompositeNode(Blackboard bb) : base(bb)
        {
            children = new List<BTNode>();
        }

        public void AddChild(BTNode child)
        {
            children.Add(child);
        }

        /// <summary>
        /// When a composite node is reset it set the child index back to 0, and it should propogate the reset down to all its children
        /// </summary>
        public override void Reset()
        {
            CurrentChildIndex = 0;
            //Reset every child
            for (int j = 0; j < children.Count; j++)
            {
                children[j].Reset();
            }
        }
    }

    /// <summary>
    /// Selectors execute their children in order until a child succeeds, at which point it stops execution
    /// If a child returns RUNNING, then it will need to stop execution but resume from the same point the next time it executes
    /// </summary>
    public class SelectorNode : CompositeNode
    {
        public SelectorNode(Blackboard bb) : base(bb)
        {

        }

        public override BTStatus Execute()
        {
            BTStatus rv = BTStatus.FAILURE;

            for (int i = CurrentChildIndex; i < children.Count; i++)
            {
                BTNode child = children[i];
                child.EnterNode();
                rv = child.Execute();
                if (rv == BTStatus.SUCCESS)
                {
                    Reset();
                    break;
                }
                else if (rv == BTStatus.RUNNING)
                {
                    CurrentChildIndex = i;
                    break;
                }
            }

            return rv;
        }
    }

    /// <summary>
    /// Sequences execute their children in order until a child fails, at which point it stops execution
    /// If a child returns RUNNING, then it will need to stop execution but resume from the same point the next time it executes
    /// </summary>
    public class SequenceNode : CompositeNode
    {
        public SequenceNode(Blackboard bb) : base(bb)
        {
        }

        public override BTStatus Execute()
        {
            BTStatus rv = BTStatus.FAILURE;

            for (int i = CurrentChildIndex; i < children.Count; i++)
            {
                BTNode child = children[i];
                child.EnterNode();
                rv = child.Execute();
                if (rv == BTStatus.FAILURE)
                {
                    Reset();
                    break;
                }
                else if (rv == BTStatus.RUNNING)
                {
                    CurrentChildIndex = i;
                    break;
                }
            }

            return rv;
        }
    }

    /// <summary>
    /// Decorator nodes customise functionality of other nodes by wrapping around them, see InverterDecorator for example
    /// </summary>
    public abstract class DecoratorNode : BTNode
    {
        protected BTNode WrappedNode;
        public DecoratorNode(BTNode WrappedNode, Blackboard bb) : base(bb)
        {
            this.WrappedNode = WrappedNode;
        }

        public BTNode GetWrappedNode()
        {
            return WrappedNode;
        }

        /// <summary>
        /// Should reset the wrapped node
        /// </summary>
        public override void Reset()
        {
            WrappedNode.Reset();
        }
    }


    /// <summary>
    /// Inverter decorator simply inverts the result of success/failure of the wrapped node
    /// </summary>
    public class InverterDecoratorNode : DecoratorNode
    {
        public InverterDecoratorNode(BTNode WrappedNode, Blackboard bb) : base(WrappedNode, bb)
        {

        }

        public override BTStatus Execute()
        {
            BTStatus rv = WrappedNode.Execute();

            if (rv == BTStatus.FAILURE)
            {
                rv = BTStatus.SUCCESS;
            }
            else if (rv == BTStatus.SUCCESS)
            {
                rv = BTStatus.FAILURE;
            }

            return rv;
        }
    }

    /// <summary>
    /// Inherit this and override CheckStatus. If that returns true, then it will execute the WrappedNode otherwise it will return failure
    /// </summary>
    public abstract class ConditionalDecoratorNode : DecoratorNode
    {
        public ConditionalDecoratorNode(BTNode WrappedNode, Blackboard bb) : base(WrappedNode, bb)
        {

        }

        public abstract bool CheckStatus();
        public override BTStatus Execute()
        {
            BTStatus rv = BTStatus.FAILURE;

            if (CheckStatus())
                rv = WrappedNode.Execute();

            return rv;
        }


    }

    /// <summary>
    /// This node simply returns success after the allotted delay time has passed
    /// </summary>
    public class DelayNode : BTNode
    {
        protected float Delay = 0.0f;
        bool Started = false;
        private Timer regulator;
        bool DelayFinished = false;
        public DelayNode(Blackboard bb, float DelayTime) : base(bb)
        {
            this.Delay = DelayTime;
            regulator = new Timer(Delay * 1000.0f); // in milliseconds, so multiply by 1000
            regulator.Elapsed += OnTimedEvent;
            regulator.Enabled = true;
            regulator.Stop();
        }

        public override BTStatus Execute()
        {
            BTStatus rv = BTStatus.RUNNING;
            if (!Started
                && !DelayFinished)
            {
                Started = true;
                regulator.Start();
            }
            else if (DelayFinished)
            {
                DelayFinished = false;
                Started = false;
                rv = BTStatus.SUCCESS;
            }

            return rv;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Started = false;
            DelayFinished = true;
            regulator.Stop();
        }

        //Timers count down independently of the Behaviour Tree, so we need to stop them when the behaviour is aborted/reset
        public override void Reset()
        {
            regulator.Stop();
            DelayFinished = false;
            Started = false;
        }

        public abstract class GoalNode : BTNode
        {
            bool isActive;

            protected GoalNode(Blackboard bb) : base(bb)
            {

            }
        }

        public abstract class CompositeGoalNode : GoalNode
        {
            Stack<GoalNode> goalNodesStack = new Stack<GoalNode>();

            protected CompositeGoalNode(Blackboard bb) : base(bb)
            {

            }

            void AddSubgoal(GoalNode node)
            {
                goalNodesStack.Push(node);
            }

            public override BTStatus Execute()
            {
                BTStatus rv = goalNodesStack.Peek().Execute();

                if (rv == BTStatus.SUCCESS)
                {
                    GoalNode node = goalNodesStack.Pop();
                    node.ExitNode();
                    node.Reset();
                }

                return rv;
            }

            public override void Reset()
            {
                foreach (GoalNode node in goalNodesStack)
                {
                    node.Reset();
                }
            }
        }
    }
}