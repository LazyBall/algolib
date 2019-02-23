using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    internal class Node<TKey, TValue>
            where TKey : IComparable<TKey>
    {
        public TKey Key { get; private set; }
        public TValue Value { get; private set; }
        public Node<TKey, TValue> Left { get; set; }
        public Node<TKey,TValue> Right { get; set; }
   
        public Node(TKey key, TValue value, Node<TKey, TValue> right = null, Node<TKey, TValue> left = null)
        {
            this.Key = key;
            this.Value = value;
            this.Left = left;
            this.Right = right;            
        }       
    }
}