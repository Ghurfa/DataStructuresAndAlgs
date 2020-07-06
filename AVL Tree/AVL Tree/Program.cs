using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVL_Tree
{
    class Program
    {
        static void Main(string[] args)
        {
            AVLTree avlTree = new AVLTree();
            while (true)
            {
                Console.WriteLine("Type Command: Insert [ins], Delete [del], Minimum [min], Maximum [max], Search [sea], View LevelOrder [lvo], View InOrder [ino], View PreOrder [pre], View PostOrder [pos], Break [brk]");

                string command = Console.ReadLine();
                int number;
                if (command.Length < 3) continue;
                switch (command.ToLower().Substring(0, 3))
                {
                    case "ins":
                        Console.WriteLine("Type value to insert:");
                        number = Convert.ToInt32(Console.ReadLine());
                        avlTree.Insert(number);
                        Console.WriteLine($"Inserted value {number}");
                        break;
                    case "del":
                        Console.WriteLine("Type value to delete:");
                        number = Convert.ToInt32(Console.ReadLine());
                        bool successful = avlTree.Delete(number);
                        Console.WriteLine($"Deleting value {number}: {successful.ToString()}");
                        break;
                    case "min":
                        AVLNode minNode = avlTree.Minimum();
                        if (minNode == null) Console.WriteLine("Tree is empty");
                        else Console.WriteLine($"Minimum value is {minNode.Value}");
                        break;
                    case "max":
                        AVLNode maxNode = avlTree.Maximum();
                        if (maxNode == null) Console.WriteLine("Tree is empty");
                        else Console.WriteLine($"Maximum value is {maxNode.Value}");
                        break;
                    case "sea":
                        Console.WriteLine("Type value to search for:");
                        number = Convert.ToInt32(Console.ReadLine());
                        AVLNode searchedNode = avlTree.Search(number);
                        if (searchedNode == null)
                        {
                            Console.WriteLine($"Did not find value {number}");
                        }
                        else
                        {
                            Console.WriteLine($"Found value {number}");
                            if (searchedNode.LeftNode != null) Console.WriteLine($"{number}'s Left node: {searchedNode.LeftNode.Value}");
                            if (searchedNode.RightNode != null) Console.WriteLine($"{number}'s Right node: {searchedNode.RightNode.Value}");
                        }
                        break;
                    case "lvo":
                        foreach (AVLNode node in avlTree.LevelOrder())
                        {
                            Console.Write($"{node.Value}, ");
                        }
                        Console.WriteLine();
                        break;
                    case "ino":
                        foreach (AVLNode node in avlTree.InOrder())
                        {
                            Console.Write($"{node.Value}, ");
                        }
                        Console.WriteLine();
                        break;
                    case "pre":
                        foreach (AVLNode node in avlTree.PreOrder())
                        {
                            Console.Write($"{node.Value}, ");
                        }
                        Console.WriteLine();
                        break;
                    case "pos":
                        foreach (AVLNode node in avlTree.PostOrder())
                        {
                            Console.Write($"{node.Value}, ");
                        }
                        Console.WriteLine();
                        break;
                    case "brk":
                    break;
                }
                Console.WriteLine();
            }
        }
    }
}
