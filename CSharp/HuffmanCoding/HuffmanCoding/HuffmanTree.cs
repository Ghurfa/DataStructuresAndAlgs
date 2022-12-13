using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCoding
{
    internal class HuffmanTree
    {
        private abstract class Node
        {
            public int Frequency;
            public Node(int frequency)
            {
                Frequency = frequency;
            }
        }

        private class MidNode : Node
        {
            public Node Child0;
            public Node Child1;
            public MidNode(Node child0, Node child1)
                : base(child0.Frequency + child1.Frequency)
            {
                Child0 = child0;
                Child1 = child1;
            }
        }

        private class LeafNode : Node
        {
            public char Value;
            public LeafNode(char value, int freq)
                : base(freq)
            {
                Value = value;
            }
        }

        private class TerminatorNode : Node
        {
            public TerminatorNode() : base(1) { }
        }


        private Node Root;
        private Dictionary<char, bool[]> Encodings;
        private bool[] TerminatorEncoding;
        private HuffmanTree(Node root)
        {
            Root = root;

            //Build encodings dictionary
            if (root is TerminatorNode)
            {
                Encodings = new Dictionary<char, bool[]>();
            }
            else if (root is LeafNode leafRoot)
            {
                Encodings = new Dictionary<char, bool[]>()
                {
                    { leafRoot.Value, new bool[] {false} }
                };
            }
            else if (root is MidNode midRoot)
            {
                Encodings = new Dictionary<char, bool[]>();

                //Traverse tree depth-first, using stack to keep track of parents
                List<(Node Node, bool IsChild1)> stack = new();
                stack.Add((midRoot.Child0, false));
                while (stack.Count > 0)
                {
                    (Node node, bool isChild0) = stack[stack.Count - 1];
                    if (node is MidNode midNode)
                    {
                        stack.Add((midNode.Child0, false));
                    }
                    else
                    {
                        if (node is LeafNode leafNode)
                        {
                            Encodings.Add(leafNode.Value, stack.Select(x => x.IsChild1).ToArray());
                        }
                        else
                        {
                            TerminatorEncoding = stack.Select(x => x.IsChild1).ToArray();
                        }

                        do
                        {
                            (Node parNode, bool parIsChild1) = stack[stack.Count - 1];
                            stack.RemoveAt(stack.Count - 1);
                            if (!parIsChild1)
                            {
                                Node child1 = stack.Count > 0 ? ((MidNode)stack[stack.Count - 1].Node).Child1 : midRoot.Child1;
                                stack.Add((child1, true));
                                break;
                            }
                        } while (stack.Count > 0);
                    }
                }
            }
        }

        public ReadOnlySpan<byte> Encode(ReadOnlySpan<char> text)
        {
            //Get bits
            List<bool> bits = new();
            foreach(char ch in text)
            {
                bits.AddRange(Encodings[ch]);
            }
            bits.AddRange(TerminatorEncoding);

            //Convert bit list to bytes
            byte[] retArr = new byte[(int)Math.Ceiling(bits.Count / 8.0)];
            for(int i = 0; i < bits.Count - 7; i += 8)
            {
                byte val = 0;
                for(int x = 0; x < 8; x++)
                {
                    val <<= 1;
                    val |= (byte)(bits[i + x] ? 1 : 0);
                }
                retArr[i / 8] = val;
            }

            int tailLen = bits.Count % 8;
            if (tailLen > 0)
            {
                byte tailVal = 0;
                for (int i = 0; i < tailLen; i++)
                {
                    tailVal <<= 1;
                    tailVal |= (byte)(bits[bits.Count - tailLen + i] ? 1 : 0);
                }
                tailVal <<= 8 - tailLen;
                retArr[^1] = tailVal;
            }

            return retArr.AsSpan();
        }

        public ReadOnlySpan<char> Decode(ReadOnlySpan<byte> bytes)
        {
            StringBuilder builder = new StringBuilder();
            int bitIdx = 0;

            while (bitIdx < bytes.Length * 8)
            {
                Node currNode = Root;
                while(currNode is MidNode midNode)
                {
                    int lrgIdx = bitIdx / 8;
                    int shiftAmt = 7 - (bitIdx % 8);
                    bool bit = ((bytes[lrgIdx] >> shiftAmt) & 1) == 1;
                    currNode = bit ? midNode.Child1 : midNode.Child0;
                    bitIdx++;
                    if(bitIdx >= bytes.Length * 8)
                    {
                        throw new ArgumentException("Bytes does not contain a terminator sequence");
                    }
                }

                if(currNode is LeafNode leafNode)
                {
                    builder.Append(leafNode.Value);
                }
                else if(currNode is TerminatorNode)
                {
                    return builder.ToString();
                }
            }

            throw new ArgumentException("Bytes does not contain a terminator sequence");
        }

        public static HuffmanTree CreateTree(ReadOnlySpan<char> text)
        {
            Dictionary<char, int> freqMap = MakeFreqMap(text);
            var queueItems = freqMap.Select(x => (new LeafNode(x.Key, x.Value) as Node, x.Value));
            PriorityQueue<Node, int> queue = new(queueItems);
            queue.Enqueue(new TerminatorNode(), 1);

            while(queue.Count > 1)
            {
                Node child0 = queue.Dequeue();
                Node child1 = queue.Dequeue();
                MidNode newNode = new(child0, child1);
                queue.Enqueue(newNode, newNode.Frequency);
            }

            return new HuffmanTree(queue.Dequeue());
        }

        private static Dictionary<char, int> MakeFreqMap(ReadOnlySpan<char> text)
        {
            Dictionary<char, int> freqMap = new();
            foreach(char ch in text)
            {
                if (freqMap.ContainsKey(ch))
                {
                    freqMap[ch]++;
                }
                else
                {
                    freqMap[ch] = 1;
                }
            }

            return freqMap;
        }

    }
}
