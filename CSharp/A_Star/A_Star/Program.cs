using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Star
{
    enum SearchType
    {
        AStarManhatten,
        AStarDiagonal,
        BestFirstManhatten
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
            KeyValuePair<TPriority, TItem>[] oldValues = values;
            values = new KeyValuePair<TPriority, TItem>[values.Length + 1];
            for (int i = 0; i < oldValues.Length; i++)
            {
                values[i] = oldValues[i];
            }
            values[oldValues.Length] = new KeyValuePair<TPriority, TItem>(priority, item);
            heapifyUp(oldValues.Length);
        }
        public KeyValuePair<TPriority, TItem> DequeueKeyValue()
        {
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
        public TItem DequeueValue()
        {
            if (IsEmpty) throw new InvalidOperationException("Cannot pop from empty heap");
            TItem rootValue = values[0].Value;
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
                    return;
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
        private void heapifyUp(int index)
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
        private void heapifyDown(int index)
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
        public IEnumerator<KeyValuePair<TPriority, TItem>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TPriority, TItem>>)values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TPriority, TItem>>)values).GetEnumerator();
        }
    }
    class Node
    {
        public readonly int X;
        public readonly int Y;
        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    class Wall : Node
    {
        public Wall(int x, int y) : base(x, y) { }
    }
    class GraphVertex : Node
    {
        public Dictionary<GraphVertex, int> Neighbors { get; private set; }
        public GraphVertex(int x, int y) : base(x, y)
        {
            Neighbors = new Dictionary<GraphVertex, int>();
        }
        public GraphVertex(int x, int y, Dictionary<GraphVertex, int> neighbors) : base(x, y)
        {
            Neighbors = neighbors;
        }
        public void AddNeighbor(GraphVertex newNeighbor, int cost)
        {
            Neighbors.Add(newNeighbor, cost);
        }
        public int ManhattenDist(GraphVertex endNode)
        {
            int x = Math.Abs(endNode.X - X);
            int y = Math.Abs(endNode.Y - Y);
            return x + y;
        }
    }
    class SearchNode
    {
        public readonly GraphVertex Vertex;
        public readonly SearchNode Founder;
        public readonly int ManhattenPriority;
        private int knownDist;
        public SearchNode(GraphVertex vertex, SearchNode founder, int distFromFounder, GraphVertex endVertex)
        {
            Vertex = vertex;
            Founder = founder;
            if (Founder == null)
            {
                knownDist = 0;
            }
            else
            {
                knownDist = Founder.knownDist + distFromFounder;
            }
            ManhattenPriority = knownDist + Vertex.ManhattenDist(endVertex);
        }
    }
    class Graph
    {
        Node[,] nodes;
        public Graph(Node[,] nodes)
        {
            this.nodes = nodes;
        }
        public Stack<GraphVertex> FindPath(int startVertexX, int startVertexY, int endVertexX, int endVertexY, SearchType searchType)
        {
            return FindPath((GraphVertex)nodes[startVertexX, startVertexY], (GraphVertex)nodes[endVertexX, endVertexY], searchType);
        }
        public Stack<GraphVertex> FindPath(GraphVertex startVertex, GraphVertex endVertex, SearchType searchType)
        {
            PriorityQueue<int, SearchNode> queue = new PriorityQueue<int, SearchNode>();
            bool[,] visitedMarkers = new bool[nodes.GetLength(0), nodes.GetLength(1)];
            queue.Enqueue(new SearchNode(startVertex, null, 0, endVertex), 0);
            visitedMarkers[startVertex.X, startVertex.Y] = true;
            SearchNode endSearchNode = null;
            while (endSearchNode == null)
            {
                if (queue.IsEmpty) throw new InvalidOperationException("No possible path");
                SearchNode currentSearchNode = queue.DequeueValue();
                if (currentSearchNode.Vertex.Equals(endVertex))
                {
                    endSearchNode = currentSearchNode;
                    break;
                }
                visitedMarkers[currentSearchNode.Vertex.X, currentSearchNode.Vertex.Y] = true;
                switch (searchType)
                {
                    case SearchType.AStarManhatten:
                    case SearchType.AStarDiagonal:
                    case SearchType.BestFirstManhatten:
                        if (searchType == SearchType.AStarDiagonal)
                        {
                            for (int x = currentSearchNode.Vertex.X - 1, y = currentSearchNode.Vertex.Y - 1;
                                x >= 0 && y >= 0;
                                x--, y--)
                            {
                                Node potentialNeighbor = nodes[x, y];
                                if (potentialNeighbor == null) continue;
                                if (visitedMarkers[potentialNeighbor.X, potentialNeighbor.Y] || potentialNeighbor.GetType() == typeof(Wall)) break;
                                if (potentialNeighbor.GetType() == typeof(GraphVertex))
                                {
                                    SearchNode upperLeftNeighbor = new SearchNode((GraphVertex)potentialNeighbor, currentSearchNode, 1, endVertex);
                                    queue.Enqueue(upperLeftNeighbor, upperLeftNeighbor.ManhattenPriority);
                                    break;
                                }
                            } //Upper Left Neighbor
                            for (int x = currentSearchNode.Vertex.X + 1, y = currentSearchNode.Vertex.Y - 1;
                                x < nodes.GetLength(0) && y >= 0;
                                x++, y--)
                            {
                                Node potentialNeighbor = nodes[x, y];
                                if (potentialNeighbor == null) continue;
                                if (visitedMarkers[potentialNeighbor.X, potentialNeighbor.Y] || potentialNeighbor.GetType() == typeof(Wall)) break;
                                if (potentialNeighbor.GetType() == typeof(GraphVertex))
                                {
                                    SearchNode upperLeftNeighbor = new SearchNode((GraphVertex)potentialNeighbor, currentSearchNode, 1, endVertex);
                                    queue.Enqueue(upperLeftNeighbor, upperLeftNeighbor.ManhattenPriority);
                                    break;
                                }
                            } //Upper Right Neighbor
                            for (int x = currentSearchNode.Vertex.X - 1, y = currentSearchNode.Vertex.Y + 1;
                                x >= 0 && y < nodes.GetLength(1);
                                x--, y++)
                            {
                                Node potentialNeighbor = nodes[x, y];
                                if (potentialNeighbor == null) continue;
                                if (visitedMarkers[potentialNeighbor.X, potentialNeighbor.Y] || potentialNeighbor.GetType() == typeof(Wall)) break;
                                if (potentialNeighbor.GetType() == typeof(GraphVertex))
                                {
                                    SearchNode upperLeftNeighbor = new SearchNode((GraphVertex)potentialNeighbor, currentSearchNode, 1, endVertex);
                                    queue.Enqueue(upperLeftNeighbor, upperLeftNeighbor.ManhattenPriority);
                                    break;
                                }
                            } //Lower Left Neighbor
                            for (int x = currentSearchNode.Vertex.X + 1, y = currentSearchNode.Vertex.Y + 1;
                                x < nodes.GetLength(0) && y < nodes.GetLength(1);
                                x++, y++)
                            {
                                Node potentialNeighbor = nodes[x, y];
                                if (potentialNeighbor == null) continue;
                                if (visitedMarkers[potentialNeighbor.X, potentialNeighbor.Y] || potentialNeighbor.GetType() == typeof(Wall)) break;
                                if (potentialNeighbor.GetType() == typeof(GraphVertex))
                                {
                                    SearchNode upperLeftNeighbor = new SearchNode((GraphVertex)potentialNeighbor, currentSearchNode, 1, endVertex);
                                    queue.Enqueue(upperLeftNeighbor, upperLeftNeighbor.ManhattenPriority);
                                    break;
                                }
                            } //Lower Right Neighbor
                        }
                        for (int x = currentSearchNode.Vertex.X - 1; x >= 0; x--)
                        {
                            Node potentialNeighbor = nodes[x, currentSearchNode.Vertex.Y];
                            if (potentialNeighbor == null) continue;
                            if (visitedMarkers[potentialNeighbor.X, potentialNeighbor.Y] || potentialNeighbor.GetType() == typeof(Wall)) break;
                            if (potentialNeighbor.GetType() == typeof(GraphVertex))
                            {
                                SearchNode leftNeighbor = new SearchNode((GraphVertex)potentialNeighbor, currentSearchNode, 1, endVertex);
                                if(searchType == SearchType.BestFirstManhatten)
                                {
                                    queue.Enqueue(leftNeighbor, currentSearchNode.Vertex.ManhattenDist(endVertex));
                                }
                                else
                                {
                                    queue.Enqueue(leftNeighbor, leftNeighbor.ManhattenPriority);
                                }
                                break;
                            }
                        } //Left Neighbor
                        for (int x = currentSearchNode.Vertex.X + 1; x < nodes.GetLength(0); x++)
                        {
                            Node potentialNeighbor = nodes[x, currentSearchNode.Vertex.Y];
                            if (potentialNeighbor == null) continue;
                            if (visitedMarkers[potentialNeighbor.X, potentialNeighbor.Y] || potentialNeighbor.GetType() == typeof(Wall)) break;
                            if (potentialNeighbor.GetType() == typeof(GraphVertex))
                            {
                                SearchNode rightNeighbor = new SearchNode((GraphVertex)potentialNeighbor, currentSearchNode, 1, endVertex);
                                if (searchType == SearchType.BestFirstManhatten)
                                {
                                    queue.Enqueue(rightNeighbor, rightNeighbor.Vertex.ManhattenDist(endVertex));
                                }
                                else
                                {
                                    queue.Enqueue(rightNeighbor, rightNeighbor.ManhattenPriority);
                                }
                                break;
                            }
                        } //Right Neighbor
                        for (int y = currentSearchNode.Vertex.Y - 1; y >= 0; y--)
                        {
                            Node potentialNeighbor = nodes[currentSearchNode.Vertex.X, y];
                            if (potentialNeighbor == null) continue;
                            if (visitedMarkers[potentialNeighbor.X, potentialNeighbor.Y] || potentialNeighbor.GetType() == typeof(Wall)) break;
                            if (potentialNeighbor.GetType() == typeof(GraphVertex))
                            {
                                SearchNode upNeighbor = new SearchNode((GraphVertex)potentialNeighbor, currentSearchNode, 1, endVertex);
                                if (searchType == SearchType.BestFirstManhatten)
                                {
                                    queue.Enqueue(upNeighbor, currentSearchNode.Vertex.ManhattenDist(endVertex));
                                }
                                else
                                {
                                    queue.Enqueue(upNeighbor, upNeighbor.ManhattenPriority);
                                }
                                break;
                            }
                        } //Up Neighbor
                        for (int y = currentSearchNode.Vertex.Y + 1; y < nodes.GetLength(1); y++)
                        {
                            Node potentialNeighbor = nodes[currentSearchNode.Vertex.X, y];
                            if (potentialNeighbor == null) continue;
                            if (visitedMarkers[potentialNeighbor.X, potentialNeighbor.Y] || potentialNeighbor.GetType() == typeof(Wall)) break;
                            if (potentialNeighbor.GetType() == typeof(GraphVertex))
                            {
                                SearchNode downNeighbor = new SearchNode((GraphVertex)potentialNeighbor, currentSearchNode, 1, endVertex);
                                if (searchType == SearchType.BestFirstManhatten)
                                {
                                    queue.Enqueue(downNeighbor, downNeighbor.Vertex.ManhattenDist(endVertex));
                                }
                                else
                                {
                                    queue.Enqueue(downNeighbor, downNeighbor.ManhattenPriority);
                                }
                                break;
                            }
                        } //Down Neighbor
                        break;
                }
            }
            Stack<GraphVertex> path = new Stack<GraphVertex>();
            SearchNode traverser = endSearchNode;
            while (traverser != null)
            {
                path.Push(traverser.Vertex);
                traverser = traverser.Founder;
            }
            return path;
        }
    }
    class Program
    {
        static void DisplayPath(string[] map, Stack<GraphVertex> path)
        {
            int originX = Console.CursorLeft;
            int originY = Console.CursorTop;
            Console.ResetColor();
            foreach (string line in map)
            {
                foreach (char character in line.ToLower())
                {
                    if (character == 'x')
                        Console.Write('X');
                    else if (character == '.')
                        Console.Write('.');
                    else
                        Console.Write(' ');
                    Console.Write(' ');
                }
                Console.WriteLine("\n");
            }
            Console.ForegroundColor = ConsoleColor.Green;
            GraphVertex parent = null;
            while (path.Count > 0)
            {
                GraphVertex node = path.Pop();
                Console.SetCursorPosition(originX + node.X * 2, originY + node.Y * 2);
                Console.Write('X');
                if (parent != null)
                {
                    Console.CursorLeft--;
                    int xDirection = Math.Sign(parent.X - node.X);
                    int yDirection = Math.Sign(parent.Y - node.Y);
                    char pathCharacter = '-';
                    if (xDirection * yDirection == -1) pathCharacter = '/';
                    else if (xDirection * yDirection == 1) pathCharacter = '\\';
                    else if (xDirection == 0) pathCharacter = '|';
                    for (int i = 1; node.X + (i / 2) * xDirection != parent.X || node.Y + (i / 2) * yDirection != parent.Y; i++)
                    {
                        Console.CursorLeft += xDirection;
                        Console.CursorTop += yDirection;
                        Console.Write(pathCharacter);
                        Console.CursorLeft--;
                    }
                }
                parent = node;
            }
            Console.SetCursorPosition(0, originY + map.Length * 2);
            Console.ResetColor();
        }
        static void Main(string[] args)
        {
            string fileName = "map.txt";
            string path = Path.Combine(Environment.CurrentDirectory, fileName);
            string[] lines = File.ReadAllLines(path);
            int longestLine = 0;
            for (int y = 0; y < lines.Length; y++)
            {
                if (lines[y].Length > longestLine) longestLine = lines[y].Length;
            }
            Node[,] nodeArray = new Node[longestLine, lines.Length];
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < longestLine && x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '.')
                    {
                        Node newNode = new Wall(x, y);
                        nodeArray[x, y] = newNode;
                    }
                    else if (lines[y].ToLower()[x] == 'x')
                    {
                        Node newNode = new GraphVertex(x, y);
                        nodeArray[x, y] = newNode;
                    }
                }
            }
            Graph graph = new Graph(nodeArray);
            Console.WriteLine("A* Manhatten: ");
            Stack<GraphVertex> aStarManhattenPath = graph.FindPath(1, 2, 8, 1, SearchType.AStarManhatten);
            DisplayPath(lines, aStarManhattenPath);
            Console.WriteLine("\nA* Diagonal:");
            Stack<GraphVertex> aStarDiagonalPath = graph.FindPath(1, 2, 8, 1, SearchType.AStarDiagonal);
            DisplayPath(lines, aStarDiagonalPath);
            Console.WriteLine("\nBest First Manhatten:");
            Stack<GraphVertex> bestFirstManhatten = graph.FindPath(1, 2, 8, 1, SearchType.BestFirstManhatten);
            DisplayPath(lines, bestFirstManhatten);
            Console.ReadLine();
        }
    }
}
