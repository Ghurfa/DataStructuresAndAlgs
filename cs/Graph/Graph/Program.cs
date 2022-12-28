namespace Graph
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph<int> graph = new();
            for (int i = 0; i < 5; i++)
            {
                graph.AddVertex(i);
            }


            graph.AddEdge(0, 3, 1);
            graph.AddEdge(0, 2, 1);
            graph.AddEdge(2, 1, 1);
            graph.AddEdge(1, 4, 1);
            graph.AddEdge(4, 3, -5);
            Console.WriteLine($"Has path from 0 to 4: {GraphAlgorithms.PathExists(graph.Search(0), graph.Search(4))}");
            //var pathfindingInfo = GraphAlgorithms.Dijkstra(graph.Search(0));
            //foreach (var info in pathfindingInfo)
            //{
            //    Console.WriteLine($"Vertex val: {info.Key.Value}; founder: {info.Value.Founder?.Value.ToString() ?? "null"}; dist: {info.Value.Distance}");
            //}
            //Console.WriteLine($"Has negative cycle: {!GraphAlgorithms.BellmanFord(graph, graph.Search(0), out var _)}");
        }
    }
}