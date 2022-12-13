using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVL_Tree
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
            for (int i = 0; i < Count; i++)
            {
                yield return currentNode;
                currentNode = currentNode.LastNode;
            }
        }
    }
}
