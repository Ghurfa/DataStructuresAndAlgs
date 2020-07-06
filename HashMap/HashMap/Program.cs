using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    public class KeyComparer<TKey>
    {
        public KeyComparer(Func<TKey, TKey, bool> equals, Func<TKey, int> getHashCode)
        {
            Equals = equals;
            GetHashCode = getHashCode;
        }
        public new Func<TKey, TKey, bool> Equals;
        public new Func<TKey, int> GetHashCode;
    }
    public class HashMap<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
    {
        public HashMap()
        {
            Count = 0;
            keyComparer = new KeyComparer<TKey>((a, b) => a.Equals(b), (obj) => obj.GetHashCode());
            map = new LinkedList<KeyValuePair<TKey, TValue>>[10];
            Keys.Clear();
            Values.Clear();
        }
        public HashMap(KeyComparer<TKey> comparer)
        {
            Count = 0;
            keyComparer = comparer;
            map = new LinkedList<KeyValuePair<TKey, TValue>>[10];
        }
        public TValue this[TKey key]
        {
            get
            {
                int index = keyComparer.GetHashCode(key) % map.Length;
                foreach(KeyValuePair<TKey, TValue> pair in map[index])
                {
                    if (keyComparer.Equals(pair.Key, key)) return pair.Value;
                }
                throw new ArgumentException();
            }
            set
            {
                int index = keyComparer.GetHashCode(key) % map.Length;
                if (map[index] == null)
                {
                    map[index] = new LinkedList<KeyValuePair<TKey, TValue>>();
                    map[index].AddFirst(new KeyValuePair<TKey, TValue>(key, value));
                    checkSize();
                    return;
                }
                for(var node = map[index].First; node != null; node = node.Next)
                {
                    if (keyComparer.Equals(node.Value.Key, key))
                    {
                        node.Value = new KeyValuePair<TKey, TValue>(key, value);
                        return;
                    }
                }
                map[index].AddFirst(new KeyValuePair<TKey, TValue>(key, value));
                checkSize();
            }
        }

        public ICollection<TKey> Keys { get; private set; }

        public ICollection<TValue> Values { get; private set; }

        public int Count { get; private set; }

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(TKey key, TValue value)
        {
            int index = keyComparer.GetHashCode(key) % map.Length;
            if (map[index] == null)
            {
                map[index] = new LinkedList<KeyValuePair<TKey, TValue>>();
                map[index].AddFirst(new KeyValuePair<TKey, TValue>(key, value));
            }
            else
            {
                for (LinkedListNode<KeyValuePair<TKey, TValue>> node = map[index].First; node != null; node = node.Next)
                {
                    if (keyComparer.Equals(node.Value.Key, key)) throw new ArgumentException();
                }
                map[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
            }
            Keys.Add(key);
            Values.Add(value);
            Count++;
            checkSize();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            int index = keyComparer.GetHashCode(item.Key) % map.Length;
            if (map[index] == null)
            {
                map[index] = new LinkedList<KeyValuePair<TKey, TValue>>();
                map[index].AddFirst(item);
            }
            else
            {
                for (LinkedListNode<KeyValuePair<TKey, TValue>> node = map[index].First; node != null; node = node.Next)
                {
                    if (keyComparer.Equals(node.Value.Key, item.Key)) throw new ArgumentException();
                }
                map[index].AddLast(item);
            }
            Keys.Add(item.Key);
            Values.Add(item.Value);
            Count++;
            checkSize();
        }

        public void Clear()
        {
            Count = 0;
            map = new LinkedList<KeyValuePair<TKey, TValue>>[10];
            Keys.Clear();
            Values.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            int index = keyComparer.GetHashCode(item.Key) % map.Length;
            if (map[index] == null) return false;
            return map[index].Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            int index = keyComparer.GetHashCode(key) % map.Length;
            if (map[index] == null) return false;
            foreach(KeyValuePair<TKey, TValue> pair in map[index])
            {
                if (keyComparer.Equals(pair.Key, key)) return true;
            }
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (arrayIndex + Count >= array.Length) throw new InvalidOperationException();
            int index = arrayIndex;
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == null) continue;
                foreach (KeyValuePair<TKey, TValue> pair in map[i])
                {
                    array[arrayIndex] = pair;
                    index++;
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for(int i = 0; i < map.Length; i++)
            {
                if (map[i] == null) continue;
                foreach(KeyValuePair<TKey, TValue> pair in map[i])
                {
                    yield return pair;
                }
            }
        }

        public bool Remove(TKey key)
        {
            int index = keyComparer.GetHashCode(key) % map.Length;
            if (map[index] == null) return false;
            for(LinkedListNode<KeyValuePair<TKey, TValue>> node = map[index].First; node != null; node = node.Next)
            {
                if (keyComparer.Equals(key, node.Value.Key))
                {
                    map[index].Remove(node);
                    return true;
                }
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            int index = keyComparer.GetHashCode(item.Key) % map.Length;
            if (map[index] == null) return false;
            for (LinkedListNode<KeyValuePair<TKey, TValue>> node = map[index].First; node != null; node = node.Next)
            {
                if (keyComparer.Equals(item.Key, node.Value.Key))
                {
                    map[index].Remove(node);
                    return true;
                }
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null) throw new ArgumentNullException();
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == null) continue;
                foreach (KeyValuePair<TKey, TValue> pair in map[i])
                {
                    if (keyComparer.Equals(pair.Key, key))
                    {
                        value = pair.Value;
                        return true;
                    }
                }
            }
            value = default(TValue);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == null) continue;
                foreach (KeyValuePair<TKey, TValue> pair in map[i])
                {
                    yield return pair;
                }
            }
        }

        public void PrintMap()
        {
            for (int i = 0; i < map.Length; i++)
            {
                var bucket = map[i];
                Console.WriteLine();
                Console.Write($"{i}\t: ");
                if (bucket == null) continue;
                foreach (var element in bucket)
                    Console.Write($"{element.Value} ");
            }
            Console.WriteLine();
        }

        private void checkSize()
        {
            if (Count == map.Length) resizeMap();
        }
        private void resizeMap()
        {
            var newMap = new LinkedList<KeyValuePair<TKey, TValue>>[map.Length * 2];
            for(int i = 0; i < map.Length; i++)
                if(map[i] != null) newMap[keyComparer.GetHashCode(map[i].First.Value.Key) % newMap.Length] = map[i];
            map = newMap;
        }

        private LinkedList<KeyValuePair<TKey, TValue>>[] map;
        private KeyComparer<TKey> keyComparer;
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
            HashMap<int, int> hashMap = new HashMap<int, int>();
            Console.WriteLine("Type Command: Insert [ins], Delete [del], Contains Key-Value pair [ckv], Contains key [con], Clear [clr], Print [prn]");
            while (true)
            {
                string command = Console.ReadLine();
                if (command.Length < 3) continue;
                int number;
                int key;
                switch (command.ToLower().Substring(0, 3))
                {
                    case "ins":
                        Console.WriteLine("Type key to insert at:");
                        try
                        {
                            key = Convert.ToInt32(Console.ReadLine());
                        }
                        catch(FormatException)
                        {
                            Console.WriteLine("Invalid Input");
                            continue;
                        }
                        Console.WriteLine("Type value to insert:");
                        try
                        {
                            number = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid Input");
                            continue;
                        }
                        hashMap.Add(key, number);
                        Console.WriteLine($"Inserted value {number}");
                        break;
                    case "del":
                        if (command.Length > 4)
                        {
                            int[] keys = valuesFromString(command.Substring(4, command.Length - 4));
                            if (keys == null) continue;
                            for (int i = 0; i < keys.Length; i++)
                                Console.WriteLine($"Deleting key {keys[i]}: {hashMap.Remove(keys[i]).ToString()}");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Type key to delete:");
                            try
                            {
                                key = Convert.ToInt32(Console.ReadLine());
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Invalid Input");
                                continue;
                            }
                            Console.WriteLine($"Deleting key {key}: {hashMap.Remove(key).ToString()}");
                        }
                        break;
                    case "ckv":
                        Console.WriteLine("Type key:");
                        try
                        {
                            key = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid Input");
                            continue;
                        }
                        Console.WriteLine("Type value:");
                        try
                        {
                            number = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid Input");
                            continue;
                        }
                        bool contains = hashMap.Contains(new KeyValuePair<int, int>(key, number));
                        Console.WriteLine($"Hash map {(contains ? "contains" : "does not contain")} the key-value pair {key}-{number}");
                        break;
                    case "con":
                        Console.WriteLine("Type key:");
                        try
                        {
                            key = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid Input");
                            continue;
                        }
                        contains = hashMap.ContainsKey(key);
                        Console.WriteLine($"Hash map {(contains ? "contains" : "does not contain")} the key {key}");
                        break;
                    case "clr":
                        hashMap.Clear();
                        Console.WriteLine("Cleared Hash Map");
                        break;
                    case "prn":
                        hashMap.PrintMap();
                        break;
                }
            }
        }
    }
}
