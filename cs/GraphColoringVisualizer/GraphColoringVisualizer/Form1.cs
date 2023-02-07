using System.Diagnostics;
using System.Windows.Forms.VisualStyles;

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

        private List<(Node A, Node? B)> ActionStack = new();
        private int CurrActionIdx = -1;
        private int MaxRedoIdx = -1;

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
            bool colorable = Graph.IsColorable((int)colorCountInput.Value);
            statusLabel.Text = colorable ? "Colorable" : "Not colorable";
            DrawGraph();
        }

        private void display_MouseClick(object sender, MouseEventArgs e)
        {
            ClearColorability();
            if (AddState == States.None || AddState == States.AddingNewNode)
            {
                if (AddState == States.AddingNewNode)
                {
                    AddNewNode();
                }

                if (Graph.CheckClick(e.Location, out Node selected))
                {
                    selected.Color = 1;
                    EdgeStartNode = selected;
                    HideNewNodeBox();
                    DrawGraph();
                    AddState = States.AddingNewEdge;
                }
                else
                {
                    ShowNewNodeBox(e.Location);
                    AddState = States.AddingNewNode;
                }
            }
            else if (AddState == States.AddingNewEdge)
            {
                if (Graph.CheckClick(e.Location, out Node selected) && EdgeStartNode != selected)
                {
                    Graph.AddEdge(EdgeStartNode, selected);

                    CurrActionIdx++;
                    MaxRedoIdx = CurrActionIdx;
                    if (ActionStack.Count == CurrActionIdx)
                    {
                        ActionStack.Add((EdgeStartNode, selected));
                    }
                    else
                    {
                        ActionStack[CurrActionIdx] = (EdgeStartNode, selected);
                    }
                }

                EdgeStartNode.Color = 0;
                EdgeStartNode = null;

                DrawGraph();
                AddState = States.None;
            }
        }

        private void Deselect()
        {
            if (AddState == States.AddingNewNode)
            {
                AddNewNode();
                HideNewNodeBox();
            }
            else if (AddState == States.AddingNewEdge)
            {
                EdgeStartNode.Color = 0;
                EdgeStartNode = null;
            }
            DrawGraph();
            AddState = States.None;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Deselect();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' && AddState == States.AddingNewNode)
            {
                AddNewNode();
                HideNewNodeBox();
                AddState = States.None;
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            Deselect();
            Graph.Nodes.Clear();
            DrawGraph();

            ActionStack.Clear();
            CurrActionIdx = -1;
            MaxRedoIdx = -1;

            AddState = States.None;
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            Deselect();
            if (CurrActionIdx < 0) return;

            ClearColorability();

            (Node a, Node? b) = ActionStack[CurrActionIdx];
            if (b == null)
            {
                Graph.RemoveNode(a);
            }
            else
            {
                Graph.RemoveEdge(a, b);
            }
            CurrActionIdx--;
            DrawGraph();
        }

        private void redoButton_Click(object sender, EventArgs e)
        {
            Deselect();
            if (CurrActionIdx >= MaxRedoIdx) return;

            ClearColorability();
            CurrActionIdx++;

            (Node a, Node? b) = ActionStack[CurrActionIdx];
            if (b == null)
            {
                Graph.AddNode(a);
            }
            else
            {
                Graph.AddEdge(a, b);
            }

            DrawGraph();
        }

        private void AddNewNode()
        {
            string name = NewNodeNameInput.Text;

            if (name != "")
            {
                Node newNode = new(new Rectangle(NewNodeLocation, new Size(20, 20)), NewNodeNameInput.Text);
                Graph.Nodes.Add(newNode);
                DrawGraph();

                CurrActionIdx++;
                MaxRedoIdx = CurrActionIdx;
                if (ActionStack.Count == CurrActionIdx)
                {
                    ActionStack.Add((newNode, null));
                }
                else
                {
                    ActionStack[CurrActionIdx] = (newNode, null);
                }
            }
        }

        private void ShowNewNodeBox(Point location)
        {
            NewNodeLocation = location;
            NewNodeNameInput.Location = NewNodeLocation;
            NewNodeNameInput.Text = "";
            NewNodeNameInput.Enabled = true;
            NewNodeNameInput.Show();
            NewNodeNameInput.Focus();
            NewNodeNameInput.BringToFront();
        }

        private void HideNewNodeBox()
        {
            NewNodeNameInput.Enabled = false;
            NewNodeNameInput.Hide();
        }

        private void ClearColorability()
        {
            Graph.ResetColors();
            statusLabel.Text = string.Empty;
            DrawGraph();
        }

        private void DrawGraph()
        {
            Draw.Clear(Color.White);
            Pen[] pens = new[] { Pens.Black, Pens.Red, Pens.Blue };

            // Double-draws edges but w/e
            foreach (Node start in Graph.Nodes)
            {
                foreach (Node end in start.Neighbors)
                {
                    Draw.DrawLine(Pens.Black, start.Center, end.Center);
                }
            }

            foreach (Node node in Graph.Nodes)
            {
                Draw.FillEllipse(Brushes.White, node.Bounds);
                Draw.DrawEllipse(pens[node.Color], node.Bounds);
                Draw.DrawString(node.Name, DefaultFont, Brushes.Black, node.Bounds);
            }

            display.Image = Image;
        }
    }
}