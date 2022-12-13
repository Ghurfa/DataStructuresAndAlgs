using System;
using System.Collections;

namespace BinarySearchTree
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
                for (int i = 0; i < Count; i++)
                {
                    yield return currentNode;
                    currentNode = currentNode.LastNode;
                }
            }
        }
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
        public class BSTNode
        {
            public int Value;
            public BSTNode LeftNode;
            public BSTNode RightNode;
            public BSTNode(int value)
            {
                Value = value;
            }
            public BSTNode(int value, BSTNode leftNode, BSTNode rightNode)
            {
                Value = value;
                LeftNode = leftNode;
                RightNode = rightNode;
            }
        }
        public class BST //British Summer Time
        {
            BSTNode rootNode;
            public BST()
            {
                rootNode = null;
            }
            public BST(BSTNode rootNode)
            {
                this.rootNode = rootNode;
            }
            public BSTNode Search(int key)
            {
                if (IsEmpty) return null;
                BSTNode nodeToCheck = rootNode;
                while (nodeToCheck.Value != key)
                {
                    if (nodeToCheck.Value > key && nodeToCheck.LeftNode != null)
                    {
                        nodeToCheck = nodeToCheck.LeftNode;
                    }
                    else if (nodeToCheck.Value < key && nodeToCheck.RightNode != null)
                    {
                        nodeToCheck = nodeToCheck.RightNode;
                    }
                    else return null;
                }
                return nodeToCheck;
            }
            public BSTNode Minimum()
            {
                if (IsEmpty) return null;
                BSTNode nodeToCheck = new BSTNode(0, rootNode, null);
                while (nodeToCheck.LeftNode != null)
                {
                    nodeToCheck = nodeToCheck.LeftNode;
                }
                return nodeToCheck;
            }
            public BSTNode Maximum()
            {
                if (IsEmpty) return null;
                BSTNode nodeToCheck = new BSTNode(0, null, rootNode);
                while (nodeToCheck.RightNode != null)
                {
                    nodeToCheck = nodeToCheck.RightNode;
                }
                return nodeToCheck;
            }
            public void Insert(int value)
            {
                BSTNode newNode = new BSTNode(value);
                if (IsEmpty) { rootNode = newNode; return; }
                BSTNode nodeToCheck = rootNode;
                bool inserted = false;
                while (!inserted)
                {
                    if (value >= nodeToCheck.Value)
                    {
                        if (nodeToCheck.RightNode != null)
                        {
                            nodeToCheck = nodeToCheck.RightNode;
                        }
                        else
                        {
                            nodeToCheck.RightNode = newNode;
                            inserted = true;
                        }
                    }
                    else
                    {
                        if (nodeToCheck.LeftNode != null)
                        {
                            nodeToCheck = nodeToCheck.LeftNode;
                        }
                        else
                        {
                            nodeToCheck.LeftNode = newNode;
                            inserted = true;
                        }
                    }
                }
            }
            public bool Delete(int value)
            {
                if (IsEmpty) return false;
                BSTNode parentNode = new BSTNode(rootNode.Value + 1, rootNode, null);
                BSTNode nodeToDelete = rootNode;
                bool left = false;
                while (nodeToDelete.Value != value)
                {
                    parentNode = nodeToDelete;
                    left = nodeToDelete.Value > value;
                    if (nodeToDelete.Value > value)
                    {
                        nodeToDelete = nodeToDelete.LeftNode;
                    }
                    else if (nodeToDelete.Value < value)
                    {
                        nodeToDelete = nodeToDelete.RightNode;
                    }
                    if (nodeToDelete == null) return false;
                }
                if (nodeToDelete.LeftNode == null)
                {
                    if (left) parentNode.LeftNode = nodeToDelete.RightNode;
                    else parentNode.RightNode = nodeToDelete.RightNode;
                }
                else if (nodeToDelete.RightNode == null)
                {
                    if (left) parentNode.LeftNode = nodeToDelete.LeftNode;
                    else parentNode.RightNode = nodeToDelete.LeftNode;
                }
                else
                {
                    BST leftSubtree = new BST(nodeToDelete.LeftNode);
                    BSTNode leftSubtreeMax = leftSubtree.Maximum();
                    leftSubtree.Delete(leftSubtreeMax.Value);
                    leftSubtreeMax.LeftNode = nodeToDelete.LeftNode;
                    leftSubtreeMax.RightNode = nodeToDelete.RightNode;
                    if (left) parentNode.LeftNode = leftSubtreeMax;
                    else parentNode.RightNode = leftSubtreeMax;
                }
                if (nodeToDelete == rootNode)
                {
                    if (left) rootNode = parentNode.LeftNode;
                    else rootNode = parentNode.RightNode;
                }
                return true;
                /*if (IsEmpty()) return false;
                BSTNode parentNode = new BSTNode(rootNode.Value + 1, rootNode, null);
                BSTNode nodeToCheck = rootNode;
                bool left = false;
                while (nodeToCheck.Value != value)
                {
                    parentNode = nodeToCheck;
                    left = nodeToCheck.Value > value;
                    if (nodeToCheck.Value > value)
                    {
                        nodeToCheck = nodeToCheck.LeftNode;
                    }
                    else if (nodeToCheck.Value < value)
                    {
                        nodeToCheck = nodeToCheck.RightNode;
                    }
                    if (nodeToCheck == null) return false;
                }
                BST otherValues;
                if (left)
                {
                    parentNode.LeftNode = nodeToCheck.LeftNode;
                    otherValues = new BST(nodeToCheck.RightNode);

                }
                else
                {
                    parentNode.RightNode = nodeToCheck.RightNode;
                    otherValues = new BST(nodeToCheck.LeftNode);
                }
                if (nodeToCheck == rootNode)
                {
                    rootNode = parentNode.RightNode;
                }
                foreach (BSTNode node in otherValues.LevelOrder())
                {
                    Insert(node.Value);
                }
                return true;*/
            }
            public bool IsEmpty => rootNode == null;
            public IEnumerable LevelOrder()
            {
                if (!IsEmpty)
                {
                    Queue<BSTNode> values = new Queue<BSTNode>();
                    values.Enqueue(rootNode);

                    while (!values.IsEmpty)
                    {
                        BSTNode value = values.Dequeue();
                        if (value.LeftNode != null) values.Enqueue(value.LeftNode);
                        if (value.RightNode != null) values.Enqueue(value.RightNode);
                        yield return value;
                    }
                }
            }
            public IEnumerable InOrder()
            {
                //recursive (probably should fix)
                BSTNode[] inOrder(BSTNode node)
                {
                    if (node == null) return null;
                    BSTNode[] combineArrays(BSTNode[] firstArray, BSTNode[] secondArray)
                    {
                        if (firstArray == null) return secondArray;
                        if (secondArray == null) return firstArray;
                        BSTNode[] newArray = new BSTNode[firstArray.Length + secondArray.Length];
                        for (int i = 0; i < firstArray.Length; i++)
                        {
                            newArray[i] = firstArray[i];
                        }
                        for (int i = 0; i < secondArray.Length; i++)
                        {
                            newArray[i + firstArray.Length] = secondArray[i];
                        }
                        return newArray;
                    }
                    return combineArrays(combineArrays(inOrder(node.LeftNode), new BSTNode[] { node }), inOrder(node.RightNode));
                }
                BSTNode[] nodes = inOrder(rootNode);
                for(int i = 0; i < nodes.Length; i++)
                {
                    yield return nodes[i];
                }
                /*Stack<BSTNode> nodeStack = new Stack<BSTNode>();
                if (!IsEmpty)
                {
                    
                    nodeStack.Push(rootNode);
                    while (!nodeStack.IsEmpty)
                    {
                        BSTNode node = nodeStack.Pop();
                        BSTNode leftNode = node.LeftNode;
                        BSTNode rightNode = node.RightNode;

                        if (!(rightNode == null && leftNode == null))
                        {
                            if (rightNode != null) nodeStack.Push(rightNode);
                            nodeStack.Push(node);
                            if (leftNode != null) nodeStack.Push(leftNode);
                        }
                        else
                        {
                            yield return node;
                        }
                    }
                    
                
                    bool delete = false;
                    while (!nodeStack.IsEmpty)
                    {
                        if (nodeStack.Peek().LeftNode != null && !delete)
                        {
                            nodeStack.Push(nodeStack.Peek().LeftNode);
                        }
                        else if (nodeStack.Peek().RightNode != null && !delete)
                        {
                            nodeStack.Push(nodeStack.Peek().RightNode);
                        }
                        else
                        {
                            delete = true;
                            BSTNode node = nodeStack.Pop();
                            BSTNode leftNode = nodeStack.IsEmpty ? null : nodeStack.Peek().LeftNode;
                            BSTNode rightNode = nodeStack.IsEmpty ? null : nodeStack.Peek().RightNode;
                            if (leftNode != null && node == rightNode)
                            {
                                nodeStack.Pop();
                            }
                            else
                            {
                                yield return node;
                                if (node == leftNode)
                                {
                                    yield return nodeStack.Peek();
                                    if (rightNode != null)
                                    {
                                        nodeStack.Push(rightNode);
                                        delete = false;
                                    }
                                }
                            }
                        }
                    }
                }*/
            }
            public IEnumerable PreOrder()
            {
                Stack<BSTNode> nodeStack = new Stack<BSTNode>();
                if (!IsEmpty)
                {
                    nodeStack.Push(rootNode);
                    while (!nodeStack.IsEmpty)
                    {
                        BSTNode leftNode = nodeStack.Peek().LeftNode;
                        BSTNode rightNode = nodeStack.Peek().RightNode;

                        yield return nodeStack.Pop();
                        if (rightNode != null) nodeStack.Push(rightNode);
                        if (leftNode != null) nodeStack.Push(leftNode);
                    }
                }
            }
            public IEnumerable PostOrder()
            {
                Stack<BSTNode> nodeStack = new Stack<BSTNode>();
                if (!IsEmpty)
                {
                    nodeStack.Push(rootNode);
                    bool delete = false;
                    while (!nodeStack.IsEmpty)
                    {
                        if (nodeStack.Peek().LeftNode != null && !delete)
                        {
                            nodeStack.Push(nodeStack.Peek().LeftNode);
                        }
                        else if (nodeStack.Peek().RightNode != null && !delete)
                        {
                            nodeStack.Push(nodeStack.Peek().RightNode);
                        }
                        else
                        {
                            delete = true;
                            BSTNode node = nodeStack.Pop();
                            yield return node;
                            if (nodeStack.IsEmpty) break;
                            if (node == nodeStack.Peek().LeftNode && nodeStack.Peek().RightNode != null)
                            {
                                nodeStack.Push(nodeStack.Peek().RightNode);
                                delete = false;
                            }
                        }
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            BST binarySearchTree = new BST();
            while (true)
            {
                Console.WriteLine("Type Command: Insert [ins], Delete [del], Minimum [min], Maximum [max], Search [sea], View LevelOrder [lvo], View InOrder [ino], View PreOrder [pre], View PostOrder [pos]");

                string command = Console.ReadLine();
                int number;
                if (command.Length < 3) continue;
                switch (command.ToLower().Substring(0, 3))
                {
                    case "ins":
                        Console.WriteLine("Type value to insert:");
                        number = Convert.ToInt32(Console.ReadLine());
                        binarySearchTree.Insert(number);
                        Console.WriteLine($"Inserted value {number}");
                        break;
                    case "del":
                        Console.WriteLine("Type value to delete:");
                        number = Convert.ToInt32(Console.ReadLine());
                        bool successful = binarySearchTree.Delete(number);
                        Console.WriteLine($"Deleting value {number}: {successful.ToString()}");
                        break;
                    case "min":
                        BSTNode minNode = binarySearchTree.Minimum();
                        if (minNode == null) Console.WriteLine("Tree is empty");
                        else Console.WriteLine($"Minimum value is {minNode.Value}");
                        break;
                    case "max":
                        BSTNode maxNode = binarySearchTree.Maximum();
                        if (maxNode == null) Console.WriteLine("Tree is empty");
                        else Console.WriteLine($"Maximum value is {maxNode.Value}");
                        break;
                    case "sea":
                        Console.WriteLine("Type value to search for:");
                        number = Convert.ToInt32(Console.ReadLine());
                        BSTNode searchedNode = binarySearchTree.Search(number);
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
                        foreach (BSTNode node in binarySearchTree.LevelOrder())
                        {
                            Console.Write($"{node.Value}, ");
                        }
                        Console.WriteLine();
                        break;
                    case "ino":
                        foreach (BSTNode node in binarySearchTree.InOrder())
                        {
                            Console.Write($"{node.Value}, ");
                        }
                        Console.WriteLine();
                        break;
                    case "pre":
                        foreach (BSTNode node in binarySearchTree.PreOrder())
                        {
                            Console.Write($"{node.Value}, ");
                        }
                        Console.WriteLine();
                        break;
                    case "pos":
                        foreach (BSTNode node in binarySearchTree.PostOrder())
                        {
                            Console.Write($"{node.Value}, ");
                        }
                        Console.WriteLine();
                        break;
                }
                Console.WriteLine();
            }
        }
    }
}
