using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public static class GraphAlgorithms
    {
        public static bool HasNegativeCycle<T>(Graph<T> graph)
        {
            Queue<Vertex<T>> startVertices = new(graph.ValToVertex.Values);

            //Try using all possible start vertices (necessary for disconnected graphs)
            while(startVertices.TryDequeue(out Vertex<T> startVertex))
            {
                Dictionary<Vertex<T>, double> distances = new();
                foreach(var vertex in graph.ValToVertex.Values)
                {
                    distances.Add(vertex, double.PositiveInfinity);
                }

                distances[startVertex] = 0;

                //Relax all edges V - 1 times
                for(int i = 0; i < graph.ValToVertex.Count - 1; i++)
                {
                    foreach(Edge<T> edge in graph.Edges)
                    {
                        if (double.IsPositiveInfinity(distances[edge.Start])) continue;

                        double newEndDist = distances[edge.Start] + edge.Distance;
                        if (distances[edge.End] > newEndDist)
                        {
                            distances[edge.End] = newEndDist;
                        }
                    }
                }

                //Last pass. If a more optimal distance is found here, then there is a negative cycle
                foreach (Edge<T> edge in graph.Edges)
                {
                    if (double.IsPositiveInfinity(distances[edge.Start])) continue;

                    double newEndDist = distances[edge.Start] + edge.Distance;
                    if (distances[edge.End] > newEndDist)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
