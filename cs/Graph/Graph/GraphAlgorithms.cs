using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public struct PathfindingInfo<T>
    {
        public Vertex<T>? Founder { get; set; }
        public double Distance { get; set; }

        public PathfindingInfo(Vertex<T> founder, double distance)
        {
            Founder = founder;
            Distance = distance;
        }
    }

    public static class GraphAlgorithms
    {
        public static bool PathExists<T>(Vertex<T> start, Vertex<T> end)
        {
            _ = start.Owner ?? throw new ArgumentException("Start vertex is not in a graph");
            _ = end.Owner ?? throw new ArgumentException("End vertex is not in a graph");
            if (start.Owner != end.Owner) throw new ArgumentException("Vertices are in different graphs");

            if (start == end) return true;

            HashSet<Vertex<T>> visited = new() { start };
            Stack<IEnumerator<Edge<T>>> stack = new();
            stack.Push(start.OutgoingEdges.GetEnumerator());

            //Peek vertices from stack, pushing and switching to first non-visited neighbor
            while(stack.TryPeek(out IEnumerator<Edge<T>> currEdges))
            {
                Vertex<T>? neighbor = null;
                while(currEdges.MoveNext())
                {
                    if (!visited.Contains(currEdges.Current.End))
                    {
                        neighbor = currEdges.Current.End;
                        if (neighbor == end) return true;
                        else break;
                    }
                }

                if (neighbor != null)
                {
                    stack.Push(neighbor.OutgoingEdges.GetEnumerator());
                    visited.Add(neighbor);
                }
                else
                {
                    stack.Pop();
                }
            }
            return false;
        }

        public static Dictionary<Vertex<T>, PathfindingInfo<T>> Dijkstra<T>(Vertex<T> start)
        {
            _ = start ?? throw new ArgumentNullException(nameof(start));
            Graph<T> graph = start.Owner ?? throw new ArgumentException("Start vertex is not in a graph");

            int a = 2;
            int b = a == 2 ? a : throw new Exception();

            foreach (Edge<T> edge in graph.Edges)
            {
                if (edge.Distance < 0.0)
                {
                    throw new ArgumentException("Cannot pathfind in a graph with negative-weight edges");
                }
            }   

            //Set up queue & dictionary
            Dictionary<Vertex<T>, PathfindingInfo<T>> visitedInfo = new();
            var comparer = Comparer<PathfindingInfo<T>>.Create((x, y) => x.Distance.CompareTo(y.Distance));
            PriorityQueue<Vertex<T>, PathfindingInfo<T>> queue = new(comparer);
            queue.Enqueue(start, new PathfindingInfo<T>(null, 0.0));

            //Visit each vertex in queue, adding non-visited neighbors
            while(queue.TryDequeue(out Vertex<T> current, out PathfindingInfo<T> currInfo))
            {
                if (visitedInfo.ContainsKey(current)) continue;
                visitedInfo.Add(current, currInfo);

                foreach (Edge<T> edge in current.OutgoingEdges)
                {
                    Vertex<T> neighbor = edge.End;
                    if (!visitedInfo.ContainsKey(neighbor))
                    {
                        queue.Enqueue(neighbor, new PathfindingInfo<T>(current, currInfo.Distance + edge.Distance));
                    }
                }
            }

            return visitedInfo;
        }

        //Returns whether there are any negative cycles
        public static bool BellmanFord<T>(Vertex<T> start, out Dictionary<Vertex<T>, PathfindingInfo<T>> pathfindingInfo)
        {
            _ = start ?? throw new ArgumentNullException(nameof(start));
            Graph<T> graph = start.Owner ?? throw new ArgumentException("Start vertex is not in a graph");

            pathfindingInfo = new Dictionary<Vertex<T>, PathfindingInfo<T>>();
            pathfindingInfo[start] = new PathfindingInfo<T>(null, 0.0);

            //Relax all edges |V| - 1 times
            for(int i = 0; i < graph.ValToVertex.Count - 1; i++)
            {
                foreach(Edge<T> edge in graph.Edges)
                {
                    if (pathfindingInfo.TryGetValue(edge.Start, out PathfindingInfo<T> startVertInfo))
                    {
                        double newEndDist = startVertInfo.Distance + edge.Distance;
                        if (!pathfindingInfo.TryGetValue(edge.End, out PathfindingInfo<T> endVertInfo)
                            || endVertInfo.Distance > newEndDist)
                        {
                            pathfindingInfo[edge.End] = new PathfindingInfo<T>(edge.Start, newEndDist);
                        }
                    }
                }
            }

            //Last pass. If a more optimal distance is found here, then there is a negative cycle
            foreach (Edge<T> edge in graph.Edges)
            {
                if (pathfindingInfo.TryGetValue(edge.Start, out PathfindingInfo<T> startVertInfo))
                {
                    double newEndDist = startVertInfo.Distance + edge.Distance;
                    if (pathfindingInfo[edge.End].Distance > newEndDist)
                    {
                        pathfindingInfo = null;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
