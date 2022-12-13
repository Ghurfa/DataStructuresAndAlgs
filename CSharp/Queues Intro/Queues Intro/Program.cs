using System;
using System.Collections;

namespace Queues_Intro
{
    class Program
    {
        class QueueNode<T>
        {
            public QueueNode<T> LastNode;
            public QueueNode<T> NextNode;
            public T Value;
            public QueueNode()
            {
                Value = default(T);
            }
            public QueueNode(T value)
            {
                Value = value;
            }
            public QueueNode(QueueNode<T> lastNode, QueueNode<T> nextNode, T value)
            {
                LastNode = lastNode;
                NextNode = nextNode;
                Value = value;
            }
        }
        class Queue<T>
        {
            private QueueNode<T> frontNode;
            private QueueNode<T> backNode;
            public int Count;
            public bool IsEmpty => Count <= 0;

            public Queue()
            {
                frontNode = null;
                backNode = null;
                Count = 0;
            }
            public void Enqueue(T value)
            {
                QueueNode<T> newNode = new QueueNode<T>(null, backNode, value);
                backNode = newNode;
                if (IsEmpty)
                {
                    frontNode = newNode;
                }
                else
                {
                    backNode.NextNode.LastNode = backNode;
                }
                Count++;
            }
            public T Dequeue()
            {
                if (IsEmpty)
                {
                    throw new NullReferenceException("Cannot dequeue; Queue is empty");
                }
                QueueNode<T> formerFrontNode = frontNode;
                frontNode = frontNode.LastNode;
                Count--;
                return formerFrontNode.Value;
            }
            public IEnumerator GetEnumerator()
            {
                QueueNode<T> currentNode = frontNode;
                for(int i = 0; i < Count; i++)
                {
                    yield return currentNode;
                    currentNode = currentNode.LastNode;
                }
            }
        }
        static void Main(string[] args)
        {
            Queue<int> Queue = new Queue<int>();
            Console.WriteLine("Enqueuing 5");
            Queue.Enqueue(5);
            Console.WriteLine($"Count: {Queue.Count}");
            Console.WriteLine($"Dequeuing: {Queue.Dequeue()}");
            Console.WriteLine($"Count: {Queue.Count}");
            //Console.WriteLine($"Dequeuing: {Queue.Dequeue()}");
            Console.WriteLine("Enqueuing 1, enqueuing 2");
            Queue.Enqueue(1);
            Queue.Enqueue(2);
            Console.WriteLine($"Count: {Queue.Count}");
            Console.WriteLine($"Dequeuing: {Queue.Dequeue()}");
            Console.WriteLine("Enqueuing 3, enqueuing 4, enqueuing 5");
            Queue.Enqueue(3);
            Queue.Enqueue(4);
            Queue.Enqueue(5);
            Console.WriteLine($"Count: {Queue.Count}");
            Console.WriteLine($"Dequeuing: {Queue.Dequeue()}");
            Console.WriteLine($"Dequeuing: {Queue.Dequeue()}");
            Console.WriteLine("Items in Queue (from front): ");
            foreach (QueueNode<int> node in Queue)
            {
                Console.Write(node.Value + " ");
            }
        }
    }
}
