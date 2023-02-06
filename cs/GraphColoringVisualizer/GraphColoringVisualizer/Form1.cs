using System.Diagnostics;

namespace GraphColoringVisualizer
{
    public partial class Form1 : Form
    {
        enum States
        {
            None,
            AddingNewNode,
            AddingNewEdge
        }

        private Bitmap Image;
        private Graphics Draw;

        private States AddState = States.None;
        private Point NewNodeLocation;
        private TextBox NewNodeNameInput = new();

        private Node? EdgeStartNode = null;

        private Graph Graph = new();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Image = new Bitmap(display.Width, display.Height);
            Draw = Graphics.FromImage(Image);

            Draw.FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), Image.Size));
            display.Image = Image;

            Controls.Add(NewNodeNameInput);
            NewNodeNameInput.Hide();
            NewNodeNameInput.Enabled = false;
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            Graph.DoColoring((int)colorCountInput.Value);
        }

        private void display_MouseClick(object sender, MouseEventArgs e)
        {
            if (AddState == States.None || AddState == States.AddingNewNode)
            {
                if(AddState == States.AddingNewNode)
                {
                    AddNewNode();
                }

                if (Graph.CheckClick(e.Location, out Node selected))
                {
                    selected.Color = 1;
                    EdgeStartNode = selected;
                    AddState = States.AddingNewEdge;

                    NewNodeNameInput.Enabled = false;
                    NewNodeNameInput.Hide();
                    DrawGraph();
                }
                else
                {
                    NewNodeLocation = e.Location;
                    NewNodeNameInput.Location = NewNodeLocation;
                    NewNodeNameInput.Text = "";
                    NewNodeNameInput.Enabled = true;
                    NewNodeNameInput.Show();
                    NewNodeNameInput.Focus();
                    NewNodeNameInput.BringToFront();

                    AddState = States.AddingNewNode;
                }
            }
            else if (AddState == States.AddingNewEdge)
            {
                if(Graph.CheckClick(e.Location, out Node selected) && EdgeStartNode != selected)
                {
                    Graph.AddEdge(EdgeStartNode, selected);
                }

                EdgeStartNode.Color = 0;
                EdgeStartNode = null;

                DrawGraph();
                AddState = States.None;
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (AddState == States.AddingNewNode)
            {
                AddNewNode();
                AddState = States.None;

                NewNodeNameInput.Enabled = false;
                NewNodeNameInput.Hide();
            }
            else if(AddState == States.AddingNewEdge)
            {
                AddState = States.None;
            }
        }

        private void AddNewNode()
        {
            string name = NewNodeNameInput.Text;

            if (name != "")
            {
                Node newNode = new(new Rectangle(NewNodeLocation, new Size(20, 20)), NewNodeNameInput.Text);
                Graph.Nodes.Add(newNode);
                DrawGraph();
            }
        }

        private void DrawGraph()
        {
            Draw.Clear(Color.White);
            Pen[] pens = new[] { Pens.Black, Pens.Red, Pens.Blue };

            foreach (Edge edge in Graph.Edges)
            {
                Draw.DrawLine(Pens.Black, edge.Start.Center, edge.End.Center);
            }

            foreach (Node node in Graph.Nodes)
            {
                Draw.FillEllipse(Brushes.White, node.Bounds);
                Draw.DrawEllipse(pens[node.Color], node.Bounds);
                Draw.DrawString(node.Name, DefaultFont, Brushes.Black, node.Bounds);
            }

            display.Image = Image;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' && AddState == States.AddingNewNode)
            {
                AddNewNode();

                NewNodeNameInput.Enabled = false;
                NewNodeNameInput.Hide();
                AddState = States.None;
            }
        }
    }
}