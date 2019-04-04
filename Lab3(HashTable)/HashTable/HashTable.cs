using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HashTable
{
    public class HashTable<TKey, TValue> : IDictionary<TKey, TValue>
    {
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
        private const double c1 = 0.5;
        private const double c2 = 0.5;
        //const double maxLoadFactor = 0.7;
        private int _maxSteps;

        private int CreateNewSize(int oldSize)
        {
            return oldSize * 2;            
        }

        public HashTable()
        {
            _entries = new Entry[16];
            _maxSteps = 4;
        }

        private void Resize()
        {
            var oldArray = _entries;
            _entries = new Entry[CreateNewSize(_entries.Length)];
            _maxSteps++;

            int i = 0, count = 0;
            while (i < oldArray.Length && count < Count)
            {
                var entry = oldArray[i];
                i++;
                if (entry != null && entry != _deleted)
                {
                    if (!TryToPut(entry))
                    {
                        _entries = new Entry[CreateNewSize(_entries.Length)];
                        _maxSteps++;
                        i = 0;
                        count = 0;
                    }
                    else
                    {                       
                        count++;
                    }
                }                
            }

        }

        private bool TryToPut(Entry entry)
        {
            int start = entry.HashCode;

            for (int i = 0; i < _maxSteps; i++)
            {
                int position = (int)(start + c1 * i + c2 * i * i) % _entries.Length;
                if (_entries[position] == null)
                {
                    _entries[position] = entry;
                    return true;
                }
                else if (_entries[position] != _deleted)
                {
                    if (_entries[position].Key is IComparable<TKey> compKey)
                    {
                        if (compKey.CompareTo(entry.Key) == 0)
                        {
                            throw new ArgumentException();
                        }
                    }
                    else if (_entries[position].Key.Equals(entry.Key))
                    {
                        throw new ArgumentException();
                    }
                }
            }

            return false;
        }

        private void Insert(Entry entry)
        {
            while (!TryToPut(entry))
            {
                Resize();
            }
            Count++;
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
            Array.Clear(_entries, 0, Count);
            Count = 0;            
        }

        private int FindEntry(TKey key)
        {
            if (key == null) throw new ArgumentException();
            int start = key.GetHashCode() & 0x7fffffff;

            for (int i = 0; i < _maxSteps; i++)
            {
                int position = (int)(start + c1 * i + c2 * i * i) % _entries.Length;
                if (_entries[position] != null && _entries[position] != _deleted)
                {
                    if (_entries[position].Key is IComparable<TKey> compKey)
                    {
                        if (compKey.CompareTo(key) == 0)
                        {
                            return position;
                        }
                    }
                    else if (_entries[position].Key.Equals(key))
                    {
                        return position;
                    }
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