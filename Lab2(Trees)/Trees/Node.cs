using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    internal class Node<TKey, TValue>
    {
        public TKey Key { get; private set; }
        public TValue Value { get; set; }
        public Node<TKey, TValue> Left { get; set; }
        public Node<TKey, TValue> Right { get; set; }
        public Node<TKey, TValue> Parent { get; set; }

        public Node(TKey key, TValue value, Node<TKey, TValue> right = null,
            Node<TKey, TValue> left = null, Node<TKey, TValue> parent = null)
        {
            this.Key = key;
            this.Value = value;
            this.Left = left;
            this.Right = right;
            this.Parent = parent;
        }
    }
}