using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class BinaryTree<TKey, TValue>
        where TKey:IComparable<TKey>
    {
        public int Count { get; private set; }
        private Node<TKey,TValue> Root { get; set; }


        public void Add(TKey key, TValue value)
        {
            var current = Root;
            while (current != null)
            {
                if (current.Key.CompareTo(key) > 0)
                {
                    current = current.Left;
                }
                else if(current.Key.CompareTo(key)<0)
                {
                    current = current.Right;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            current = new Node<TKey, TValue>(key, value);
            Count++;
        }



    }
}
