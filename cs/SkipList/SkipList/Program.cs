using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipList
{
    public class SkipNode<T> : IComparable<SkipNode<T>>, IComparable<T> where T : IComparable<T>
    {
        public T Value;
        private int height;
        public int Height
        {
            get => height;
            set
            {
                height = value;
                SkipNode<T>[] newNext = new SkipNode<T>[height];
                int x = height > Next.Length ? Next.Length : height;
                for (int i = 0; i < x; i++)
                    newNext[i] = Next[i];
                Next = newNext;
            }
        }
        public SkipNode<T>[] Next;
        public SkipNode(int height)
        {
            this.height = height;
            Next = new SkipNode<T>[height];
        }
        public SkipNode(int height, SkipNode<T>[] next)
        {
            this.height = height;
            Next = next;
        }
        public SkipNode(T value, int height, SkipNode<T>[] next)
        {
            Value = value;
            this.height = height;
            Next = next;
        }
        public int CompareTo(SkipNode<T> other)
        {
            if (other == null) return 1;
            return Value.CompareTo(other.Value);
        }

        public int CompareTo(T other)
        {
            if (other == null) return 1;
            return Value.CompareTo(other);
        }
    }
    public class SkipList<T> : ICollection<T> where T : IComparable<T>
    {
        public SkipList()
        {
            random = new Random();
            head = new SkipNode<T>(1);
            Count = 0;
        }
        public int Count { get; private set; }

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(T item)
        {
            int height = chooseRandomHeight();
            SkipNode<T> newNode = new SkipNode<T>(item, height, new SkipNode<T>[height]);
            if (height > head.Height) head.Height = height;
            void add(SkipNode<T> node, int level)
            {
                if (node.Next[level] == null)
                {
                    if (level == 0) node.Next[0] = newNode;
                    else
                    {
                        add(node, level - 1);
                        if (height > level) node.Next[level] = newNode;
                    }
                }
                else if (node.Next[level].CompareTo(newNode) > 0)
                {
                    if (level == 0)
                    {
                        newNode.Next[0] = node.Next[0];
                        node.Next[0] = newNode;
                    }
                    else
                    {
                        add(node, level - 1);
                        if (height > level)
                        {
                            newNode.Next[level] = node.Next[level];
                            node.Next[level] = newNode;
                        }
                    }
                }
                else add(node.Next[level], level);
            }
            add(head, head.Height - 1);
            Count++;
        }

        public void Clear()
        {
            head = new SkipNode<T>(1);
            Count = 0;
        }

        public bool Contains(T item)
        {
            bool contains(SkipNode<T> node, int level)
            {
                if (node.Value.CompareTo(item) == 0 && node != head) return true;
                else if (node.Next[level] == null)
                {
                    if (level == 0) return false;
                    else return contains(node, level - 1);
                }
                else if (node.Next[level].CompareTo(item) > 0)
                {
                    if (level == 0) return false;
                    else return contains(node.Next[level], level - 1);
                }
                else return contains(node.Next[level], level);
            }
            return contains(head, head.Height - 1);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex + Count >= array.Length) throw new IndexOutOfRangeException();
            SkipNode<T> node = head.Next[0];
            for(int i = 0; i < Count; i++)
            {
                array[i + arrayIndex] = node.Value;
                node = node.Next[0];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            SkipNode<T> node = head.Next[0];
            for (int i = 0; i < Count; i++)
            {
                yield return node.Value;
                node = node.Next[0];
            }
        }

        public bool Remove(T item)
        {
            bool remove(SkipNode<T> node, int level)
            {
                if (node.Next[level] == null)
                {
                    if (level == 0) return false;
                    else return remove(node, level - 1);
                }
                else if (node.Next[level].CompareTo(item) > 0)
                {
                    if (level == 0) return false;
                    else return remove(node, level - 1);
                }
                else if(node.Next[level].CompareTo(item) == 0)
                {
                    node.Next[level] = node.Next[level].Next[level];
                    if (node.Next[level] == null && node == head) head.Height--;
                    if(level > 0) remove(node, level - 1);
                    return true;
                }
                else return remove(node.Next[level], level);
            }
            if(remove(head, head.Height - 1))
            {
                Count--;
                return true;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            SkipNode<T> node = head.Next[0];
            for (int i = 0; i < Count; i++)
            {
                yield return node;
                node = node.Next[0];
            }
        }

        public void Print()
        {
            int y = Console.CursorTop;
            try
            {
                for (int i = 0; i < head.Height; i++)
                    Console.WriteLine("H");
                y = Console.CursorTop - 1;
                SkipNode<T> node = head.Next[0];
                for (int i = 0; i < Count; i++)
                {
                    Console.CursorLeft += 2;
                    Console.CursorTop = y;
                    for (int j = 0; j < node.Height; j++)
                    {
                        Console.Write(node.Value);
                        Console.CursorTop--;
                        Console.CursorLeft -= node.Value.ToString().Length;
                    }
                    Console.CursorLeft += node.Value.ToString().Length;
                    node = node.Next[0];
                }
                Console.SetCursorPosition(0, y + 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.SetCursorPosition(0, y + 2);
                Console.WriteLine("Console was too small to fully display skipList");
            }
        }

        private int chooseRandomHeight()
        {
            int height = 1;
            while (random.Next(2) == 1 && height < head.Height + 1)
                height++;
            return height;
        }
        private SkipNode<T> head;
        private Random random;
    }
    class Program
    {
        static int[] valuesFromString(string input)
        {
            int[] output = new int[0];
            int value = 0;
            int negativeMultiplier = 1;
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                {
                    value = 10 * value + negativeMultiplier * Convert.ToInt32(input[i].ToString());
                }
                else if (input[i] == '-')
                {
                    negativeMultiplier = -negativeMultiplier;
                }
                else if (value != 0)
                {
                    if (output.Length == 0) output = new int[1] { value };
                    else
                    {
                        int[] oldValues = output;
                        output = new int[output.Length + 1];
                        for (int j = 0; j < oldValues.Length; j++)
                        {
                            output[j] = oldValues[j];
                        }
                        output[oldValues.Length] = value;
                    }
                    value = 0;
                    negativeMultiplier = 1;
                }
            }
            if (value != 0)
            {
                if (output.Length == 0) output = new int[1] { value };
                else
                {
                    int[] oldValues = output;
                    output = new int[output.Length + 1];
                    for (int j = 0; j < oldValues.Length; j++)
                    {
                        output[j] = oldValues[j];
                    }
                    output[oldValues.Length] = value;
                }
            }
            return output;
        }
        static void Main(string[] args)
        {
            SkipList<int> skipList = new SkipList<int>();
            while (true)
            {
                Console.WriteLine("Type Command: Insert [ins], Delete [del], Minimum [min], Maximum [max], Search [sea], Display [dis], Generate random skip list [gen]");
                string command = Console.ReadLine();
                if (command.Length < 3) continue;
                int number;
                switch (command.ToLower().Substring(0, 3))
                {
                    case "ins":
                        if (command.Length > 4)
                        {
                            int[] numbers = valuesFromString(command.Substring(4, command.Length - 4));
                            if (numbers == null) continue;
                            Console.Write($"Inserted values ");
                            for (int i = 0; i < numbers.Length; i++)
                            {
                                skipList.Add(numbers[i]);
                                Console.Write(numbers[i] + ", ");
                            }
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Type number to insert:");
                            number = Convert.ToInt32(Console.ReadLine());
                            skipList.Add(number);
                            Console.WriteLine($"Inserted value {number}");
                        }
                        break;
                    case "del":
                        if (command.Length > 4)
                        {
                            int[] numbers = valuesFromString(command.Substring(4, command.Length - 4));
                            if (numbers == null) continue;
                            for (int i = 0; i < numbers.Length; i++)
                                Console.WriteLine($"Deleting value {numbers[i]}: {skipList.Remove(numbers[i]).ToString()}");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Type number to delete:");
                            number = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine($"Deleting value {number}: {skipList.Remove(number).ToString()}");
                        }
                        break;
                    case "sea":
                        if (command.Length > 3) number = Convert.ToInt32(command.Substring(4, command.Length - 4));
                        else
                        {
                            Console.WriteLine("Type value to search for:");
                            number = Convert.ToInt32(Console.ReadLine());
                        }
                        bool contains = skipList.Contains(number);
                        Console.WriteLine($"Skip list {(contains ? "contains" : "does not contain")} the value {number}");
                        break;
                    case "dis":
                        skipList.Print();
                        break;
                    case "gen":
                        skipList = new SkipList<int>();
                        for (int i = 1; i < 51; i++)
                            skipList.Add(i * 2);
                        break;
                }
            }
        }
    }
}
