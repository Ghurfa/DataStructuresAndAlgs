using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoringVisualizer
{
    internal class Node
    {
        public Rectangle Bounds { get; set; }
        public string Name { get; set; }
        public int Color { get; set; }

        public List<Node> Neighbors { get; }

        public Point Center => new Point(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2);

        public Node(Rectangle bounds, string name)
        {
            Bounds = bounds;
            Name = name;
            Color = 0;
            Neighbors = new List<Node>();
        }
    }
}
