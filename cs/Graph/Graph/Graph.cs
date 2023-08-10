using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    /*
     * Duplicate vertex values not allowed
     * Multiple edges between the same nodes allowed
     * Edges from a node to itself not allowed
     * Vertex value cannot be null;
     */
    public class Graph<T>
    {
        internal Dictionary<T, Vertex<T>> ValToVertex;
        private List<Edge<T>> _edges;

        public Graph() 
        { 
            ValToVertex = new Dictionary<T, Vertex<T>>();
            _edges = new List<Edge<T>>();
        }

        public Graph(Graph<T> other) 
        { 
            ValToVertex = new Dictionary<T, Vertex<T>>();
            foreach(Vertex<T> vertex in other.ValToVertex.Values)
            {
                AddVertex(vertex.Value);
            }

            _edges = new List<Edge<T>>();
            foreach(Edge<T> edge in other._edges)
            {
                AddEdge(edge.Start.Value, edge.End.Value, edge.Distance);
            }
        }

        public IReadOnlyList<Edge<T>> Edges => _edges;

        public Vertex<T> AddVertex(T value)
        {
            Vertex<T> newVert = new(value);
            AddVertex(newVert);
            return newVert;
        }

        public void AddVertex(Vertex<T> vertex)
        {
            _ = vertex ?? throw new ArgumentNullException(nameof(vertex));
            if (vertex.Owner == this) throw new ArgumentException("Vertex already in graph");
            if(vertex.Owner != null) throw new ArgumentException("Vertex already has an owner");

            if(!ValToVertex.TryAdd(vertex.Value, vertex))
            {
                throw new ArgumentException("A vertex with that value already exists in the graph");
            }
            vertex.Owner = this;
        }

        public bool RemoveVertex(Vertex<T> vertex)
        {
            if (vertex == null || vertex.Owner != this) return false;

            foreach (Edge<T> edge in vertex.IncomingEdges)
            {
                edge.Start._edges.Remove(edge);
                _edges.Remove(edge);
                edge.Owner = null;
            }

            foreach (Edge<T> edge in vertex.OutgoingEdges)
            {
                edge.End._edges.Remove(edge);
                _edges.Remove(edge);
                edge.Owner = null;
            }

            vertex.Owner = null;
            vertex._edges.Clear();
            ValToVertex.Remove(vertex.Value);
            return true;
        }

        public Edge<T> AddEdge(T startValue, T endValue, double distance)
        {
            Vertex<T> startVert = Search(startValue) ?? throw new ArgumentException("Start value not found in graph");
            Vertex<T> endVert = Search(endValue) ?? throw new ArgumentException("End value not found in graph");

            return AddEdge(startVert, endVert, distance);
        }

        public Edge<T> AddEdge(Vertex<T> startVert, Vertex<T> endVert, double distance)
        {
            _ = startVert ?? throw new ArgumentNullException(nameof(startVert));
            _ = endVert ?? throw new ArgumentNullException(nameof(endVert));
            if (startVert.Owner != this) throw new ArgumentException("Graph does not own start vertex");
            if (endVert.Owner != this) throw new ArgumentException("Graph does not own end vertex");
            if (startVert == endVert) throw new ArgumentException("Cannot add an edge from a vertex to itself");

            Edge<T> newEdge = new(startVert, endVert, distance);
            startVert._edges.Add(newEdge);
            endVert._edges.Add(newEdge);
            _edges.Add(newEdge);
            newEdge.Owner = this;
            return newEdge;
        }

        public void AddEdge(Edge<T> edge)
        {
            if (edge.Owner == this) throw new ArgumentException("Graph already contains edge");
            if (edge.Owner != null) throw new ArgumentException("Edge already has an owner");
            if (edge.Start.Owner != this) throw new ArgumentException("Graph does not own start vertex");
            if (edge.End.Owner != this) throw new ArgumentException("Graph does not own end vertex");

            edge.Start._edges.Add(edge);
            edge.End._edges.Add(edge);
            _edges.Add(edge);
            edge.Owner = this;
        }

        public bool RemoveEdge(Edge<T> edge)
        {
            if (edge.Owner != this) return false;

            edge.Start._edges.Remove(edge);
            edge.End._edges.Remove(edge);
            _edges.Remove(edge);
            return true;
        }

        public IEnumerable<Edge<T>> GetEdges(T startValue, T endValue)
        {
            Vertex<T> startVert = Search(startValue) ?? throw new ArgumentException("Start value not found in graph");
            Vertex<T> endVert = Search(endValue) ?? throw new ArgumentException("End value not found in graph");
            return GetEdges(startVert, endVert);
        }

        public IEnumerable<Edge<T>> GetEdges(Vertex<T> startVertex, Vertex<T> endVertex)
        {
            if (startVertex.Owner != this) throw new ArgumentException("Graph does not own start vertex");
            if (endVertex.Owner != this) throw new ArgumentException("Graph does not own end vertex");

            foreach (Edge<T> edge in startVertex.OutgoingEdges)
            {
                if (edge.End == endVertex)
                {
                    yield return edge;
                }
            }

            foreach (Edge<T> edge in endVertex.OutgoingEdges)
            {
                if (edge.End == startVertex)
                {
                    yield return edge;
                }
            }
        }

        public Vertex<T>? Search(T value)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));
            if (ValToVertex.TryGetValue(value, out var vertex))
            {
                return vertex;
            }
            else return null;
        }
    }
}
