using System;
using System.Collections;
using System.Collections.Generic;

namespace ConvenientCollections
{
    /// <summary>
    /// A sorted list which only sorts its contents on read, 
    /// supporting O(1) insertion and O(logn) search during regular use.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The value of the key.</typeparam>
    public class LazySortedList<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        private readonly List<KeyValuePair<TKey, TValue>> _list;

        private bool isSorted;

        /// <summary>
        /// Creates an instance of the <see cref="LazySortedList{TKey, TValue}"/> object.
        /// </summary>
        public LazySortedList(int capacity = 4)
        {
            this._list = new List<KeyValuePair<TKey, TValue>>(capacity);
        }

        /// <summary>
        /// Creates an instance of the <see cref="LazySortedList{TKey, TValue}"/> object.
        /// </summary>
        /// <param name="keyValues">The key-value pairs to insert.</param>
        public LazySortedList(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        {
            this._list = new List<KeyValuePair<TKey, TValue>>(keyValues);
        }

        /// <inheritdoc/>
        public TValue this[TKey key]
        {
            get
            {
                if (!this.TryGetValue(key, out TValue value))
                {
                    throw new KeyNotFoundException();
                }

                return value;
            }

            set
            {
                int index = this.FindItemIndex(key);
                if (index < 0)
                {
                    throw new KeyNotFoundException();
                }

                this._list[index] = new KeyValuePair<TKey, TValue>(key, value);
                this.isSorted = false;
            }
        }

        /// <inheritdoc/>
        public ICollection<TKey> Keys
        {
            get
            {
                List<TKey> keys = new List<TKey>(this.Count);
                for (int i = 0; i < this.Count; i++)
                {
                    keys.Add(this._list[i].Key);
                }

                return keys;
            }
        }

        /// <inheritdoc/>
        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> values = new List<TValue>(this.Count);
                for (int i = 0; i < this.Count; i++)
                {
                    values.Add(this._list[i].Value);
                }

                return values;
            }
        }

        /// <inheritdoc/>
        public int Count => this._list.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            this._list.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this._list.Add(item);
            this.isSorted = this.Count == 1 || 
                // optimize sorted insert
                KeyComparer.Instance.Compare(this._list[this.Count - 2], item) >= 0;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this._list.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            this.EnsureSorted();

            int index = this._list.BinarySearch(item, KeyComparer.Instance);
            if (index < 0)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
        {
            return this.Contains(new KeyValuePair<TKey, TValue>(key, default));
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            this.EnsureSorted();

            for (int i = 0; i < this._list.Count; i++)
            {
                array[arrayIndex + i] = this._list[i];
            }
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            this.EnsureSorted();
            return this._list.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            return this.Remove(new KeyValuePair<TKey, TValue>(key, default));
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            this.EnsureSorted();
            int index = this.FindItemIndex(item.Key);

            if (index < 0)
            {
                return false;
            }

            this._list.RemoveAt(index);

            return true;
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValue value)
        {
            this.EnsureSorted();

            int index = this.FindItemIndex(key);

            if (index < 0)
            {
                value = default;
                return false;
            }

            value = this._list[index].Value;

            return true;
        }

        private void EnsureSorted()
        {
            if (this.isSorted)
            {
                return;
            }

            this._list.Sort(KeyComparer.Instance);
            this.isSorted = true;
        }

        private int FindItemIndex(TKey key)
        {
            return this.BinarySearch(key);
        }

        private int FindItemIndex(KeyValuePair<TKey, TValue> item)
        {
            int index = this.BinarySearch(item.Key);
            return index;
        }

        private int BinarySearch(TKey key)
        {
            int low = 0;
            int high = this._list.Count - 1;

            while (low <= high)
            {
                int median = (low + high) >> 1;

                int compareResult = key.CompareTo(this._list[median].Key);
                if (compareResult == 0)
                {
                    return median;
                }
                else if (compareResult > 0)
                {
                    low = median + 1;
                    continue;
                }
                else
                {
                    high = median - 1;
                    continue;
                }
            }

            return ~low;
        }

        private class KeyComparer : IComparer<KeyValuePair<TKey, TValue>>
        {
            public static readonly KeyComparer Instance = new KeyComparer();

            public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            {
                return x.Key.CompareTo(y.Key);
            }
        }
    }
}
