using System;
using System.Collections.Generic;

namespace UndirectedUnweightedGraph
{
    public class GraphVertex<TKey, TValue>
    {
        public readonly TValue Value;
        public readonly TKey Key;
        private LinkedList<GraphVertex<TKey, TValue>> neighbors;
        public GraphVertex(TKey key)
        {
            Key = key;
            Value = default(TValue);
            neighbors = new LinkedList<GraphVertex<TKey, TValue>>();
        }
        public GraphVertex(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            neighbors = new LinkedList<GraphVertex<TKey, TValue>>();
        }
        public void AddNeighbor(GraphVertex<TKey, TValue> vertex)
        {
            neighbors.AddLast(vertex);
        }
        public bool RemoveNeighbor(GraphVertex<TKey, TValue> vertex)
        {
            return neighbors.Remove(vertex);
        }
        public IEnumerable<GraphVertex<TKey, TValue>> Neighbors()
        {
            foreach (GraphVertex<TKey, TValue> neighbor in neighbors)
                yield return neighbor;
        }
        public bool Equals(GraphVertex<TKey, TValue> other) => Key.Equals(other.Key);
    }
    public class UndirectedUnweightedGraph<TKey, TValue>
    {
        public int Count { get; private set; }
        private GraphVertex<TKey, TValue>[] vertices;
        public UndirectedUnweightedGraph()
        {
            vertices = new GraphVertex<TKey, TValue>[0];
        }
        public void AddVertex(GraphVertex<TKey, TValue> vertex)
        {
            GraphVertex<TKey, TValue>[] temp = new GraphVertex<TKey, TValue>[vertices.Length + 1];
            for (int i = 0; i < vertices.Length; i++)
            {
                temp[i] = vertices[i];
                if (temp[i].Key.Equals(vertex.Key)) throw new ArgumentException();
            }
            temp[vertices.Length] = vertex;
            vertices = temp;
            Count++;
        }
        public bool RemoveVertex(TKey key)
        {
            GraphVertex<TKey, TValue>[] temp = new GraphVertex<TKey, TValue>[vertices.Length - 1];
            bool foundVertex = false;
            int i;
            for (i = 0; i < vertices.Length - 1; i++)
            {
                temp[i] = vertices[i];
                if (vertices[i].Key.Equals(key))
                {
                    foundVertex = true;
                    break;
                }
            }
            if (!foundVertex && !vertices[i].Key.Equals(key)) return false;
            Count--;
            if (Count > 0)
            {
                foreach (GraphVertex<TKey, TValue>  neighbor in temp[i].Neighbors())
                    neighbor.RemoveNeighbor(temp[i]);
                for (; i < vertices.Length - 1; i++)
                {
                    temp[i] = vertices[i + 1];
                }
            }
            vertices = temp;
            return true;
        }
        public void AddEdge(TKey a, TKey b)
        {
            if (a.Equals(b)) throw new ArgumentException();
            else if (a == null || b == null) throw new ArgumentNullException();
            GraphVertex<TKey, TValue> first = null;
            GraphVertex<TKey, TValue> second = null;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i].Key.Equals(a)) first = vertices[i];
                else if (vertices[i].Key.Equals(b)) second = vertices[i];
            }
            if (a == null || b == null) throw new ArgumentException();
            first.AddNeighbor(second);
            second.AddNeighbor(first);
        }
        public bool RemoveEdge(TKey a, TKey b)
        {
            if (a.Equals(b) || a == null || b == null) return false;
            GraphVertex<TKey, TValue> first = null;
            GraphVertex<TKey, TValue> second = null;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i].Key.Equals(a))
                {
                    first = vertices[i];
                }
                else if (vertices[i].Key.Equals(b))
                {
                    second = vertices[i];
                }
            }
            if (first == null || second == null) return false;
            return first.RemoveNeighbor(second) && second.RemoveNeighbor(first);
        }
        public IEnumerator<GraphVertex<TKey, TValue>> DepthFirstTraversal(TKey start)
        {
            GraphVertex<TKey, TValue> startVertex = null;
            foreach(GraphVertex<TKey, TValue> vertex in vertices)
                if(vertex.Key.Equals(start))
                {
                    startVertex = vertex;
                    break;
                }
            if (startVertex == null) throw new ArgumentNullException();
            GraphVertex<TKey, TValue>[] reachedVertices = new GraphVertex<TKey, TValue>[Count];
            Stack<GraphVertex<TKey, TValue>> stack = new Stack<GraphVertex<TKey, TValue>>();
            stack.Push(startVertex);
            int count = 0;
            while (stack.Count > 0)
            {
                GraphVertex<TKey, TValue> temp = stack.Pop();
                bool reached = false;
                for (int i = 0; i < count; i++)
                {
                    if (reachedVertices[i] == temp)
                    {
                        reached = true;
                        break;
                    }
                }
                if (reached) continue;
                yield return temp;
                reachedVertices[count] = temp;
                foreach (GraphVertex<TKey, TValue> neighbor in temp.Neighbors())
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (reachedVertices[i] == neighbor)
                        {
                            reached = true;
                            break;
                        }
                    }
                    if (!reached) stack.Push(neighbor);
                }
                count++;
            }
        }
        public IEnumerator<GraphVertex<TKey, TValue>> BreadthFirstTraversal(GraphVertex<TKey, TValue> start)
        {
            GraphVertex<TKey, TValue> startVertex = null;
            foreach (GraphVertex<TKey, TValue> vertex in vertices)
                if (vertex.Key.Equals(start))
                {
                    startVertex = vertex;
                    break;
                }
            if (startVertex == null) throw new ArgumentNullException();
            GraphVertex<TKey, TValue>[] reachedVertices = new GraphVertex<TKey, TValue>[Count];
            Queue<GraphVertex<TKey, TValue>> queue = new Queue<GraphVertex<TKey, TValue>>();
            queue.Enqueue(startVertex);
            int count = 0;
            while (queue.Count > 0)
            {
                GraphVertex<TKey, TValue> temp = queue.Dequeue();
                bool reached = false;
                for (int i = 0; i < count; i++)
                {
                    if (reachedVertices[i] == temp)
                    {
                        reached = true;
                        break;
                    }
                }
                if (reached) continue;
                yield return temp;
                reachedVertices[count] = temp;
                foreach (GraphVertex<TKey, TValue> neighbor in temp.Neighbors())
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (reachedVertices[i] == neighbor)
                        {
                            reached = true;
                            break;
                        }
                    }
                    if (!reached) queue.Enqueue(neighbor);
                }
                count++;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            UndirectedUnweightedGraph<int, int> graph = new UndirectedUnweightedGraph<int, int>();
            Console.WriteLine("Type 'hlp' for help");
            Dictionary<int, GraphVertex<int, int>> vertices = new Dictionary<int, GraphVertex<int, int>>();
            int count = 0;
            while (true)
            {
                string command = Console.ReadLine();
                if (command.Length < 3) continue;
                int value;
                int key1;
                int key2;
                switch (command.ToLower().Substring(0, 3))
                {
                    case "adv":
                        Console.WriteLine("Value of vertex: ");
                        value = int.Parse(Console.ReadLine());
                        Console.WriteLine("Key of vertex: ");
                        key1 = int.Parse(Console.ReadLine());
                        graph.AddVertex(new GraphVertex<int, int>(key1, value));
                        vertices.Add(key1, new GraphVertex<int, int>(key1, value));
                        Console.WriteLine($"Added vertex with value {value}");
                        count++;
                        break;
                    case "dev":
                        Console.WriteLine("Key of vertex: ");
                        key1 = int.Parse(Console.ReadLine());
                        if (graph.RemoveVertex(key1))
                        {
                            Console.WriteLine($"Deleting vertex {key1} successful");
                            foreach (GraphVertex<int, int> neighbor in vertices[key1].Neighbors())
                                neighbor.RemoveNeighbor(vertices[key1]);
                            vertices.Remove(key1);
                        }
                        else Console.WriteLine($"Deleting vertex {key1} unsuccessful");
                        break;
                    case "ade":
                        Console.WriteLine("Key of vertex 1: ");
                        key1 = int.Parse(Console.ReadLine());
                        Console.WriteLine("Key of vertex 2: ");
                        key2 = int.Parse(Console.ReadLine());
                        graph.AddEdge(key1, key2);
                        vertices[key1].AddNeighbor(vertices[key2]);
                        vertices[key2].AddNeighbor(vertices[key1]);
                        break;
                    case "dee":
                        Console.WriteLine("Key of vertex 1: ");
                        key1 = int.Parse(Console.ReadLine());
                        Console.WriteLine("Key of vertex 2: ");
                        key2 = int.Parse(Console.ReadLine());
                        if (graph.RemoveVertex(key1))
                        {
                            Console.WriteLine($"Deleting edge successful");
                            vertices[key1].RemoveNeighbor(vertices[key2]);
                            vertices[key2].RemoveNeighbor(vertices[key1]);
                        }
                        else Console.WriteLine($"Deleting edge unsuccessful");
                        break;
                    case "dft":
                        Console.WriteLine("Key of start vertex");
                        key1 = int.Parse(Console.ReadLine());
                        foreach(GraphVertex<int, int> vertex in graph.DepthFirstTraversal(key1))
                        {

                        }
                        break;
                    case "bft":
                        break;
                    case "svl":
                        foreach (KeyValuePair<int, GraphVertex<int, int>> pair in vertices)
                        {
                            Console.Write(pair.Key + ": ");
                            foreach (GraphVertex<int, int> neighbor in pair.Value.Neighbors())
                            {
                                Console.Write(neighbor.Key);
                            }
                            Console.WriteLine();
                        }
                        break;
                    case "hlp":
                        Console.WriteLine("Type Command: Add Vertex [inv], Delete Vertex[dev], Add Edge [ine], Delete Edge [dee], Depth First Traversal [dft], Breadth First Traversal [bft], Show vertex list [svl]");
                        break;
                }
            }
        }
    }
}
