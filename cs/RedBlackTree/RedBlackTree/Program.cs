using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    class Program
    {
        public static void DisplayTree(RedBlackTree tree)
        {
            if (tree.IsEmpty) return;
            int displaySubTree(RedBlackNode node, int xOffset, int width)
            {
                if (node == null) return 0;
                if (node.LeftChild == null && node.RightChild == null)
                {
                    Console.CursorLeft = xOffset + width / 2 - node.Value.ToString().Length;
                    Console.ForegroundColor = node.IsRed ? ConsoleColor.Red : ConsoleColor.DarkGray;
                    Console.Write(node.Value);
                    return 1;
                }
                else
                {
                    int row = Console.CursorTop;
                    Console.CursorTop++;
                    int leftHeight = displaySubTree(node.LeftChild, xOffset, width / 2);
                    Console.SetCursorPosition(xOffset + width / 2 - node.Value.ToString().Length, row);
                    Console.ForegroundColor = node.IsRed ? ConsoleColor.Red : ConsoleColor.DarkGray;
                    Console.WriteLine(node.Value);
                    int rightHeight = displaySubTree(node.RightChild, xOffset + width / 2, width / 2);
                    return leftHeight > rightHeight ? 1 + leftHeight : 1 + rightHeight;
                }
            }
            RedBlackNode rootNode = null;
            foreach (RedBlackNode node in tree.LevelOrder())
            {
                rootNode = node;
                break;
            }
            int cursorRow = Console.CursorTop;
            int height = displaySubTree(rootNode, 0, 96);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0, cursorRow + height);
        }
        static void Main(string[] args)
        {
            RedBlackTree tree = new RedBlackTree();
            Console.WriteLine("Type 'hlp' for help");
            while (true)
            {
                string command = Console.ReadLine();
                if (command.Length < 3) continue;
                int number;
                switch (command.ToLower().Substring(0, 3))
                {
                    case "ins":
                        if (command.Length > 3) number = Convert.ToInt32(command.Substring(4, command.Length - 4));
                        else
                        {
                            Console.WriteLine("Type value to insert:");
                            number = Convert.ToInt32(Console.ReadLine());
                        }
                        tree.Insert(number);
                        Console.WriteLine($"Inserted value {number}");
                        break;
                    case "del":
                        if (command.Length > 3) number = Convert.ToInt32(command.Substring(4, command.Length - 4));
                        else
                        {
                            Console.WriteLine("Type value to delete:");
                            number = Convert.ToInt32(Console.ReadLine());
                        }
                        bool successful = tree.Delete(number);
                        Console.WriteLine($"Deleting value {number}: {successful.ToString()}");
                        break;
                    case "sea":
                        if (command.Length > 3) number = Convert.ToInt32(command.Substring(4, command.Length - 4));
                        else
                        {
                            Console.WriteLine("Type value to search for:");
                            number = Convert.ToInt32(Console.ReadLine());
                        }
                        RedBlackNode searchedNode = tree.Search(number);
                        if (searchedNode == null)
                        {
                            Console.WriteLine($"Did not find value {number}");
                        }
                        else
                        {
                            Console.WriteLine($"Found {number} / {(searchedNode.IsRed ? "Red" : "Black")}");
                            if (searchedNode.LeftChild != null) Console.WriteLine($"\t Left node: {searchedNode.LeftChild.Value} / {(searchedNode.LeftChild.IsRed ? "Red" : "Black")}");
                            if (searchedNode.RightChild != null) Console.WriteLine($"\t Right node: {searchedNode.RightChild.Value} / {(searchedNode.RightChild.IsRed ? "Red" : "Black")}");
                        }
                        break;
                    case "dis":
                        DisplayTree(tree);
                        break;
                    case "lvo":
                        foreach (RedBlackNode node in tree.LevelOrder())
                        {
                            Console.Write($"{node.Value}, ");
                        }
                        Console.WriteLine();
                        break;
                    case "ino":
                        foreach (RedBlackNode node in tree.InOrder())
                        {
                            Console.Write($"{node.Value}, ");
                        }
                        Console.WriteLine();
                        break;
                    case "pre":
                        foreach (RedBlackNode node in tree.PreOrder())
                        {
                            Console.Write($"{node.Value}, ");
                        }
                        Console.WriteLine();
                        break;
                    case "pos":
                        foreach (RedBlackNode node in tree.PostOrder())
                        {
                            Console.Write($"{node.Value}, ");
                        }
                        Console.WriteLine();
                        break;
                    case "tst":
                        int[] values = new int[5000];
                        Random random = new Random();
                        for (int i = 0; i < 5000; i++)
                        {
                            values[i] = random.Next(50000);
                        }
                        for (int i = 0; i < 5000; i++)
                        {
                            tree.Insert(values[i]);
                            Console.WriteLine($"Inserted value {values[i]}");
                        }
                        for (int i = 0; i < 5000; i++)
                        {
                            tree.Delete(values[i]);
                            Console.WriteLine($"Deleted value {values[i]}");
                        }
                        Console.WriteLine("done");
                        break;
                    case "clr":
                        Console.Clear();
                        Console.WriteLine("Type 'hlp' for help");
                        break;
                    case "hlp":
                        Console.WriteLine("Type Command: Insert [ins], Delete [del], Minimum [min], Maximum [max], Search [sea], Display Tree [dis], View LevelOrder [lvo], View InOrder [ino], View PreOrder [pre], View PostOrder [pos], Auto-test [tst], Clear [clr");
                        break;
                }
            }
        }
    }
}