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
        public static Dictionary<Vertex<T>, PathfindingInfo<T>> Dijkstra<T>(Graph<T> graph, Vertex<T> start)
        {
            _ = graph ?? throw new ArgumentNullException(nameof(graph));
            _ = start ?? throw new ArgumentNullException(nameof(start));
            if (start.Owner != graph) throw new ArgumentException("Start vertex is not in graph");

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
        public static bool BellmanFord<T>(Graph<T> graph, Vertex<T> start, out Dictionary<Vertex<T>, PathfindingInfo<T>> pathfindingInfo)
        {
            _ = graph ?? throw new ArgumentNullException(nameof(graph));
            _ = start ?? throw new ArgumentNullException(nameof(start));
            if (start.Owner != graph) throw new ArgumentException("Start vertex is not in graph");

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
