using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    internal class MathStuff
    {
        public static bool IsEulerian(Graph<int> graph)
        {
            foreach (Vertex<int> vert in graph.ValToVertex.Values)
            {
                if (vert.Edges.Count() % 4 != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static int ComponentCount(Graph<int> graph)
        {
            int componentCount = 0;

            HashSet<Vertex<int>> vertices = new HashSet<Vertex<int>>(graph.ValToVertex.Values);
            while(vertices.Count > 0)
            {
                Vertex<int> start = vertices.First();
                Dictionary<Vertex<int>, PathfindingInfo<int>> dijkstraResult = GraphAlgorithms.Dijkstra(start);
                foreach(Vertex<int> vert in dijkstraResult.Keys)
                {
                    vertices.Remove(vert);
                }
                componentCount++;
            }

            return componentCount;
        }

        public static bool IsTough(Graph<int> graph)
        {
            for(int i = 0; i < (int)Math.Pow(2, graph.ValToVertex.Count) - 1; i++)
            {
                Graph<int> copy = new Graph<int>(graph);
                // Determine which vertices to remove
                int removedCount = 0;
                int iCopy = i;
                for(int j = 0; j < graph.ValToVertex.Count; j++)
                {
                    if((iCopy & 1) == 0)
                    {
                        copy.RemoveVertex(copy.ValToVertex[j]);
                        removedCount++;
                    }
                    iCopy >>= 1;
                }

                if(ComponentCount(copy) > removedCount)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
