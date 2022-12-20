using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class Edge<T>
    {
        public Vertex<T> Start { get; init; }
        public Vertex<T> End { get; init; }
        public Graph<T>? Owner { get; internal set; }
        public double Distance { get; init; }

        public Edge(Vertex<T> start, Vertex<T> end, double distance)
        {
            if (start == null || end == null) throw new ArgumentNullException("Endpoint is null");
            if (start.Owner != end.Owner) throw new ArgumentException("Vertices have mismatching owners");
            if (start == end) throw new ArgumentException("Cannot add an edge from a vertex to itself");

            Start = start;
            End = end;
            Distance = distance;
        }
    }
}
