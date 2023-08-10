namespace Graph
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph<int> graph = new();

            for (int i = 0; i < 15; i++)
            {
                graph.AddVertex(i);
            }

            void AddEdge(int a, int b)
            {
                graph.AddEdge(a, b, 1);
                graph.AddEdge(b, a, 1);
            }

            /*AddEdge(0, 1);
            AddEdge(2, 3);
            AddEdge(0, 4);
            AddEdge(1, 4);
            AddEdge(2, 4);
            AddEdge(3, 4);*/

            // Outer pentagon
            AddEdge(0, 1);
            AddEdge(1, 2);
            AddEdge(2, 3);
            AddEdge(3, 4);
            AddEdge(4, 0);

            // Inner start
            AddEdge(5, 7);
            AddEdge(7, 9);
            AddEdge(9, 6);
            AddEdge(6, 8);
            AddEdge(8, 5);

            // Outer pentagon to inner star
            AddEdge(0, 5);
            AddEdge(1, 6);
            AddEdge(2, 7);
            AddEdge(3, 8);
            AddEdge(4, 9);

            // Reinforcement "edges"
            AddEdge(0, 10);
            AddEdge(5, 10);
            AddEdge(1, 11);
            AddEdge(6, 11);
            AddEdge(2, 12);
            AddEdge(7, 12);
            AddEdge(3, 13);
            AddEdge(8, 13);
            AddEdge(4, 14);
            AddEdge(9, 14);

            Console.WriteLine(MathStuff.ComponentCount(graph));
            Console.WriteLine($"Eulerian: {MathStuff.IsEulerian(graph)}");
            Console.WriteLine($"Tough: {MathStuff.IsTough(graph)}");
        }
    }
}