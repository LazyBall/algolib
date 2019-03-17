using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class AVLTree<TKey,TValue> 
        where TKey:IComparable<TKey>
    {

        private class Node<NTKey, NTValue>
        {
            public NTKey Key { get; private set; }
            public NTValue Value { get; set; }
            public Node<NTKey, NTValue> Left { get; set; }
            public Node<NTKey, NTValue> Right { get; set; }
            public Node<NTKey, NTValue> Parent { get; set; }
            public int Height { get; set; }

            public Node(NTKey key, NTValue value, Node<NTKey, NTValue> left = null,
                Node<NTKey, NTValue> right = null, Node<NTKey, NTValue> parent = null, int height = 0)
            {
                this.Key = key;
                this.Value = value;
                this.Left = left;
                this.Right = right;
                this.Parent = parent;
                this.Height = height;
            }
        }

        private int GetHeightNode(Node<TKey,TValue> node)
        {
            return (node != null) ? node.Height : 0;
        }

        private int GetBalanceFactor(Node<TKey,TValue> node)
        {
            if (node == null) return 0;
            return GetHeightNode(node.Right) - GetHeightNode(node.Left);
        }

        private void FixHeightNode(Node<TKey,TValue> node)
        {
            if (node == null) return;
            node.Height = Math.Max(GetHeightNode(node.Left), GetHeightNode(node.Right)) + 1;
        }
    }
}