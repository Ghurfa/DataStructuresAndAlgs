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

        public void AddNode(Node node)
        {
            Nodes.Add(node);
        }

        public bool AddEdge(Node start, Node end)
        {
            if (start.Neighbors.Contains(end))
            {
                return false;
            }

            start.Neighbors.Add(end);
            end.Neighbors.Add(start);
            return true;
        }

        public bool RemoveNode(Node node)
        {
            return Nodes.Remove(node);
        }

        public bool RemoveEdge(Node start, Node end)
        {
            return start.Neighbors.Remove(end) & end.Neighbors.Remove(start);
        }

        public bool IsColorable(int colorCount)
        {
            if (colorCount == 2)
            {
                return Is2Colorable();
            }
            else throw new NotImplementedException();
        }

        private bool Is2Colorable()
        {
            ResetColors();

            Queue<Node> queue = new(Nodes);

            // Component = connected subgraph that is not part of any other connected subgraph
            while (queue.TryDequeue(out Node componentStart))
            {
                if (componentStart.Color != 0) continue;

                componentStart.Color = 1;
                Stack<Node> currGraphStack = new();
                currGraphStack.Push(componentStart);

                while (currGraphStack.TryPop(out Node curr))
                {
                    foreach (Node neighbor in curr.Neighbors)
                    {
                        if (neighbor.Color == 0)
                        {
                            neighbor.Color = curr.Color == 1 ? 2 : 1;
                            currGraphStack.Push(neighbor);
                        }
                        else if (neighbor.Color == curr.Color)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public void ResetColors()
        {
            foreach(Node node in Nodes)
            {
                node.Color = 0;
            }
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
