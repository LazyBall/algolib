using System;
using System.Collections.Generic;

namespace Trees
{
    public class AVLTree<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        public int Count { get; private set; }

        private int Height
        {
            get
            {
                return (int)(1.45 * (Math.Log(Count + 2) / Math.Log(2)));
            }
        }

        private class Node<NTKey, NTValue>
        {
            public NTKey Key { get; private set; }
            public NTValue Value { get; set; }
            public Node<NTKey, NTValue> Left { get; set; }
            public Node<NTKey, NTValue> Right { get; set; }
            public int Height { get; set; }

            public Node(NTKey key, NTValue value, Node<NTKey, NTValue> left = null,
                Node<NTKey, NTValue> right = null, int height = 1)
            {
                this.Key = key;
                this.Value = value;
                this.Left = left;
                this.Right = right;
                this.Height = height;
            }
        }

        private Node<TKey, TValue> Root { get; set; }

        private int GetNodeHeight(Node<TKey, TValue> node)
        {
            return (node != null) ? node.Height : 0;
        }

        private int GetBalancingFactor(Node<TKey, TValue> node)
        {
            if (node == null) return 0;
            return GetNodeHeight(node.Right) - GetNodeHeight(node.Left);
        }

        private void FixNodeHeight(Node<TKey, TValue> node)
        {
            if (node == null) return;
            node.Height = Math.Max(GetNodeHeight(node.Left), GetNodeHeight(node.Right)) + 1;
        }

        private void CheckKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key is null.");
            }
        }

        private Node<TKey, TValue> FindNode(TKey key)
        {
            CheckKey(key);
            var current = Root;

            while (current != null)
            {
                int comparisonResult = current.Key.CompareTo(key);
                if (comparisonResult > 0)
                {
                    current = current.Left;
                }
                else if (comparisonResult < 0)
                {
                    current = current.Right;
                }
                else
                {
                    return current;
                }
            }

            return null;
        }

        private Node<TKey, TValue> RotateRight(Node<TKey, TValue> node)
        {
            var left = node.Left;
            node.Left = left.Right;
            left.Right = node;
            FixNodeHeight(node);
            FixNodeHeight(left);
            return left;
        }

        private Node<TKey, TValue> RotateLeft(Node<TKey, TValue> node)
        {
            var right = node.Right;
            node.Right = right.Left;
            right.Left = node;
            FixNodeHeight(node);
            FixNodeHeight(right);
            return right;
        }

        private Node<TKey, TValue> Balance(Node<TKey, TValue> node)
        {
            FixNodeHeight(node);
            var balanceFactor = GetBalancingFactor(node);
            if (balanceFactor == 2)
            {
                if (GetBalancingFactor(node.Right) < 0)
                {
                    node.Right = RotateRight(node.Right);
                }
                return RotateLeft(node);
            }
            else if (balanceFactor == -2)
            {
                if (GetBalancingFactor(node.Left) > 0)
                {
                    node.Left = RotateLeft(node.Left);
                }
                return RotateRight(node);
            }
            return node;
        }

        public void Add(TKey key, TValue value)
        {
            CheckKey(key);
            var stack = new Stack<Node<TKey, TValue>>(Height + 2);
            var current = Root;

            while (current != null)
            {
                stack.Push(current);
                int comparisonResult = current.Key.CompareTo(key);
                if (comparisonResult > 0)
                {
                    current = current.Left;
                }
                else if (comparisonResult < 0)
                {
                    current = current.Right;
                }
                else
                {
                    throw new ArgumentException
                       ("An element with the same key already exists in the AVLTree<TKey, TValue>.");
                }
            }

            current = new Node<TKey, TValue>(key, value);

            while (stack.Count > 0)
            {
                var parent = stack.Pop();
                int comparisonResult = current.Key.CompareTo(parent.Key);
                if (comparisonResult < 0)
                {
                    parent.Left = current;
                }
                else
                {
                    parent.Right = current;
                }
                current = Balance(parent);
            }

            Root = current;
            Count++;
        }

        public bool Remove(TKey key)
        {
            CheckKey(key);
            var stack = new Stack<Node<TKey, TValue>>(Height + 2);
            var current = Root;

            while (current != null)
            {
                Node<TKey, TValue> next = null;
                int comparisonResult = current.Key.CompareTo(key);
                if (comparisonResult > 0)
                {
                    next = current.Left;
                }
                else if (comparisonResult < 0)
                {
                    next = current.Right;
                }
                else
                {
                    break;
                }
                stack.Push(current);
                current = next;
            }

            if (current == null)
            {
                return false;
            }
            else
            {
                Node<TKey, TValue> replacement = null; //элемент, который заменит current
                // Если один из сыновей или оба отсутсвуют, то просто заменяем
                // удаляемый элемент на существующего сына, или на null, если сыновей нет
                if (current.Left == null || current.Right == null)
                {
                    replacement = current.Left ?? current.Right;
                }
                // Если оба сына есть, то ищем элемент, который будет следующим после current
                // при обходе дерева (successor) и заменяем им текущий узел
                else
                {
                    var successor = current.Right;
                    Node<TKey, TValue> successorParent = null;
                    var queueSuccessor = new Queue<Node<TKey, TValue>>(Height - stack.Count + 2);

                    while (successor.Left != null)
                    {
                        queueSuccessor.Enqueue(successor);
                        successorParent = successor;
                        successor = successor.Left;
                    }

                    if (successorParent != null)
                    {
                        successorParent.Left = successor.Right;
                    }
                    else
                    {
                        current.Right = successor.Right;
                    }

                    replacement = successor;
                    replacement.Left = current.Left;
                    replacement.Right = current.Right;
                    stack.Push(successor);

                    while (queueSuccessor.Count > 0)
                    {
                        stack.Push(queueSuccessor.Dequeue());
                    }

                }

                Node<TKey, TValue> temp = null;
                if (stack.Count > 0)
                {
                    temp = stack.Pop();
                    temp = Balance(temp);
                    while (stack.Count > 0)
                    {
                        var parent = stack.Pop();
                        var comparisonResult = parent.Key.CompareTo(temp.Key);
                        if (comparisonResult > 0)
                        {
                            parent.Left = temp;
                        }
                        else
                        {
                            parent.Right = temp;
                        }
                        temp = Balance(parent);
                    }
                }
                Root = temp;
            }
            Count--;
            return true;
        }

    }
}