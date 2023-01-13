using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BTree
{
    internal class Node23<T> where T : IComparable<T>
    {
        public List<T> Values { get; set; }
        public List<Node23<T>> Children { get; set; }

        public Node23(T value)
        {
            Values = new List<T>(2) { value };
            Children = new List<Node23<T>>(3);
        }

        public bool IsLeaf() => Children.Count == 0;
        public void Print(int indent)
        {
            Console.Write(new string(' ', indent));
            foreach (T val in Values)
            {
                Console.Write(val.ToString() + ' ');
            }
            Console.WriteLine();
            foreach (Node23<T> child in Children)
            {
                child.Print(indent + 1);
            }
        }
        public override string ToString()
        {
            return Values.Aggregate("", (str, val) => str + val.ToString() + " ");
        }
    }

    internal class Tree23<T> where T : IComparable<T>
    {
        private Node23<T>? Root = null;

        public void Print()
        {
            Root?.Print(0);
        }

        public void Insert(T value)
        {
            if (Root == null)
            {
                Root = new Node23<T>(value);
            }
            else
            {
                InsertHelper(value, Root);
            }
        }

        //Returns true iff it expanded upwards
        private bool InsertHelper(T newVal, Node23<T> curr)
        {
            //If leaf, then fatten leaf
            if (curr.IsLeaf())
            {
                //Fatten
                if (newVal.CompareTo(curr.Values[0]) < 0)
                {
                    curr.Values.Insert(0, newVal);
                }
                else if (curr.Values.Count == 2 && newVal.CompareTo(curr.Values[1]) < 1)
                {
                    curr.Values.Insert(1, newVal);
                }
                else
                {
                    curr.Values.Add(newVal);
                }

                //If leaf is too big, split
                if (curr.Values.Count == 3)
                {
                    T left = curr.Values[0];
                    T right = curr.Values[2];
                    curr.Values.RemoveAt(0);
                    curr.Values.RemoveAt(1);
                    curr.Children.Add(new Node23<T>(left));
                    curr.Children.Add(new Node23<T>(right));
                    return true;
                }
                return false;
            }

            //If not leaf, add to child
            for (int i = 0; i < curr.Children.Count; i++)
            {
                if (i == curr.Children.Count - 1 || newVal.CompareTo(curr.Values[i]) < 0)
                {
                    Node23<T> child = curr.Children[i];
                    bool childGrewUp = InsertHelper(newVal, child);

                    //If child expanded upward, absorb it
                    if (childGrewUp)
                    {
                        curr.Values.Insert(i, child.Values[0]);
                        curr.Children.RemoveAt(i);
                        curr.Children.Insert(i, child.Children[0]);
                        curr.Children.Insert(i + 1, child.Children[1]);
                    }
                    break;
                }
            }

            //If too fat, split and expand up
            if (curr.Values.Count == 3)
            {
                Node23<T> leftChild = new(curr.Values[0]);
                leftChild.Children.Add(curr.Children[0]);
                leftChild.Children.Add(curr.Children[1]);

                Node23<T> rightChild = new(curr.Values[2]);
                rightChild.Children.Add(curr.Children[2]);
                rightChild.Children.Add(curr.Children[3]);

                curr.Values.RemoveAt(0);
                curr.Values.RemoveAt(1);
                curr.Children.Clear();
                curr.Children.Add(leftChild);
                curr.Children.Add(rightChild);
                return true;
            }
            return false;
        }

        //Returns true if found
        public bool Remove(T value)
        {
            if (Root == null) return false;

            bool found = RemoveHelper(value, Root);
            if (!found)
            {
                return false;
            }

            if (Root.Values.Count == 0)
            {
                Root = Root.Children[0];
            }
            return true;
        }

        //Returns whether the value was found
        private bool RemoveHelper(T value, Node23<T> curr)
        {
            //Leaf: shrink leaf
            if (curr.IsLeaf())
            {
                return curr.Values.Remove(value);
            }

            //Non-leaf step 1: Recurse to leaf
            for (int i = 0; i < curr.Children.Count; i++)
            {
                bool isLast = i == curr.Children.Count - 1;

                int compare;
                if (isLast || (compare = value.CompareTo(curr.Values[i])) < 0)
                {
                    //Value is not in curr: recurse to children and fixup
                    Node23<T> child = curr.Children[i];
                    bool found = RemoveHelper(value, child);

                    if (!found) return false;

                    Fixup(curr, i);
                    return true;
                }
                else if (compare == 0)
                {
                    //Value is in curr: replace with largest value in subtree to the left of the value & fixup
                    Node23<T> child = curr.Children[i];
                    T largestLeft = RemoveLargest(child);
                    curr.Values[i] = largestLeft;

                    Fixup(curr, i);
                    return true;
                }
            }
            throw new InvalidOperationException("Shouldn't get here?");
        }

        //Returns larest value
        private T RemoveLargest(Node23<T> curr)
        {
            //Leaf: Remove largest value in leaf
            if (curr.IsLeaf())
            {
                T largest = curr.Values[^1];
                curr.Values.RemoveAt(curr.Values.Count - 1);
                return largest;
            }

            //Non-leaf: recurse & fixup
            T largestVal = RemoveLargest(curr.Children[^1]);

            Fixup(curr, curr.Children.Count - 1);
            return largestVal;
        }

        private void Fixup(Node23<T> parent, int childIdx)
        {
            Node23<T> node = parent.Children[childIdx];
            if (node.Values.Count > 0) return; //No need to fixup

            //Try stealing from the left sibling
            if (childIdx > 0 && parent.Children[childIdx - 1].Values.Count > 1)
            {
                Node23<T> leftSibling = parent.Children[childIdx - 1];

                T siblingVal = leftSibling.Values[^1];
                leftSibling.Values.RemoveAt(leftSibling.Values.Count - 1);

                T parentVal = parent.Values[childIdx - 1];
                parent.Values[childIdx - 1] = siblingVal;

                node.Values.Insert(0, parentVal);

                if (!leftSibling.IsLeaf())
                {
                    Node23<T> nibling = leftSibling.Children[^1];
                    leftSibling.Children.RemoveAt(leftSibling.Children.Count - 1);
                    node.Children.Insert(0, nibling);
                }

            }
            else if (childIdx < parent.Children.Count - 1 && parent.Children[childIdx + 1].Values.Count > 1) //Try stealing from right sibling
            {
                Node23<T> rightSibling = parent.Children[childIdx + 1];

                T siblingVal = rightSibling.Values[0];
                rightSibling.Values.RemoveAt(0);

                T parentVal = parent.Values[childIdx];
                parent.Values[childIdx] = siblingVal;

                node.Values.Add(parentVal);

                if (!rightSibling.IsLeaf())
                {
                    Node23<T> nibling = rightSibling.Children[0];
                    rightSibling.Children.RemoveAt(0);
                    node.Children.Add(nibling);
                }
            }
            else if (childIdx > 0) //Try dropping down the parent value to merge with the left sibling
            {
                Node23<T> leftSibling = parent.Children[childIdx - 1];

                T parentVal = parent.Values[childIdx - 1];
                parent.Values.RemoveAt(childIdx - 1);
                parent.Children.RemoveAt(childIdx);
                leftSibling.Values.Add(parentVal);

                if (!leftSibling.IsLeaf())
                {
                    leftSibling.Children.Add(node.Children[0]);
                }
            }
            else //Drop down the parent value to merge with the right sibling
            {
                Node23<T> rightSibling = parent.Children[childIdx + 1];

                T parentVal = parent.Values[childIdx];
                parent.Values.RemoveAt(childIdx);
                parent.Children.RemoveAt(childIdx);
                rightSibling.Values.Insert(0, parentVal);

                if (!rightSibling.IsLeaf())
                {
                    rightSibling.Children.Insert(0, node.Children[0]);
                }
            }
        }

        public bool Contains(T value)
        {
            Node23<T> curr = Root;
            while (true)
            {
                if (curr.IsLeaf()) return curr.Values.Contains(value);

                for (int i = 0; i < curr.Children.Count; i++)
                {
                    if (i == curr.Children.Count - 1)
                    {
                        curr = curr.Children[^1];
                        break;
                    }

                    int compare = value.CompareTo(curr.Values[i]);
                    if (compare < 0)
                    {
                        curr = curr.Children[i];
                    }
                    else if (compare == 0)
                    {
                        return true;
                    }
                }
            }
        }
    }
}
