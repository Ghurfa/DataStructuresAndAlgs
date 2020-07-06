using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVL_vs_RedBlack
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] values = new int[5000];
            Random random = new Random();
            for (int i = 0; i < 5000; i++)
            {
                values[i] = random.Next(50000);
            }
            AVLTree avlTree = new AVLTree();
            DateTime startTime = DateTime.Now;
            for (int i = 0; i < 5000; i++)
            {
                avlTree.Insert(values[i]);
            }
            TimeSpan avlInsertTime = (DateTime.Now - startTime);
            RedBlackTree redBlackTree = new RedBlackTree();
            startTime = DateTime.Now;
            for (int i = 0; i < 5000; i++)
            {
                redBlackTree.Insert(values[i]);
            }
            TimeSpan redBlackInsertTime = (DateTime.Now - startTime);
            startTime = DateTime.Now;
            for (int i = 0; i < 5000; i++)
            {
                avlTree.Delete(values[i]);
            }
            TimeSpan avlDeleteTime = (DateTime.Now - startTime);
            startTime = DateTime.Now;
            for (int i = 0; i < 5000; i++)
            {
                redBlackTree.Delete(values[i]);
            }
            TimeSpan redBlackDeleteTime = (DateTime.Now - startTime);
            Console.WriteLine($"AVL Tree Insert time: {avlInsertTime.TotalMilliseconds} milliseconds");
            Console.WriteLine($"Red Black Tree Insert time: {redBlackInsertTime.TotalMilliseconds} milliseconds");
            Console.WriteLine($"AVL Tree Delete time: {avlDeleteTime.TotalMilliseconds} milliseconds");
            Console.WriteLine($"Red Black Tree Delete time: {redBlackDeleteTime.TotalMilliseconds} milliseconds");
            while (true) ;
        }
    }
}
