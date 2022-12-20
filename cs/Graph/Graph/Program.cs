namespace Graph
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph<int> graph = new();
            for(int i = 0; i < 5; i++)
            {
                graph.AddVertex(i);
            }

            graph.AddEdge(0, 4, 1);
            graph.AddEdge(4, 3, 1);
            graph.AddEdge(3, 2, 1);
            graph.AddEdge(2, 1, 1);
            graph.AddEdge(1, 0, -5);
            Console.WriteLine(GraphAlgorithms.HasNegativeCycle(graph));
        }
    }
}