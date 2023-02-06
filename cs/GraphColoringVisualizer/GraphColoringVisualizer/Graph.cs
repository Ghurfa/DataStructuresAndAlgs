using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoringVisualizer
{
    internal class Graph
    {
        public List<Node> Nodes = new();
        public List<Edge> Edges = new();

        public void AddNode(Node node)
        {
            Nodes.Add(node);
        }

        public void DoColoring(int colorCount)
        {

        }

        public bool AddEdge(Node start, Node end)
        {
            foreach(Edge other in Edges)
            {
                if(other.Start == start && other.End == end) 
                {
                    return false;
                }
            }

            Edges.Add(new Edge(start, end));
            return true;
        }

        public bool CheckClick(Point point, out Node? selected)
        {
            foreach (Node node in Nodes)
            {
                if (node.Bounds.Contains(point))
                {
                    selected = node;
                    return true;
                }
            }

            selected = null;
            return false;
        }
    }
}
