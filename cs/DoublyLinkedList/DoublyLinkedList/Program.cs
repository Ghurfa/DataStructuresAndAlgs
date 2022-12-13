using System;
using System.Collections;
using System.Linq;
namespace DoublyLinkedList
{
    class Program
    {
        public class DoublyLinkedNode<T>
        {
            public DoublyLinkedNode<T> NextNode;
            public DoublyLinkedNode<T> LastNode;
            public T Value;

            public DoublyLinkedNode()
            {
                NextNode = null;
                LastNode = null;
            }
            public DoublyLinkedNode(T value)
            {
                NextNode = null;
                LastNode = null;
                this.Value = value;
            }
        }
        public class DoublyLinkedList<T>
        {
            public DoublyLinkedNode<T> HeadNode;
            public DoublyLinkedNode<T> TailNode;
            public int Count;
            public bool IsCircular;

            public DoublyLinkedList(bool isCircular)
            {
                HeadNode = TailNode = null;
                Count = 0;
                IsCircular = isCircular;
            }
            public void AddAtIndex(T value, int index)
            {
                if (index < 0 || index > Count) return;

                DoublyLinkedNode<T> previousNode;
                previousNode = new DoublyLinkedNode<T>
                {
                    NextNode = HeadNode
                };
                DoublyLinkedNode<T> nextNode = HeadNode;

                DoublyLinkedNode<T> newNode = new DoublyLinkedNode<T>(value);
                for (int i = 0; i < index; i++)
                {
                    nextNode = nextNode.NextNode;
                    previousNode = previousNode.NextNode;
                }
                if (index == 0)
                {
                    HeadNode = newNode;
                }
                else
                {
                    newNode.LastNode = previousNode;
                    previousNode.NextNode = newNode;
                }
                if (index == Count)
                {
                    TailNode = newNode;
                }
                else
                {
                    newNode.NextNode = nextNode;
                    nextNode.LastNode = newNode;
                }
                if (IsCircular && index == 0)
                {
                    newNode.LastNode = TailNode;
                    TailNode.NextNode = newNode;
                }
                if (IsCircular && index == Count)
                {
                    newNode.NextNode = HeadNode;
                    HeadNode.LastNode = newNode;
                }
                Count++;
            }
            public void AddToEnd(T value)
            {
                AddAtIndex(value, Count);
            }
            public void AddToFront(T value)
            {
                AddAtIndex(value, 0);
            }
            public bool RemoveAtIndex(int index)
            {
                if (index < 0 || index > Count - 1) return false;

                DoublyLinkedNode<T> nodeToDelete = HeadNode;
                
                for (int i = 0; i < index; i++)
                {
                    nodeToDelete = nodeToDelete.NextNode;
                }
                if (index == 0)
                {
                    HeadNode = HeadNode.NextNode;
                }
                if (index == Count - 1)
                {
                    TailNode = TailNode.LastNode;
                }
                if(IsCircular || index != 0)
                {
                    nodeToDelete.LastNode.NextNode = nodeToDelete.NextNode;
                }
                if(IsCircular || index != Count - 1)
                {
                    nodeToDelete.NextNode.LastNode = nodeToDelete.LastNode;
                }
                Count--;
                return true;
            }
            public bool RemoveFromFront()
            {
                return RemoveAtIndex(0);
            }
            public bool RemoveFromEnd()
            {
                return RemoveAtIndex(Count - 1);
            }
            public bool IsEmpty() => Count <= 0;
            public IEnumerator GetEnumerator()
            {
                DoublyLinkedNode<T> node = HeadNode;
                for (int i = 0; i < Count; i++)
                {
                    yield return node;
                    node = node.NextNode;
                }
            }
            public IEnumerable GetFromHead(bool repeat = false)
            {
                DoublyLinkedNode<T> node = HeadNode;
                int times = repeat ? 2 : 1;
                for (int i = 0; i < times * Count; i++)
                {
                    yield return node;
                    node = node.NextNode;
                }
            }
            public IEnumerable GetFromTail(bool repeat = false)
            {
                DoublyLinkedNode<T> node = TailNode;
                int times = repeat ? 2 : 1;
                for (int i = 0; i < times * Count; i++)
                {
                    yield return node;
                    node = node.LastNode;
                }
            }
        }
        static void Main(string[] args)
        {
            void displayList(DoublyLinkedList<int> list)
            {
                Console.Write($"FROM HEAD: ");
                int counter = 0;
                foreach(DoublyLinkedNode<int> number in list.GetFromHead(true))
                {
                    if (counter == list.Count) Console.Write("| ");
                    Console.Write(number.Value + " ");
                    counter++;
                }
                Console.Write($"FROM TAIL: ");
                counter = 0;
                foreach (DoublyLinkedNode<int> number in list.GetFromTail(true))
                {
                    if (counter == list.Count) Console.Write("| ");
                    Console.Write(number.Value + " ");
                    counter++;
                }
                Console.WriteLine();
            }
            DoublyLinkedList<int> integerList = new DoublyLinkedList<int>(true);
            bool exit = false;
            while(!exit)
            {
                Console.WriteLine("Options: Display list [d], Add at Front [af], Add at End [ae], Add at Index [ai], Remove from Front [rf], Remove from End [re], Remove at Index [ri], Exit [ex]");
                string choice = Console.ReadLine().ToLower();
                int value = 0;
                int index = 0;
                string status;
                switch(choice)
                {
                    case "d":
                        displayList(integerList);
                        Console.WriteLine();
                        break;
                    case "af":
                        Console.WriteLine("Value:");
                        value = Convert.ToInt32(Console.ReadLine());
                        integerList.AddToFront(value);
                        Console.WriteLine($"Added {value} to front");
                        Console.WriteLine();
                        break;
                    case "ae":
                        Console.WriteLine("Value:");
                        value = Convert.ToInt32(Console.ReadLine());
                        integerList.AddToEnd(value);
                        Console.WriteLine($"Added {value} to end");
                        Console.WriteLine();
                        break;
                    case "ai":
                        Console.WriteLine("Value:");
                        value = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Index:");
                        index = Convert.ToInt32(Console.ReadLine());
                        integerList.AddAtIndex(value, index);
                        Console.WriteLine($"Added {value} at index {index}");
                        Console.WriteLine();
                        break;
                    case "rf":
                        status = integerList.RemoveFromFront() ? "successful" : "unsuccessful";
                        Console.WriteLine($"Removing from front was {status}");
                        Console.WriteLine();
                        break;
                    case "re":
                        status = integerList.RemoveFromEnd() ? "successful" : "unsuccessful";
                        Console.WriteLine($"Removing from end was {status}");
                        Console.WriteLine();
                        break;
                    case "ri":
                        Console.WriteLine("Index:");
                        index = Convert.ToInt32(Console.ReadLine());
                        status = integerList.RemoveAtIndex(index) ? "successful" : "unsuccessful";
                        Console.WriteLine($"Removing at index {index} was {status}");
                        Console.WriteLine();
                        break;
                    case "ex":
                        exit = true;
                        break;
                }

            }
            /*Console.WriteLine("Adding 5 to end");
            integerList.AddToEnd(5);
            displayList(integerList);

            Console.WriteLine("Adding 6 to end");
            integerList.AddToEnd(6);
            displayList(integerList);

            Console.WriteLine("Adding 7 to end");
            integerList.AddToEnd(7);
            displayList(integerList);

            Console.WriteLine("Adding 3 to front");
            integerList.AddToFront(3);
            displayList(integerList);

            Console.WriteLine("Adding 2 to front");
            integerList.AddToFront(2);
            displayList(integerList);

            Console.WriteLine("Adding 1 to front");
            integerList.AddToFront(1);
            displayList(integerList);

            Console.WriteLine("Adding 4 at index 3");
            integerList.AddAtIndex(4, 3);
            displayList(integerList);

            Console.WriteLine("Clearing list...");
            integerList = new DoublyLinkedList<int>(true);
            
            Console.WriteLine("Adding 1 to front");
            integerList.AddToFront(1);
            
            Console.WriteLine($"Removing from front: {integerList.RemoveFromFront()}");

            displayList(integerList);

            Console.WriteLine($"Removing from front: {integerList.RemoveFromFront()}");
            Console.WriteLine($"Removing from end: {integerList.RemoveFromEnd()}");
            Console.WriteLine($"Removing at index 0: {integerList.RemoveAtIndex(0)}");

            Console.WriteLine("Adding 1 to front");
            integerList.AddToFront(1);

            Console.WriteLine($"Removing from end: {integerList.RemoveFromEnd()}");

            Console.WriteLine("Adding 1 to end, 2 to end, 3 to end, 4 to end, 5 to end");
            integerList.AddToEnd(1);
            integerList.AddToEnd(2);
            integerList.AddToEnd(3);
            integerList.AddToEnd(4);
            integerList.AddToEnd(5);
            displayList(integerList);

            Console.WriteLine($"Removing from front: {integerList.RemoveFromFront()}");
            displayList(integerList);
            Console.WriteLine($"Removing from end: {integerList.RemoveFromEnd()}");
            displayList(integerList);
            Console.WriteLine($"Removing at index 1: {integerList.RemoveAtIndex(1)}");
            displayList(integerList);*/
        }
    }
}
