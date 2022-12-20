using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class Vertex<T>
    {
        public T Value { get; private set; }

        internal List<Edge<T>> _edges { get; private set; }
        public IEnumerable<Edge<T>> Edges => _edges;
        public IEnumerable<Edge<T>> OutgoingEdges => _edges.Where(x => x.Start == this);
        public IEnumerable<Edge<T>> IncomingEdges => _edges.Where(x => x.End == this);

        public Graph<T>? Owner { get; internal set; }

        public Vertex(T value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            _edges = new List<Edge<T>>();
            Owner = null;
        }
    }
}
