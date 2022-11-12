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
        private List<TKey> keys;
        private List<TValue> values;
        private LinkedList<KeyValuePair<TKey, TValue>>[] map;
        private KeyComparer<TKey> keyComparer;

        public ICollection<TKey> Keys => keys.AsReadOnly();
        public ICollection<TValue> Values => values.AsReadOnly();
        public int Count { get; private set; }
        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get
            {
                int index = keyComparer.GetHashCode(key) % map.Length;
                foreach (KeyValuePair<TKey, TValue> pair in map[index])
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
                for (var node = map[index].First; node != null; node = node.Next)
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

        public HashMap()
        {
            Count = 0;
            keyComparer = new KeyComparer<TKey>((a, b) => a.Equals(b), (obj) => obj.GetHashCode());
            map = new LinkedList<KeyValuePair<TKey, TValue>>[10];
            keys = new List<TKey>();
            values = new List<TValue>();
        }
        public HashMap(KeyComparer<TKey> comparer)
        {
            Count = 0;
            keyComparer = comparer;
            map = new LinkedList<KeyValuePair<TKey, TValue>>[10];
        }

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
            keys.Add(key);
            values.Add(value);
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
            keys.Add(item.Key);
            values.Add(item.Value);
            Count++;
            checkSize();
        }

        public void Clear()
        {
            Count = 0;
            map = new LinkedList<KeyValuePair<TKey, TValue>>[10];
            keys.Clear();
            values.Clear();
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
            foreach (KeyValuePair<TKey, TValue> pair in map[index])
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
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == null) continue;
                foreach (KeyValuePair<TKey, TValue> pair in map[i])
                {
                    yield return pair;
                }
            }
        }

        public bool Remove(TKey key)
        {
            int index = keyComparer.GetHashCode(key) % map.Length;
            if (map[index] == null) return false;
            for (LinkedListNode<KeyValuePair<TKey, TValue>> node = map[index].First; node != null; node = node.Next)
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
            for (int i = 0; i < map.Length; i++)
                if (map[i] != null) newMap[keyComparer.GetHashCode(map[i].First.Value.Key) % newMap.Length] = map[i];
            map = newMap;
        }
    }
}
