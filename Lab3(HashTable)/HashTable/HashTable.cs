using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HashTable
{
    public class HashTable<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private static readonly IReadOnlyList<int> _primes =
            new int[]
            { 11, 23, 47, 97, 197, 397, 797, 1597, 3203, 6421, 12853, 25717, 51437, 102877, 205759,
             411527, 823117, 1646237, 3292489, 6584983, 13169977, 26339969, 52679969, 105359939, 210719881,
             421439783, 842879579, 1685759167, 1947484393, 2147483549, 2147483629, 2147483647 };

        public TValue this[TKey key]
        {
            get
            {
                var position = FindEntry(key);
                if (position < 0) throw new ArgumentException();
                return _entries[position].Value;
            }

            set
            {
                var position = FindEntry(key);
                if (position < 0) throw new ArgumentException();
                _entries[position].Value = value;
            }
        }

        public ICollection<TKey> Keys => new List<TKey>(from t in this select t.Key);

        public ICollection<TValue> Values => new List<TValue>(from t in this select t.Value);

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        private class Entry
        {
            public int HashCode { get; private set; }
            public TKey Key { get; private set; }
            public TValue Value { get; set; }

            public Entry(TKey key, TValue value, int hashCode)
            {
                this.Key = key;
                this.Value = value;
                this.HashCode = hashCode;
            }
        }

        private readonly Entry _deleted = new Entry(default(TKey), default(TValue), 0);
        private Entry[] _entries;
        private const double c1 = 0;
        private const double c2 = 1;
        private const double _maxLoadFactor = 0.6;
        private int _primeIndex;

        public HashTable()
        {
            _primeIndex = 0;
            _entries = new Entry[_primes[_primeIndex]];
        }

        private void Resize()
        {
            var oldArray = _entries;
            _entries = new Entry[_primes[++_primeIndex]];

            int count = 0;
            for (int i = 0; count < Count && i < oldArray.Length; i++)
            {
                var entry = oldArray[i];
                if (entry != null && entry != _deleted)
                {
                    Put(entry);
                    count++;
                }
            }

        }

        private void Put(Entry entry)
        {
            int start = entry.HashCode;
            int i;
            for (i = 0; i < _entries.Length; i++)
            {
                var position = (int)(start + c1 * i + c2 * i * i) % _entries.Length;
                if (_entries[position] == null)
                {
                    _entries[position] = entry;
                    return;
                }
                else if (_entries[position] == _deleted)
                {
                    _entries[position] = entry;
                    break;
                }
                else if (_entries[position].Key.Equals(entry.Key))
                {
                    throw new ArgumentException();
                }
            }

            for (i++; i < _entries.Length; i++)
            {
                var position = (int)(start + c1 * i + c2 * i * i) % _entries.Length;
                var element = _entries[position];
                if (element == null) return;
                else if (element != _deleted && element.Key.Equals(entry.Key))
                {
                    throw new ArgumentException();
                }
            }

        }

        private void Insert(Entry entry)
        {
            Put(entry);
            Count++;
            if (Count >= _entries.Length * _maxLoadFactor)
            {
                Resize();
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentException();
            var entry = new Entry(key, value, key.GetHashCode() & 0x7fffffff);
            Insert(entry);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Array.Clear(_entries, 0, _entries.Length);
            Count = 0;
        }

        private int FindEntry(TKey key)
        {
            if (key == null) throw new ArgumentException();
            int start = key.GetHashCode() & 0x7fffffff;

            for (int i = 0; i < _entries.Length; i++)
            {
                int position = (int)(start + c1 * i + c2 * i * i) % _entries.Length;
                if (_entries[position] == null) return -1;
                else if (_entries[position] != _deleted && _entries[position].Key.Equals(key))
                {
                    return position;
                }
            }

            return -1;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var position = FindEntry(item.Key);
            if (position < 0) return false;
            if (_entries[position].Value is IComparable<TValue> compValue)
            {
                return compValue.CompareTo(_entries[position].Value) == 0;
            }
            else return item.Value.Equals(_entries[position].Value);
        }

        public bool ContainsKey(TKey key)
        {
            return FindEntry(key) >= 0;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array is null.");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex is less than 0.");
            }
            if (array.Length < arrayIndex + this.Count)
            {
                throw new ArgumentException("The number of elements in the source " +
                        "HashTable<TKey,TValue> " +
                        "is greater than the available space from index to the end of the " +
                        "destination array.");
            }
            int i = arrayIndex;

            foreach (var elem in this)
            {
                array[i] = elem;
                i++;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            int i = 0;

            foreach (var entry in _entries)
            {
                if (entry != null && entry != _deleted)
                {
                    yield return new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
                    i++;
                    if (i == Count) break;
                }
            }
        }

        public bool Remove(TKey key)
        {
            var position = FindEntry(key);
            if (position < 0) return false;
            else
            {
                _entries[position] = _deleted;
                Count--;
                return true;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var position = FindEntry(item.Key);
            if (position < 0) return false;

            bool founded = false;
            if (_entries[position].Value is IComparable<TValue> compValue)
            {
                founded = compValue.CompareTo(_entries[position].Value) == 0;
            }
            else founded = item.Value.Equals(_entries[position].Value);
            if (founded)
            {
                _entries[position] = _deleted;
                Count--;
            }
            return founded;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var position = FindEntry(key);
            if (position < 0)
            {
                value = default(TValue);
                return false;
            }
            else
            {
                value = _entries[position].Value;
                return true;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}