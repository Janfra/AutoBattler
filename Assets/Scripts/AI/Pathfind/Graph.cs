using AutoBattler;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GameAI
{
    [Serializable]
    public class GraphNodeHandle
    {
        const int INVALID_HANDLE_INDEX = -1;

        public int Handle => handle;
        private int handle;

        public GraphNodeHandle(int handle = INVALID_HANDLE_INDEX)
        {
            this.handle = handle;
        }

        public bool IsValid()
        {
            return handle != INVALID_HANDLE_INDEX;
        }

        public bool IsValid(ref List<GraphNode> nodes)
        {
            return handle != INVALID_HANDLE_INDEX && handle < nodes.Count;
        }
    }

    [Serializable]
    public class Graph
    {
        [SerializeField]
        private List<GraphNode> nodes = new List<GraphNode>();
        public void LoadNodes(int graphWitdh, int grapthHeight)
        {
            if (nodes.Count > 0)
            {
                throw new MethodAccessException($"{nameof(LoadNodes)} method should only be called once. Graph has already been initialized beforehand.");
            }

            int index = 0;
            Dictionary<Vector2Int, GraphNode> nodesPositions = new Dictionary<Vector2Int, GraphNode>();
            List<Vector2Int> storedPositions = new List<Vector2Int>();

            for (int width = 0; width < graphWitdh; width++)
            {
                for (int heigth = 0; heigth < grapthHeight; heigth++)
                {
                    GraphNode currentNode = new GraphNode(index);
                    Vector2Int graphPosition = new Vector2Int(width, heigth);
                    nodes.Add(currentNode);
                    nodesPositions[graphPosition] = currentNode;
                    storedPositions.Add(graphPosition);
                    index++;
                }
            }

            foreach (Vector2Int position in storedPositions)
            {
                Vector2Int upNodePosition = position + Vector2Int.up;
                Vector2Int downNodePosition = position + Vector2Int.down;
                Vector2Int leftNodePosition = position + Vector2Int.left;
                Vector2Int rightNodePosition = position + Vector2Int.right;
                Vector2Int[] edgeNodePositions = { upNodePosition, downNodePosition, leftNodePosition, rightNodePosition };
                GraphNode currentNode = nodesPositions[position];

                foreach (Vector2Int edgeNodePosition in edgeNodePositions)
                {
                    if (nodesPositions.ContainsKey(edgeNodePosition))
                    {
                        SetNodeEdge(currentNode, nodesPositions[edgeNodePosition]);
                    }
                }
            }
        }

        public GraphNodeHandle GetNodeHandleAt(int index)
        {
            if (index < nodes.Count && index >= 0)
            {
                return new GraphNodeHandle(index);
            }

            return null;
        }

        public void SetNodePosition(GraphNodeHandle handler, Vector3 position)
        {
            if (handler.IsValid(ref nodes))
            {
                nodes[handler.Handle].worldPosition = position;
            }
        }

        public void SetNodePosition(GraphNodeHandle handler, Transform transform)
        {
            if (handler.IsValid(ref nodes))
            {
                Debug.Log($"Set the node {handler.Handle} to the transform of {transform.name}");
                nodes[handler.Handle].worldPosition = transform.position;
            }
        }

        public Vector3[] GetPathDFS(GraphNodeHandle startNodeHandler, GraphNodeHandle targetNodeHandler) 
        {
            int[] route = new int[nodes.Count];
            bool[] visited = new bool[nodes.Count];
            Stack<GraphEdge> targets = new Stack<GraphEdge>();
            GraphNode startNode = nodes[startNodeHandler.Handle];
            GraphNode endNode = nodes[targetNodeHandler.Handle];

            visited[startNode.index] = true;
            foreach (GraphEdge edge in startNode.edges)
            {
                targets.Push(edge);
            }

            while (targets.Count > 0)
            {
                GraphEdge edge = targets.Pop();
                route[edge.To] = edge.From;
                visited[edge.From] = true;
                if (edge.From == targetNodeHandler.Handle)
                {
                    break;
                }

                GraphNode visitedNode = nodes[edge.From];
                foreach (GraphEdge futureEdge in visitedNode.edges)
                {
                    if (visited[futureEdge.From] == false)
                    {
                        targets.Push(futureEdge);
                    }
                }
            }

            return GetPathFromRoute(startNodeHandler.Handle, targetNodeHandler.Handle, route);
        }
        
        public Vector3[] GetPathBFS(GraphNodeHandle startNodeHandler, GraphNodeHandle targetNodeHandler)
        {
            int[] route = new int[nodes.Count];
            bool[] visited = new bool[nodes.Count];
            Queue<GraphEdge> targets = new Queue<GraphEdge>();
            GraphNode startNode = nodes[startNodeHandler.Handle];
            GraphNode endNode = nodes[targetNodeHandler.Handle];

            visited[startNode.index] = true;
            foreach (GraphEdge edge in startNode.edges)
            {
                targets.Enqueue(edge);
            }

            while (targets.Count > 0)
            {
                GraphEdge edge = targets.Dequeue();
                route[edge.From] = edge.To;
                visited[edge.From] = true;
                if (edge.From == targetNodeHandler.Handle)
                {
                    break;
                }

                GraphNode visitedNode = nodes[edge.From];
                foreach (GraphEdge futureEdge in visitedNode.edges)
                {
                    if (visited[futureEdge.From] == false)
                    {
                        if (!targets.Contains(futureEdge))
                        {
                            targets.Enqueue(futureEdge);
                        }
                    }
                }
            }

            return GetReversePathFromRoute(startNodeHandler.Handle, targetNodeHandler.Handle, route);
        }

        public Vector3[] GetPathDijkstra(GraphNodeHandle startNodeHandler, GraphNodeHandle targetNodeHandler)
        {
            // Just sort for priority queue for now
            List<GraphEdge> priorityQueue = new List<GraphEdge>();

            int[] route = new int[nodes.Count];
            float[] cost = new float[nodes.Count];
            List<GraphEdge> traversedEdges = new List<GraphEdge>();
            GraphNode startNode = nodes[startNodeHandler.Handle];
            GraphNode endNode = nodes[targetNodeHandler.Handle];

            for (int i = 0; i < cost.Length; i++)
            {
                cost[i] = Mathf.Infinity;
            }

            cost[startNodeHandler.Handle] = 0.0f;
            for (int i = 0; i < startNode.edges.Count; i++)
            {
                GraphEdge edge = startNode.edges[i];
                edge.Cost = (nodes[edge.To].worldPosition - nodes[edge.From].worldPosition).sqrMagnitude;
                priorityQueue.Add(edge);

                GraphNode startNeighbourNode = nodes[edge.From];
                foreach (var neighbourEdge in startNeighbourNode.edges)
                {
                    if(neighbourEdge.From == startNodeHandler.Handle)
                    {
                        traversedEdges.Add(neighbourEdge);
                    }
                } 
            }
            priorityQueue.Sort();

            while (priorityQueue.Count > 0)
            {
                GraphEdge edge = priorityQueue[0];
                priorityQueue.RemoveAt(0);
                traversedEdges.Add(edge);

                if (cost[edge.From] > cost[edge.To] + edge.Cost)
                {
                    // This is supposed to be the other way
                    route[edge.From] = edge.To;
                    cost[edge.From] = cost[edge.To] + edge.Cost;
                    if (edge.From == targetNodeHandler.Handle)
                    {
                        Debug.Log("target found");
                    }
                }

                AddAndSortPriorityList(nodes[edge.From], cost[edge.From], ref priorityQueue, ref traversedEdges);
            }

            return GetReversePathFromRoute(startNodeHandler.Handle, targetNodeHandler.Handle, route);
        }

        public int[] GetIndexPathDFS(GraphNodeHandle startNodeHandler, GraphNodeHandle targetNodeHandler)
        {
            int[] route = new int[nodes.Count];
            bool[] visited = new bool[nodes.Count];
            Stack<GraphEdge> targets = new Stack<GraphEdge>();
            GraphNode startNode = nodes[startNodeHandler.Handle];
            GraphNode endNode = nodes[targetNodeHandler.Handle];

            visited[startNode.index] = true;
            foreach (GraphEdge edge in startNode.edges)
            {
                targets.Push(edge);
            }

            while (targets.Count > 0)
            {
                GraphEdge edge = targets.Pop();
                route[edge.To] = edge.From;
                visited[edge.From] = true;
                if (edge.From == targetNodeHandler.Handle)
                {
                    break;
                }

                GraphNode visitedNode = nodes[edge.From];
                foreach (GraphEdge futureEdge in visitedNode.edges)
                {
                    if (visited[futureEdge.From] == false)
                    {
                        targets.Push(futureEdge);
                    }
                }
            }

            return GetIndexPathFromRoute(startNodeHandler.Handle, targetNodeHandler.Handle, route);
        }

        public int[] GetIndexPathBFS(GraphNodeHandle startNodeHandler, GraphNodeHandle targetNodeHandler)
        {
            int[] route = new int[nodes.Count];
            bool[] visited = new bool[nodes.Count];
            Queue<GraphEdge> targets = new Queue<GraphEdge>();
            GraphNode startNode = nodes[startNodeHandler.Handle];
            GraphNode endNode = nodes[targetNodeHandler.Handle];

            visited[startNode.index] = true;
            foreach (GraphEdge edge in startNode.edges)
            {
                targets.Enqueue(edge);
            }

            while (targets.Count > 0)
            {
                GraphEdge edge = targets.Dequeue();
                route[edge.From] = edge.To;
                visited[edge.From] = true;
                if (edge.From == targetNodeHandler.Handle)
                {
                    break;
                }

                GraphNode visitedNode = nodes[edge.From];
                foreach (GraphEdge futureEdge in visitedNode.edges)
                {
                    if (visited[futureEdge.From] == false)
                    {
                        if (!targets.Contains(futureEdge))
                        {
                            targets.Enqueue(futureEdge);
                        }
                    }
                }
            }

            return GetReverseIndexPathFromRoute(startNodeHandler.Handle, targetNodeHandler.Handle, route);
        }

        public int[] GetIndexPathDijkstra(GraphNodeHandle startNodeHandler, GraphNodeHandle targetNodeHandler)
        {
            // Just sort for priority queue for now
            List<GraphEdge> priorityQueue = new List<GraphEdge>();

            int[] route = new int[nodes.Count];
            float[] cost = new float[nodes.Count];
            List<GraphEdge> traversedEdges = new List<GraphEdge>();
            GraphNode startNode = nodes[startNodeHandler.Handle];
            GraphNode endNode = nodes[targetNodeHandler.Handle];

            for (int i = 0; i < cost.Length; i++)
            {
                cost[i] = Mathf.Infinity;
            }

            cost[startNodeHandler.Handle] = 0.0f;
            for (int i = 0; i < startNode.edges.Count; i++)
            {
                GraphEdge edge = startNode.edges[i];
                edge.Cost = (nodes[edge.To].worldPosition - nodes[edge.From].worldPosition).sqrMagnitude;
                priorityQueue.Add(edge);

                GraphNode startNeighbourNode = nodes[edge.From];
                foreach (var neighbourEdge in startNeighbourNode.edges)
                {
                    if (neighbourEdge.From == startNodeHandler.Handle)
                    {
                        traversedEdges.Add(neighbourEdge);
                    }
                }
            }
            priorityQueue.Sort();

            while (priorityQueue.Count > 0)
            {
                GraphEdge edge = priorityQueue[0];
                priorityQueue.RemoveAt(0);
                traversedEdges.Add(edge);

                if (cost[edge.From] > cost[edge.To] + edge.Cost)
                {
                    // This is supposed to be the other way
                    route[edge.From] = edge.To;
                    cost[edge.From] = cost[edge.To] + edge.Cost;
                    if (edge.From == targetNodeHandler.Handle)
                    {
                        Debug.Log("target found");
                    }
                }

                AddAndSortPriorityList(nodes[edge.From], cost[edge.From], ref priorityQueue, ref traversedEdges);
            }

            return GetReverseIndexPathFromRoute(startNodeHandler.Handle, targetNodeHandler.Handle, route);
        }

        static public void OnGizmosDrawPath(BattleTile[] debugPath, Color color)
        {
            Vector3[] path = new Vector3[debugPath.Length];
            for (int i = 0; i < debugPath.Length; i++)
            {
                path[i] = debugPath[i].transform.position;
            }
            OnGizmosDrawPath(path, color);
        }

        static public void OnGizmosDrawPath(Vector3[] debugPath, Color color)
        {
            if (debugPath != null)
            {
                Gizmos.color = color;
                List<Vector3> path = new List<Vector3>();

                for (int i = 0; i < debugPath.Length; i++)
                {
                    path.Add(debugPath[i]);
                    if (i == 0 || i == debugPath.Length - 1)
                    {
                        continue;
                    }
                    path.Add(debugPath[i]);
                }

                if (path.Count % 2 == 0)
                {
                    Gizmos.DrawLineList(path.ToArray());
                }
            }
        }

        private Vector3[] GetReversePathFromRoute(int startNodeIndex, int targetNodeIndex, int[] route)
        {
            List<Vector3> path = new List<Vector3>();
            int currentNode = targetNodeIndex;
            path.Add(nodes[currentNode].worldPosition);

            int count = 0;
            int emergencyExitCount = route.Length;
            while (currentNode != startNodeIndex)
            {
                currentNode = route[currentNode];
                path.Add(nodes[currentNode].worldPosition);
                count++;

                if (count == emergencyExitCount)
                {
                    break;
                }
            }

            path.Reverse();
            return path.ToArray();
        }

        private Vector3[] GetPathFromRoute(int startNodeIndex, int targetNodeIndex, int[] route)
        {
            List<Vector3> path = new List<Vector3>();
            int currentNode = startNodeIndex;
            path.Add(nodes[currentNode].worldPosition);

            int count = 0;
            int emergencyExitCount = route.Length;
            while (currentNode != targetNodeIndex)
            {
                currentNode = route[currentNode];
                path.Add(nodes[currentNode].worldPosition);
                count++;

                if (count == emergencyExitCount)
                {
                    break;
                }
            }

            return path.ToArray();
        }

        private int[] GetReverseIndexPathFromRoute(int startNodeIndex, int targetNodeIndex, int[] route)
        {
            List<int> path = new List<int>();
            int currentNode = targetNodeIndex;
            path.Add(currentNode);

            int count = 0;
            int emergencyExitCount = route.Length;
            while (currentNode != startNodeIndex)
            {
                currentNode = route[currentNode];
                path.Add(currentNode);
                count++;

                if (count == emergencyExitCount)
                {
                    break;
                }
            }

            return path.ToArray();
        }

        private int[] GetIndexPathFromRoute(int startNodeIndex, int targetNodeIndex, int[] route)
        {
            List<int> path = new List<int>();
            int currentNode = startNodeIndex;
            path.Add(currentNode);

            int count = 0;
            int emergencyExitCount = route.Length;
            while (currentNode != targetNodeIndex)
            {
                currentNode = route[currentNode];
                path.Add(currentNode);
                count++;

                if (count == emergencyExitCount)
                {
                    break;
                }
            }

            return path.ToArray();
        }

        private void AddAndSortPriorityList(GraphNode targetNode, float originCost, ref List<GraphEdge> priorityQueue, ref List<GraphEdge> traversedEdges)
        {
            for (int i = 0; i < targetNode.edges.Count; i++)
            {
                GraphEdge edge = targetNode.edges[i];
                if (priorityQueue.Contains(edge) || traversedEdges.Contains(edge))
                {
                    continue;
                }

                edge.Cost = (nodes[edge.To].worldPosition - nodes[edge.From].worldPosition).sqrMagnitude + originCost;
                priorityQueue.Add(edge);
            }
            priorityQueue.Sort();
        }

        private void SetNodeEdge(GraphNode node, GraphNode edgeNode)
        {
            if (node == null)
            {
                return;
            }

            if (edgeNode == null)
            {
                return;
            }

            node.AddEdge(edgeNode.index);
        }
    }

    [Serializable]
    public class GraphNode
    {
        public int index;
        public Vector2 worldPosition;
        public List<GraphEdge> edges = new List<GraphEdge>();

        public GraphNode(int index)
        {
            this.index = index;
        }

        public void AddEdge(int edgeIndex)
        {
            edges.Add(new GraphEdge(index, edgeIndex));
        }
    }

    [Serializable]
    public class GraphEdge : IComparable<GraphEdge>
    {
        public int To;
        public int From;
        public float Cost;

        public GraphEdge(int To, int From)
        {
            this.To = To;
            this.From = From;
            Cost = 0.0f;
        }

        public int CompareTo(GraphEdge other)
        {
            return Cost.CompareTo(other.Cost);
        }
    }
}
