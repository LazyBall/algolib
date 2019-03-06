

namespace Trees
{
    internal class Node<TKey, TValue>
    {
        public TKey Key { get; private set; }
        public TValue Value { get; set; }
        public Node<TKey, TValue> Left { get; set; }
        public Node<TKey, TValue> Right { get; set; }


        public Node(TKey key, TValue value, Node<TKey, TValue> right = null, Node<TKey, TValue> left = null)
        {
            this.Key = key;
            this.Value = value;
            this.Left = left;
            this.Right = right;
        }
    }
}