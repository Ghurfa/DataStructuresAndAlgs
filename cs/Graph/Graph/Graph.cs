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
     * Vertex.Edge stores both incoming and outgoing edges
     */
    public class Graph<T>
    {
        internal Dictionary<T, Vertex<T>> ValToVertex = new();
        internal List<Edge<T>> Edges = new();

        public Vertex<T> AddVertex(T value)
        {
            Vertex<T> newVert = new(value);
            AddVertex(newVert);
            return newVert;
        }

        public void AddVertex(Vertex<T> vertex)
        {
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
            if (vertex.Owner != this) return false;

            foreach (Edge<T> edge in vertex.Edges)
            {
                Vertex<T> other = edge.Start == vertex ? edge.End : edge.Start;
                other._edges.Remove(edge);
                Edges.Remove(edge);
                edge.Owner = null;
            }

            vertex.Owner = null;
            vertex._edges.Clear();
            return true;
        }

        public Edge<T> AddEdge(T startValue, T endValue, double distance)
        {
            _ = startValue ?? throw new ArgumentNullException(nameof(startValue));
            _ = endValue ?? throw new ArgumentNullException(nameof(endValue));
            Vertex<T> startVert = Search(startValue) ?? throw new ArgumentException("Start value not found in graph");
            Vertex<T> endVert = Search(endValue) ?? throw new ArgumentException("End value not found in graph");
            if (startVert == endVert) throw new ArgumentException("Cannot add an edge from a vertex to itself");

            Edge<T> newEdge = new(startVert, endVert, distance);
            startVert._edges.Add(newEdge);
            endVert._edges.Add(newEdge);
            Edges.Add(newEdge);
            newEdge.Owner = this;
            return newEdge;
        }

        public Edge<T> AddEdge(Vertex<T> startVert, Vertex<T> endVert, double distance)
        {
            if (startVert.Owner != this) throw new ArgumentException("Graph does not own start vertex");
            if (endVert.Owner != this) throw new ArgumentException("Graph does not own end vertex");
            if (startVert == endVert) throw new ArgumentException("Cannot add an edge from a vertex to itself");

            Edge<T> newEdge = new(startVert, endVert, distance);
            startVert._edges.Add(newEdge);
            endVert._edges.Add(newEdge);
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
            Edges.Add(edge);
            edge.Owner = this;
        }

        public bool RemoveEdge(Edge<T> edge)
        {
            if (edge.Owner != this) return false;

            edge.Start._edges.Remove(edge);
            edge.End._edges.Remove(edge);
            Edges.Remove(edge);
            return true;
        }

        public IEnumerable<Edge<T>> GetEdges(T startValue, T endValue)
        {
            _ = startValue ?? throw new ArgumentNullException(nameof(startValue));
            _ = endValue ?? throw new ArgumentNullException(nameof(endValue));
            Vertex<T> startVert = Search(startValue) ?? throw new ArgumentException("Start value not found in graph");
            Vertex<T> endVert = Search(endValue) ?? throw new ArgumentException("End value not found in graph");

            foreach (Edge<T> edge in startVert.OutgoingEdges)
            {
                if (edge.End == endVert)
                {
                    yield return edge;
                }
            }

            foreach (Edge<T> edge in endVert.OutgoingEdges)
            {
                if (edge.End == startVert)
                {
                    yield return edge;
                }
            }
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
