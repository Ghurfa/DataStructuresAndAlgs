using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Min_Heap
{
    public class Heap<T>
    {
        T[] values;
        public Heap()
        {
            values = null;
        }
        public void Insert(T value)
        {
            
        }
        public bool Delete(T value)
        {

        }
        public bool IsEmpty => values.Length == 0;
        public IEnumerable<T> LevelOrder()
        {
        }
        public IEnumerable<T> InOrder()
        {
        }
        public IEnumerable<T> PreOrder()
        {
        }
        public IEnumerable<T> PostOrder()
        {
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
