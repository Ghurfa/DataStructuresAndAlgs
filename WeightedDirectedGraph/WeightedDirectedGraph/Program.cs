using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightedDirectedGraph
{
    class PathFindNode<TVertex>
    {
        public readonly PathFindNode<TVertex> Parent;
        public readonly TVertex Vertex;
        public PathFindNode(PathFindNode<TVertex> parent, TVertex vertex)
        {
            Parent = parent;
            Vertex = vertex;
        }
    }
    class PathFindTree<TVertex>
    {
        LinkedList<PathFindNode<TVertex>> vertices;
        public PathFindTree()
        {
            vertices = new LinkedList<PathFindNode<TVertex>>();
        }
        public static Stack<TVertex> GetPath(PathFindNode<TVertex> node)
        {
            var stack = new Stack<TVertex>();
            while (node != null)
            {
                stack.Push(node.Vertex);
                node = node.Parent;
            }
            return stack;
        }
        public bool Contains(TVertex vertexToFind)
        {
            foreach (PathFindNode<TVertex> vertex in vertices)
                if (vertex.Vertex.Equals(vertexToFind)) return true;
            return false;
        }
        public PathFindNode<TVertex> Add(TVertex vertexToAdd, TVertex parent)
        {
            PathFindNode<TVertex> parentNode = null;
            foreach (PathFindNode<TVertex> vertex in vertices)
            {
                if (vertex.Vertex.Equals(parent))
                {
                    parentNode = vertex;
                    break;
                }
            }
            PathFindNode<TVertex> newVertex = new PathFindNode<TVertex>(parentNode, vertexToAdd);
            vertices.AddFirst(newVertex);
            return vertices.First.Value;
        }
    }

    public class PriorityQueue<TPriority, TItem> : IEnumerable<KeyValuePair<TPriority, TItem>> where TPriority : IComparable<TPriority>
    {
        KeyValuePair<TPriority, TItem>[] values;
        public bool IsEmpty => values.Length == 0;
        public PriorityQueue()
        {
            values = new KeyValuePair<TPriority, TItem>[0];
        }
        public void Enqueue(TItem item, TPriority priority)
        {
            void heapifyUp(int index)
            {
                while (index > 0)
                {
                    int parentIndex = parent(index);

                    if (values[index].Key.CompareTo(values[parentIndex].Key) < 0)
                    {
                        swapValues(index, parentIndex);
                        index = parentIndex;
                    }
                    else return;
                }
            }
            KeyValuePair<TPriority, TItem>[] oldValues = values;
            values = new KeyValuePair<TPriority, TItem>[values.Length + 1];
            for (int i = 0; i < oldValues.Length; i++)
            {
                values[i] = oldValues[i];
            }
            values[oldValues.Length] = new KeyValuePair<TPriority, TItem>(priority, item);
            heapifyUp(oldValues.Length);
        }
        public KeyValuePair<TPriority, TItem> Dequeue()
        {
            void heapifyDown(int index)
            {
                int left = leftChild(index);
                int right = left + 1;
                while (left < values.Length)
                {
                    if (right < values.Length)
                    {
                        if (values[right].Key.CompareTo(values[left].Key) < 0 && values[right].Key.CompareTo(values[index].Key) < 0)
                        {
                            swapValues(right, index);
                            index = right;
                        }
                        else if (values[left].Key.CompareTo(values[index].Key) < 0)
                        {
                            swapValues(left, index);
                            index = left;
                        }
                        else return;
                    }
                    else if (values[left].Key.CompareTo(values[index].Key) < 0)
                    {
                        swapValues(left, index);
                        index = left;
                    }
                    else return;
                    left = leftChild(index);
                    right = left + 1;
                }
            }
            if (IsEmpty) throw new InvalidOperationException("Cannot pop from empty heap");
            KeyValuePair<TPriority, TItem> rootValue = values[0];
            values[0] = values[values.Length - 1];
            var newValues = new KeyValuePair<TPriority, TItem>[values.Length - 1];
            for (int i = 0; i < newValues.Length; i++)
                newValues[i] = values[i];
            values = newValues;
            heapifyDown(0);
            return rootValue;
        }
        public void UpdatePriority(TItem item, TPriority newPriority)
        {
            for (int i = 0; i < values.Length; i++)
                if (values[i].Value.Equals(item))
                {
                    if (newPriority.CompareTo(values[i].Key) < 0)
                    {
                        values[i] = new KeyValuePair<TPriority, TItem>(newPriority, item);
                    }
                    break;
                }
        }
        private int parent(int index) => (index - 1) / 2;
        private int leftChild(int index) => 2 * index + 1;
        private int rightChild(int index) => 2 * index + 2;
        private void swapValues(int firstIndex, int secondIndex)
        {
            var firstValue = values[firstIndex];
            values[firstIndex] = values[secondIndex];
            values[secondIndex] = firstValue;
        }

        public IEnumerator<KeyValuePair<TPriority, TItem>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TPriority, TItem>>)values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TPriority, TItem>>)values).GetEnumerator();
        }
    }
    class GraphEdge<TKey, TValue>
    {
        public readonly GraphVertex<TKey, TValue> StartVertex;
        public readonly GraphVertex<TKey, TValue> EndVertex;
        public int Cost;
        public GraphEdge(GraphVertex<TKey, TValue> start, GraphVertex<TKey, TValue> end, int cost)
        {
            StartVertex = start;
            EndVertex = end;
            Cost = cost;
        }
        public bool IsStartVertex(GraphVertex<TKey, TValue> vertex) => vertex == StartVertex;
        public bool ConnectsToVertex(GraphVertex<TKey, TValue> vertex) =>
            StartVertex == vertex || EndVertex == vertex;
        public bool MatchesEdge(GraphEdge<TKey, TValue> other) =>
            StartVertex == other.StartVertex && EndVertex == other.EndVertex;
    }
    class GraphVertex<TKey, TValue>
    {
        public readonly TKey Key;
        public readonly TValue Value;
        public LinkedList<GraphEdge<TKey, TValue>> Edges;
        public GraphVertex(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Edges = new LinkedList<GraphEdge<TKey, TValue>>();
        }
        public bool Equals(GraphVertex<TKey, TValue> other) => Key.Equals(other.Key);
    }
    class WeightedDirectedGraph<TKey, TValue> : IEnumerable<GraphVertex<TKey, TValue>>
    {
        private Dictionary<TKey, GraphVertex<TKey, TValue>> vertices;
        private LinkedList<GraphEdge<TKey, TValue>> edges;
        public GraphVertex<TKey, TValue> this[TKey key] => vertices[key];
        public int VertexCount { get; private set; }
        public WeightedDirectedGraph()
        {
            vertices = new Dictionary<TKey, GraphVertex<TKey, TValue>>();
            edges = new LinkedList<GraphEdge<TKey, TValue>>();
            VertexCount = 0;
        }
        public void AddVertex(TKey key, TValue value)
        {
            vertices.Add(key, new GraphVertex<TKey, TValue>(key, value));
        }
        public bool RemoveVertex(TKey key)
        {
            GraphVertex<TKey, TValue> deletedVertex = vertices[key];
            if (deletedVertex == null) return false;
            vertices.Remove(key);
            foreach (GraphEdge<TKey, TValue> edge in edges)
            {
                if (edge.ConnectsToVertex(deletedVertex)) edges.Remove(edge);
            }
            return true;
        }
        public void AddEdge(TKey startKey, TKey endKey, int cost)
        {
            if (startKey == null || endKey == null) throw new ArgumentNullException();
            else if (startKey.Equals(endKey)) throw new ArgumentException();
            GraphVertex<TKey, TValue> startVertex = null;
            GraphVertex<TKey, TValue> endVertex = null;
            foreach (KeyValuePair<TKey, GraphVertex<TKey, TValue>> pair in vertices)
            {
                if (pair.Key.Equals(startKey))
                {
                    startVertex = pair.Value;
                    if (endVertex != null) break;
                }
                else if (pair.Key.Equals(endKey))
                {
                    endVertex = pair.Value;
                    if (startVertex != null) break;
                }
            }
            if (startVertex == null || endVertex == null) throw new ArgumentException();
            GraphEdge<TKey, TValue> edge = new GraphEdge<TKey, TValue>(startVertex, endVertex, cost);
            foreach(GraphEdge<TKey, TValue> connectingEdge in startVertex.Edges)
                if(connectingEdge.MatchesEdge(edge)) throw new ArgumentException();
            edges.AddLast(edge);
            startVertex.Edges.AddLast(edge);
            endVertex.Edges.AddLast(edge);
        }
        public bool RemoveEdge(TKey startKey, TKey endKey)
        {
            if (startKey == null || endKey == null) return false;
            else if (startKey.Equals(endKey)) return false;
            GraphVertex<TKey, TValue> startVertex = null;
            GraphVertex<TKey, TValue> endVertex = null;
            foreach (KeyValuePair<TKey, GraphVertex<TKey, TValue>> pair in vertices)
            {
                if (pair.Key.Equals(startKey))
                {
                    startVertex = pair.Value;
                    if (endVertex != null) break;
                }
                else if (pair.Key.Equals(endKey))
                {
                    endVertex = pair.Value;
                    if (startVertex != null) break;
                }
            }
            if (startVertex == null || endVertex == null) throw new ArgumentException();
            GraphEdge<TKey, TValue> edgeToRemove = new GraphEdge<TKey, TValue>(startVertex, endVertex, 0);
            bool deleted = false;
            foreach (GraphEdge<TKey, TValue> edge in edges)
            {
                if (edge.MatchesEdge(edgeToRemove))
                {
                    edges.Remove(edge);
                    deleted = true;
                    break;
                }
            }
            if (!deleted) return false;
            foreach (GraphEdge<TKey, TValue> edge in startVertex.Edges)
            {
                if (edge.MatchesEdge(edgeToRemove))
                {
                    startVertex.Edges.Remove(edge);
                    break;
                }
            }
            foreach (GraphEdge<TKey, TValue> edge in endVertex.Edges)
            {
                if (edge.MatchesEdge(edgeToRemove))
                {
                    endVertex.Edges.Remove(edge);
                    return true;
                }
            }
            return true;
        }
        public Stack<GraphVertex<TKey, TValue>> DjikstraPathFind(TKey startKey, TKey endKey)
        {
            if (startKey == null || endKey == null) throw new ArgumentNullException();
            else if (startKey.Equals(endKey)) throw new ArgumentException();
            GraphVertex<TKey, TValue> startVertex = null;
            GraphVertex<TKey, TValue> endVertex = null;
            foreach (KeyValuePair<TKey, GraphVertex<TKey, TValue>> pair in vertices)
            {
                if (pair.Key.Equals(startKey))
                {
                    startVertex = pair.Value;
                    if (endVertex != null) break;
                }
                else if (pair.Key.Equals(endKey))
                {
                    endVertex = pair.Value;
                    if (startVertex != null) break;
                }
            }
            if (startVertex == null || endVertex == null) throw new ArgumentException();
            var priorityQueue = new PriorityQueue<int, (GraphVertex<TKey, TValue> parent, GraphVertex<TKey, TValue> vertex)>();
            PathFindTree<GraphVertex<TKey, TValue>> reachedVertices = new PathFindTree<GraphVertex<TKey, TValue>>();
            PathFindNode<GraphVertex<TKey, TValue>> endNode = null;
            priorityQueue.Enqueue((null, startVertex), 0);
            while (endNode == null)
            {
                KeyValuePair<int, (GraphVertex<TKey, TValue> parent, GraphVertex<TKey, TValue> vertex)> pair = priorityQueue.Dequeue();
                PathFindNode<GraphVertex<TKey, TValue>> node = reachedVertices.Add(pair.Value.vertex, pair.Value.parent);
                if (node.Vertex.Equals(endVertex))
                {
                    endNode = node;
                    break;
                }
                foreach (GraphEdge<TKey, TValue> edge in pair.Value.vertex.Edges)
                {
                    if (edge.EndVertex.Equals(pair.Value.vertex)) continue;
                    if (reachedVertices.Contains(edge.EndVertex))
                    {
                        priorityQueue.UpdatePriority((pair.Value.vertex, edge.EndVertex), pair.Key + edge.Cost);
                    }
                    else
                    {
                        priorityQueue.Enqueue((pair.Value.vertex, edge.EndVertex), pair.Key + edge.Cost);
                    }
                }
            }
            return PathFindTree<GraphVertex<TKey, TValue>>.GetPath(endNode);
        }
        public IEnumerator<GraphVertex<TKey, TValue>> GetEnumerator()
        {
            foreach (GraphVertex<TKey, TValue> vertex in vertices.Values)
            {
                yield return vertex;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (GraphVertex<TKey, TValue> vertex in vertices.Values)
            {
                yield return vertex;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            WeightedDirectedGraph<int, int> graph = new WeightedDirectedGraph<int, int>();
            Console.WriteLine("Type 'hlp' for help");
            int count = 0;
            graph.AddVertex(0, 0);
            graph.AddVertex(1, 1);
            graph.AddVertex(2, 2);
            graph.AddVertex(3, 3);
            graph.AddVertex(4, 4);
            graph.AddVertex(5, 5);
            graph.AddEdge(0, 1, 5);
            graph.AddEdge(0, 2, 7);
            graph.AddEdge(2, 4, 2);
            graph.AddEdge(4, 2, -10);
            graph.AddEdge(1, 4, 9);
            graph.AddEdge(1, 3, 10);
            graph.AddEdge(2, 5, 3);
            graph.AddEdge(5, 3, 4);
            graph.AddEdge(4, 3, 8);
            while (true)
            {
                string command = Console.ReadLine();
                if (command.Length < 3) continue;
                switch (command.ToLower().Substring(0, 3))
                {
                    case "adv":
                        {
                            Console.WriteLine("Value of vertex: ");
                            int value = int.Parse(Console.ReadLine());
                            Console.WriteLine("Key of vertex: ");
                            int key1 = int.Parse(Console.ReadLine());
                            graph.AddVertex(key1, value);
                            Console.WriteLine($"Added vertex with value {value}");
                            count++;
                            break;
                        }
                    case "dev":
                        {
                            Console.WriteLine("Key of vertex: ");
                            int key1 = int.Parse(Console.ReadLine());
                            if (graph.RemoveVertex(key1)) Console.WriteLine($"Deleting vertex {key1} successful");
                            else Console.WriteLine($"Deleting vertex {key1} unsuccessful");
                            break;
                        }
                    case "ade":
                        {
                            Console.WriteLine("Key of start vertex ");
                            int startKey = int.Parse(Console.ReadLine());
                            Console.WriteLine("Key of end vertex: ");
                            int endKey = int.Parse(Console.ReadLine());
                            Console.WriteLine("Cost of edge: ");
                            int cost = int.Parse(Console.ReadLine());
                            graph.AddEdge(startKey, endKey, cost);
                            break;
                        }
                    case "dee":
                        {
                            Console.WriteLine("Key of vertex 1: ");
                            int key1 = int.Parse(Console.ReadLine());
                            Console.WriteLine("Key of vertex 2: ");
                            int key2 = int.Parse(Console.ReadLine());
                            if (graph.RemoveEdge(key1, key2)) Console.WriteLine($"Deleting edge successful");
                            else Console.WriteLine($"Deleting edge unsuccessful");
                            break;
                        }
                    case "svl":
                        foreach (GraphVertex<int, int> vertex in graph)
                        {
                            Console.Write($"{vertex.Key}: ");
                            foreach (GraphEdge<int, int> connection in vertex.Edges)
                            {
                                if (connection.IsStartVertex(vertex)) Console.Write($"{connection.EndVertex.Key} ({connection.Cost}), ");
                            }
                            Console.WriteLine();
                        }
                        break;
                    case "djk":
                        {
                            Console.WriteLine("Key of start vertex ");
                            int startKey = int.Parse(Console.ReadLine());
                            Console.WriteLine("Key of end vertex: ");
                            int endKey = int.Parse(Console.ReadLine());
                            Stack<GraphVertex<int, int>> path = graph.DjikstraPathFind(startKey, endKey);
                            Console.Write("Path: ");
                            while (path.Count > 0)
                                Console.Write(path.Pop().Key + "-");
                            Console.WriteLine();
                        }
                        break;
                    case "hlp":
                        Console.WriteLine("Type Command: Add Vertex [adv], Delete Vertex[dev], Add Edge [ade], Delete Edge [dee], Depth First Traversal [dft], Breadth First Traversal [bft], Show vertex list [svl]");
                        break;
                }
            }
        }
    }
}
