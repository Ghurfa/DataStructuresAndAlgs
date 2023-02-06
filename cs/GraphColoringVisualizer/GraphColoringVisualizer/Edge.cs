using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoringVisualizer
{
    internal class Edge
    {
        public Node Start { get; set; }
        public Node End { get; set; }

        public Edge (Node start, Node end)
        {
            Start = start; 
            End = end;
        }
    }
}
