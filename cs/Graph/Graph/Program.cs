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
            graph.AddEdge(1, 4, -5);
            var pathfindingInfo = GraphAlgorithms.Dijkstra(graph, graph.Search(0));
            foreach (var info in pathfindingInfo)
            {
                Console.WriteLine($"Vertex val: {info.Key.Value}; founder: {info.Value.Founder?.Value.ToString() ?? "null"}; dist: {info.Value.Distance}");
            }
            //Console.WriteLine($"Has negative cycle: {!GraphAlgorithms.BellmanFord(graph, graph.Search(0), out var _)}");
        }
    }
}