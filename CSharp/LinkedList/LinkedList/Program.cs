using System;

namespace LinkedList
{
    class Program
    {
        public class SinglyLinkedNode<T>
        {
            public T Value;
            public SinglyLinkedNode<T> Next;

            public SinglyLinkedNode(T value, SinglyLinkedNode<T> nextNode = null)
            {
                Value = value;
                Next = nextNode;
            }
        }
        public class SinglyLinkedList<T>
        {
            SinglyLinkedNode<T> HeadNode;
            public SinglyLinkedList() => Clear();
            public void AddToFront(T value)
            {
                SinglyLinkedNode<T> oldHeadNode = HeadNode;
                HeadNode = new SinglyLinkedNode<T>(value, oldHeadNode);
                Count++;
            }
            public void AddToEnd(T value)
            {
                SinglyLinkedNode<T> lastNode = HeadNode;
                if (lastNode != null)
                {
                    while (lastNode.Next != null)
                    {
                        lastNode = lastNode.Next;
                    }
                    lastNode.Next = new SinglyLinkedNode<T>(value);
                }
                else
                {
                    HeadNode = new SinglyLinkedNode<T>(value, null);
                }
                Count++;
            }
            public bool RemoveAtPosition(int index)
            {
                if (Count <= index || index < 0)
                {
                    return false;
                }
                SinglyLinkedNode<T> nodeBefore = HeadNode;
                for (int i = 0; i < index - 1; i++)
                {
                    nodeBefore = nodeBefore.Next;
                }
                SinglyLinkedNode<T> nodeAfter = nodeBefore.Next.Next;
                nodeBefore.Next = nodeAfter;
                Count--;
                return true;
            }
            public bool RemoveItem(T value)
            {
                SinglyLinkedNode<T> nodeBefore = null;
                SinglyLinkedNode<T> currentNode = HeadNode;
                for (int i = 0; i < Count; i++)
                {
                    if (currentNode.Value.Equals(value))
                    {
                        nodeBefore.Next = currentNode.Next;
                        Count--;
                        return true;
                    }
                    else
                    {
                        nodeBefore = currentNode;
                        currentNode = currentNode.Next;
                    }
                }
                return false;
            }
            public bool IsEmpty() => Count <= 0;
            public void Clear()
            {
                HeadNode = null;
                Count = 0;
            }
            public bool Contains(T value)
            {
                SinglyLinkedNode<T> currentNode = HeadNode;
                for(int i = 0; i < Count; i++)
                {
                    if(currentNode.Value.Equals(value))
                    {
                        return true;
                    }
                    else
                    {
                        currentNode = currentNode.Next;
                    }
                }
                return false;
            }
            public SinglyLinkedNode<T> Find(T value)
            {
                SinglyLinkedNode<T> currentNode = HeadNode;
                for(int i = 0; i < Count; i++)
                {
                    if(currentNode.Value.Equals(value))
                    {
                        return currentNode;
                    }
                    else
                    {
                        currentNode = currentNode.Next;
                    }
                }
                return null;
            }
            public int Count;
        }
        static void Main(string[] args)
        {
            SinglyLinkedList<int> list = new SinglyLinkedList<int>();

        }
    }
}