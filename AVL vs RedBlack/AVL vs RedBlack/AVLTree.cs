using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVL_vs_RedBlack
{
    public class AVLNode
    {
        public int Value;
        public AVLNode LeftNode;
        public AVLNode RightNode;
        private int height;
        public int BalanceFactor()
        {
            int leftHeight = LeftNode != null ? LeftNode.height : 0;
            int rightHeight = RightNode != null ? RightNode.height : 0;
            return rightHeight - leftHeight;
        }
        public AVLNode(int value)
        {
            Value = value;
            height = 1;
        }
        public AVLNode(AVLNode leftNode, AVLNode rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
            UpdateHeight();
        }
        public AVLNode(int value, AVLNode leftNode, AVLNode rightNode)
        {
            Value = value;
            LeftNode = leftNode;
            RightNode = rightNode;
            UpdateHeight();
        }
        public void UpdateHeight()
        {
            int leftHeight = LeftNode != null ? LeftNode.height : 0;
            int rightHeight = RightNode != null ? RightNode.height : 0;
            height = leftHeight > rightHeight ? leftHeight + 1 : rightHeight + 1;
        }
    }
    public class AVLTree
    {
        AVLNode rootNode;
        public bool IsEmpty => rootNode == null;
        public AVLTree()
        {
            rootNode = null;
        }
        public AVLTree(AVLNode root)
        {
            rootNode = root;
        }
        private AVLNode deleteMaxValue()
        {
            if (IsEmpty) return null;
            if (rootNode.RightNode != null)
            {
                AVLTree rightSubTree = new AVLTree(rootNode.RightNode);
                AVLNode result = rightSubTree.deleteMaxValue();
                rootNode.RightNode = rightSubTree.rootNode;
                balance();
                return result;
            }
            else
            {
                AVLNode oldRootNode = rootNode;
                rootNode = rootNode.LeftNode;
                return oldRootNode;
            }
        }
        public void Insert(int value)
        {
            if (IsEmpty) { rootNode = new AVLNode(value); return; }

            if (value < rootNode.Value)
            {
                if (rootNode.LeftNode == null)
                {
                    rootNode.LeftNode = new AVLNode(value);
                }
                else
                {
                    AVLTree subTree = new AVLTree(rootNode.LeftNode);
                    subTree.Insert(value);
                    rootNode.LeftNode = subTree.rootNode;
                }
            }
            else
            {
                if (rootNode.RightNode == null)
                {
                    rootNode.RightNode = new AVLNode(value);
                }
                else
                {
                    AVLTree subTree = new AVLTree(rootNode.RightNode);
                    subTree.Insert(value);
                    rootNode.RightNode = subTree.rootNode;
                }
            }
            balance();
        }
        public bool Delete(int value)
        {
            if (IsEmpty) return false;
            if (value == rootNode.Value)
            {
                bool leftNodeNull = rootNode.LeftNode == null;
                bool rightNodeNull = rootNode.RightNode == null;
                if (leftNodeNull && rightNodeNull)
                {
                    rootNode = null;
                }
                else if (!leftNodeNull && rightNodeNull)
                {
                    rootNode = rootNode.LeftNode;
                }
                else if (leftNodeNull && !rightNodeNull)
                {
                    rootNode = rootNode.RightNode;
                }
                else
                {
                    AVLTree leftSubTree = new AVLTree(rootNode.LeftNode);
                    rootNode = new AVLNode(leftSubTree.deleteMaxValue().Value, leftSubTree.rootNode, rootNode.RightNode);
                    balance();
                }
                return true;
            }
            else if (value < rootNode.Value)
            {
                AVLTree leftSubTree = new AVLTree(rootNode.LeftNode);
                bool result = leftSubTree.Delete(value);
                rootNode.LeftNode = leftSubTree.rootNode;
                balance();
                return result;
            }
            else
            {
                AVLTree rightSubTree = new AVLTree(rootNode.RightNode);
                bool result = rightSubTree.Delete(value);
                rootNode.RightNode = rightSubTree.rootNode;
                balance();
                return result;
            }
        }
        public AVLNode Search(int value)
        {
            AVLNode nodeToCheck = rootNode;
            while (nodeToCheck != null && nodeToCheck.Value != value)
            {
                if (value < nodeToCheck.Value)
                {
                    nodeToCheck = nodeToCheck.LeftNode;
                    continue;
                }
                nodeToCheck = nodeToCheck.RightNode;
            }
            return nodeToCheck;
        }
        public AVLNode Minimum()
        {
            if (IsEmpty) return null;
            AVLNode nodeToCheck = rootNode;
            while (nodeToCheck.LeftNode != null)
            {
                nodeToCheck = nodeToCheck.LeftNode;
            }
            return nodeToCheck;
        }
        public AVLNode Maximum()
        {
            if (IsEmpty) return null;
            AVLNode nodeToCheck = rootNode;
            while (nodeToCheck.RightNode != null)
            {
                nodeToCheck = nodeToCheck.RightNode;
            }
            return nodeToCheck;
        }
        public IEnumerable LevelOrder()
        {
            if (!IsEmpty)
            {
                Queue<AVLNode> values = new Queue<AVLNode>();
                values.Enqueue(rootNode);

                while (values.Count > 0)
                {
                    AVLNode value = values.Dequeue();
                    if (value.LeftNode != null) values.Enqueue(value.LeftNode);
                    if (value.RightNode != null) values.Enqueue(value.RightNode);
                    yield return value;
                }
            }
        }
        public IEnumerable InOrder()
        {
            if (!IsEmpty)
            {
                AVLNode[] inOrder(AVLNode node)
                {
                    if (node == null) return null;
                    AVLNode[] combineArrays(AVLNode[] firstArray, AVLNode[] secondArray)
                    {
                        if (firstArray == null) return secondArray;
                        if (secondArray == null) return firstArray;
                        AVLNode[] newArray = new AVLNode[firstArray.Length + secondArray.Length];
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
                    return combineArrays(combineArrays(inOrder(node.LeftNode), new AVLNode[] { node }), inOrder(node.RightNode));
                }
                AVLNode[] nodes = inOrder(rootNode);
                for (int i = 0; i < nodes.Length; i++)
                {
                    yield return nodes[i];
                }
            }
        }
        public IEnumerable PreOrder()
        {
            Stack<AVLNode> nodeStack = new Stack<AVLNode>();
            if (!IsEmpty)
            {
                nodeStack.Push(rootNode);
                while (nodeStack.Count > 0)
                {
                    AVLNode leftNode = nodeStack.Peek().LeftNode;
                    AVLNode rightNode = nodeStack.Peek().RightNode;

                    yield return nodeStack.Pop();
                    if (rightNode != null) nodeStack.Push(rightNode);
                    if (leftNode != null) nodeStack.Push(leftNode);
                }
            }
        }
        public IEnumerable PostOrder()
        {
            Stack<AVLNode> nodeStack = new Stack<AVLNode>();
            if (!IsEmpty)
            {
                nodeStack.Push(rootNode);
                bool delete = false;
                while (nodeStack.Count > 0)
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
                        AVLNode node = nodeStack.Pop();
                        yield return node;
                        if (nodeStack.Count == 0) break;
                        if (node == nodeStack.Peek().LeftNode && nodeStack.Peek().RightNode != null)
                        {
                            nodeStack.Push(nodeStack.Peek().RightNode);
                            delete = false;
                        }
                    }
                }
            }
        }
        private void rotateLeft()
        {
            AVLNode oldRightNode = rootNode.RightNode;
            AVLNode tempNode = rootNode.RightNode.LeftNode;
            rootNode.RightNode.LeftNode = rootNode;
            rootNode.RightNode = tempNode;
            rootNode = oldRightNode;
            rootNode.LeftNode.UpdateHeight();
            rootNode.UpdateHeight();
        }
        private void rotateRight()
        {
            AVLNode oldLeftNode = rootNode.LeftNode;
            AVLNode tempNode = rootNode.LeftNode.RightNode;
            rootNode.LeftNode.RightNode = rootNode;
            rootNode.LeftNode = tempNode;
            rootNode = oldLeftNode;
            rootNode.RightNode.UpdateHeight();
            rootNode.UpdateHeight();
        }
        private void rotateLeftRight()
        {
            AVLNode oldRootNode = rootNode;
            AVLNode oldRightNode = rootNode.RightNode;
            rootNode = oldRightNode.LeftNode;
            oldRootNode.RightNode = rootNode.LeftNode;
            oldRightNode.LeftNode = rootNode.RightNode;
            rootNode.LeftNode = oldRootNode;
            rootNode.RightNode = oldRightNode;
            rootNode.LeftNode.UpdateHeight();
            rootNode.RightNode.UpdateHeight();
            rootNode.UpdateHeight();
        }
        private void rotateRightLeft()
        {
            AVLNode oldRootNode = rootNode;
            AVLNode oldLeftNode = rootNode.LeftNode;
            rootNode = oldLeftNode.RightNode;
            oldRootNode.LeftNode = rootNode.RightNode;
            oldLeftNode.RightNode = rootNode.LeftNode;
            rootNode.LeftNode = oldLeftNode;
            rootNode.RightNode = oldRootNode;
            rootNode.LeftNode.UpdateHeight();
            rootNode.RightNode.UpdateHeight();
            rootNode.UpdateHeight();
        }
        private void balance()
        {
            rootNode.UpdateHeight();
            if (rootNode.BalanceFactor() < -1)
            {
                if (rootNode.LeftNode.BalanceFactor() > 0) rotateRightLeft();
                else rotateRight();
            }
            else if (rootNode.BalanceFactor() > 1)
            {
                if (rootNode.RightNode.BalanceFactor() < 0) rotateLeftRight();
                else rotateLeft();
            }
        }
    }
}
