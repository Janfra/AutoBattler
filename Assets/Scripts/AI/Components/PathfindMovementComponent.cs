using AutoBattler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    public class PathfindMovementComponent : MovementComponent
    {
        [SerializeField]
        private SharedGraphNodeHandle ownerPathfindNode;
        private GraphNodeHandle targetNode;
        private BattleTile currentTargetTile;
        private PathfindRequester pathfindRequester;
        private IEnumerator movementCoroutine;
        private bool hasReachedTarget = false;
        BattleTile[] waypoints;

        public void Initialise(PathfindRequester requester, SharedGraphNodeHandle sharedHandle)
        {
            SetPathfindRequester(requester);
            SetPathfindNode(sharedHandle);
        }

        public void SetPathfindNode(SharedGraphNodeHandle sharedHandle)
        {
            if (sharedHandle != null && sharedHandle.Value.IsValid())
            {
                ownerPathfindNode = sharedHandle;
            }
        }

        public void SetPathfindRequester(PathfindRequester requester)
        {
            if (requester != null)
            {
                pathfindRequester = requester;
            }
        }

        public void SetPathfindTarget(GraphNodeHandle target)
        {
            if (target == null)
            {
                return;
            }

            if (targetNode == target)
            {
                return;
            }

            targetNode = target;
            waypoints = pathfindRequester.GetPathFromTo(ownerPathfindNode.Value, target);
            if (waypoints.Length <= 0)
            {
                return;
            }

            if (movementCoroutine != null)
            {
                StopAndClearMovementCoroutine();
            }

            Debug.Log($"Set pathfind target for {name}");
            movementCoroutine = MoveToTargetTile(new Stack<BattleTile>(waypoints));
            StartCoroutine(movementCoroutine);
        }

        private IEnumerator MoveToTargetTile(Stack<BattleTile> waypoints)
        {
            BattleTile targetTile = waypoints.Pop();
            OnTargetReached += OnTargetTileReached;
            OnTargetCancelled += OnTargetTileCancelled;
            if (targetTile != null)
            {
                SetTileAsMovementTarget(targetTile);
            }

            // Keep moving to the next waypoint
            while (targetTile != null && waypoints.Count > 0)
            {
                if (hasReachedTarget)
                {
                    targetTile = waypoints.Pop();
                    SetTileAsMovementTarget(targetTile);
                }

                yield return null;
            }

            // On last waypoint, wait till arrive
            while (!hasReachedTarget)
            {
                yield return null;
            }

            OnTargetCancelled -= OnTargetTileCancelled;
            OnTargetReached -= OnTargetTileReached;
        }

        private void SetTileAsMovementTarget(BattleTile targetTile)
        {
            currentTargetTile = targetTile;
            SetMovementTarget(targetTile.transform.position);
            hasReachedTarget = false;
        }

        private void OnTargetTileReached()
        {
            hasReachedTarget = true;
            ownerPathfindNode.Value = currentTargetTile.pathfindHandler;
        }

        private void StopAndClearMovementCoroutine()
        {
            OnTargetReached -= OnTargetTileReached;
            OnTargetCancelled -= OnTargetTileCancelled;
            StopCoroutine(movementCoroutine);
        }

        private void OnTargetTileCancelled()
        {
            StopAndClearMovementCoroutine();
        }

        private void OnDrawGizmos()
        {
            if (waypoints == null)
            {
                return;
            }

            if (waypoints.Length > 0)
            {
                Graph.OnGizmosDrawPath(waypoints, Color.green);
            }
        }
    }
}
