using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVL_Tree
{
    class StackNode<T>
    {
        public T Value;
        public StackNode<T> LowerNode;
        public StackNode()
        {
            LowerNode = null;
        }
        public StackNode(T value)
        {
            Value = value;
            LowerNode = null;
        }
    }
    class Stack<T>
    {
        private StackNode<T> topNode;
        public int Count;
        public bool IsEmpty => Count <= 0;
        public Stack()
        {
            topNode = null;
        }
        public void Push(T value)
        {
            StackNode<T> newNode = new StackNode<T>(value)
            {
                LowerNode = topNode
            };
            topNode = newNode;
            Count++;
        }
        public T Pop()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Cannot pop; stack is empty");
            }
            StackNode<T> oldTopNode = topNode;
            topNode = topNode.LowerNode;
            Count--;
            return oldTopNode.Value;
        }
        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Cannot peek; stack is empty");
            }
            T topValue = Pop();
            Push(topValue);
            return topValue;
        }
    }
}
