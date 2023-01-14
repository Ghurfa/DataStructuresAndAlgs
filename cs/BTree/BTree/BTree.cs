using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BTrees
{
    internal class BNode<T> where T : IComparable<T>
    {
        public List<T> Values { get; set; }
        public List<BNode<T>> Children { get; set; }
        public int MaxDegree { get; init; }

        public BNode(T value, int maxDegree)
        {
            Values = new List<T>(maxDegree - 1) { value };
            Children = new List<BNode<T>>(maxDegree);
            MaxDegree = maxDegree;
        }

        private BNode(List<T> values, List<BNode<T>> children, int maxDegree)
        {
            Values = values;
            Children = children;
            MaxDegree = maxDegree;
        }

        public bool IsLeaf() => Children.Count == 0;

        public void Fatten(T newVal)
        {
            for (int i = 0; i < Values.Count; i++)
            {
                if (newVal.CompareTo(Values[i]) < 0)
                {
                    Values.Insert(i, newVal);
                    return;
                }
            }
            Values.Add(newVal);
        }

        public bool SplitIfFat()
        {
            if (Values.Count < MaxDegree) return false;

            // Make children
            int leftDeg = (MaxDegree + 1) / 2;
            List<T> leftVals = Values.GetRange(0, leftDeg - 1);
            List<BNode<T>> leftChildren = IsLeaf() ? new(MaxDegree) : Children.GetRange(0, leftDeg);
            BNode<T> leftChild = new(leftVals, leftChildren, MaxDegree);

            int rightDeg = MaxDegree + 1 - leftDeg;
            List<T> rightVals = Values.GetRange(leftDeg, rightDeg - 1);
            List<BNode<T>> rightChildren = IsLeaf() ? new(MaxDegree) : Children.GetRange(leftDeg, rightDeg);
            BNode<T> rightChild = new(rightVals, rightChildren, MaxDegree);

            // Remake current node
            T middleVal = Values[leftDeg - 1];
            Values.Clear();
            Children.Clear();
            Values.Add(middleVal);
            Children.Add(leftChild);
            Children.Add(rightChild);
            return true;
        }

        public void Print(int indent)
        {
            Console.Write(new string(' ', indent));
            foreach (T val in Values)
            {
                Console.Write(val.ToString() + ' ');
            }
            Console.WriteLine();
            foreach (BNode<T> child in Children)
            {
                child.Print(indent + 1);
            }
        }

        public override string ToString()
        {
            return Values.Aggregate("", (str, val) => str + val.ToString() + " ");
        }
    }

    public class BTree<T> where T : IComparable<T>
    {
        private BNode<T>? Root;
        private readonly int MaxDegree;

        public int Count { get; private set; }

        public BTree(int maxDegree)
        {
            MaxDegree = maxDegree;
            Root = null;
            Count = 0;
        }

        public void Print()
        {
            Root?.Print(0);
        }

        public void Insert(T value)
        {
            if (Root == null)
            {
                Root = new BNode<T>(value, MaxDegree);
            }
            else
            {
                InsertHelper(value, Root);
            }
            Count++;
        }

        // Returns true iff it split
        private bool InsertHelper(T newVal, BNode<T> curr)
        {
            // If leaf, then fatten leaf
            if (curr.IsLeaf())
            {
                curr.Fatten(newVal);
                return curr.SplitIfFat();
            }

            // If not leaf, add to child
            for (int i = 0; i < curr.Children.Count; i++)
            {
                bool isLastChild = i == curr.Children.Count - 1;
                if (isLastChild || newVal.CompareTo(curr.Values[i]) < 0)
                {
                    BNode<T> child = curr.Children[i];
                    bool childGrewUp = InsertHelper(newVal, child);

                    // If child split, absorb it
                    if (childGrewUp)
                    {
                        curr.Values.Insert(i, child.Values[0]);
                        curr.Children.RemoveAt(i);
                        curr.Children.Insert(i, child.Children[0]);
                        curr.Children.Insert(i + 1, child.Children[1]);
                        return curr.SplitIfFat();
                    }
                    return false;
                }
            }

            throw new InvalidOperationException("Unreachable");
        }

        // Returns true if found
        public bool Remove(T value)
        {
            if (Root == null) return false;

            bool found = RemoveHelper(value, Root);
            if (!found) return false;

            if (Root.Values.Count == 0)
            {
                Root = Root.Children[0];
            }
            Count--;
            return true;
        }

        // Returns whether the value was found
        private bool RemoveHelper(T value, BNode<T> curr)
        {
            // Leaf: shrink leaf
            if (curr.IsLeaf())
            {
                return curr.Values.Remove(value);
            }

            // Non-leaf: Recurse to leaf
            for (int i = 0; i < curr.Children.Count; i++)
            {
                bool isLastChild = i == curr.Children.Count - 1;

                int compare;
                if (isLastChild || (compare = value.CompareTo(curr.Values[i])) < 0)
                {
                    // Value is not in curr: recurse to children and fixup
                    BNode<T> child = curr.Children[i];
                    bool found = RemoveHelper(value, child);

                    if (!found) return false;

                    Fixup(curr, i);
                    return true;
                }
                else if (compare == 0)
                {
                    // Value is in curr: replace with largest value in subtree to the left of the value & fixup
                    BNode<T> child = curr.Children[i];
                    T largestLeft = RemoveLargest(child);
                    curr.Values[i] = largestLeft;

                    Fixup(curr, i);
                    return true;
                }
            }
            throw new InvalidOperationException("Unreachable");
        }

        // Returns largest value
        private T RemoveLargest(BNode<T> curr)
        {
            // Leaf: Remove largest value in leaf
            if (curr.IsLeaf())
            {
                T largest = curr.Values[^1];
                curr.Values.RemoveAt(curr.Values.Count - 1);
                return largest;
            }

            // Non-leaf: recurse & fixup
            T largestVal = RemoveLargest(curr.Children[^1]);

            Fixup(curr, curr.Children.Count - 1);
            return largestVal;
        }

        // Fixes any placeholder child nodes created by a remove operation
        private void Fixup(BNode<T> parent, int childIdx)
        {
            BNode<T> node = parent.Children[childIdx];
            if (node.Values.Count > 0) return; // No need to fixup

            if (childIdx > 0 && parent.Children[childIdx - 1].Values.Count > 1) // Try stealing from the left sibling
            {
                BNode<T> leftSibling = parent.Children[childIdx - 1];

                // Node takes from parent
                T parentVal = parent.Values[childIdx - 1];
                node.Values.Insert(0, parentVal);

                // Parent takes from sibling
                T siblingVal = leftSibling.Values[^1];
                leftSibling.Values.RemoveAt(leftSibling.Values.Count - 1);
                parent.Values[childIdx - 1] = siblingVal;

                // Move child of sibling if needed
                if (!leftSibling.IsLeaf())
                {
                    BNode<T> nibling = leftSibling.Children[^1];
                    leftSibling.Children.RemoveAt(leftSibling.Children.Count - 1);
                    node.Children.Insert(0, nibling);
                }
            }
            else if (childIdx < parent.Children.Count - 1 && parent.Children[childIdx + 1].Values.Count > 1) // Try stealing from right sibling
            {
                BNode<T> rightSibling = parent.Children[childIdx + 1];

                // Node takes from parent
                T parentVal = parent.Values[childIdx];
                node.Values.Add(parentVal);

                // Parent takes from sibling
                T siblingVal = rightSibling.Values[0];
                rightSibling.Values.RemoveAt(0);
                parent.Values[childIdx] = siblingVal;

                // Move child of sibling if needed
                if (!rightSibling.IsLeaf())
                {
                    BNode<T> nibling = rightSibling.Children[0];
                    rightSibling.Children.RemoveAt(0);
                    node.Children.Add(nibling);
                }
            }
            else if (childIdx > 0) // Try dropping down the parent value and merging with the left sibling
            {
                BNode<T> leftSibling = parent.Children[childIdx - 1];

                T parentVal = parent.Values[childIdx - 1];
                parent.Values.RemoveAt(childIdx - 1);
                parent.Children.RemoveAt(childIdx);
                leftSibling.Values.Add(parentVal);

                if (!leftSibling.IsLeaf())
                {
                    leftSibling.Children.Add(node.Children[0]);
                }
            }
            else // Drop down the parent value and merge with the right sibling
            {
                BNode<T> rightSibling = parent.Children[childIdx + 1];

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
            if (Root == null) return false;

            BNode<T> curr = Root;
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

        public void Clear()
        {
            Root = null;
            Count = 0;
        }
    }
}
