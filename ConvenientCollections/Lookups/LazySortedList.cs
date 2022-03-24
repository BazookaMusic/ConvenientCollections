namespace BazookaMusic.ConvenientCollections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

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

        private readonly KeyValueComparer comparer;

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

        /// <summary>
        /// Creates an instance of the <see cref="LazySortedList{TKey, TValue}"/> object.
        /// </summary>
        /// <param name="comparer">The comparer to use.</param>
        public LazySortedList(int capacity, IComparer<TKey> comparer)
        {
            this._list = new List<KeyValuePair<TKey, TValue>>(capacity);
            this.comparer = comparer != null ? new KeyValueComparer(comparer) : KeyValueComparer.Instance;
        }

        /// <summary>
        /// Creates an instance of the <see cref="LazySortedList{TKey, TValue}"/> object.
        /// </summary>
        /// <param name="keyValues">The key-value pairs to insert.</param>
        /// <param name="comparer">The comparer to use.</param>
        public LazySortedList(IEnumerable<KeyValuePair<TKey, TValue>> keyValues, IComparer<TKey> comparer)
        {
            this._list = new List<KeyValuePair<TKey, TValue>>(keyValues);
            this.comparer = comparer != null ? new KeyValueComparer(comparer) : KeyValueComparer.Instance;
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
                this.EnsureSorted();
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
                this.EnsureSorted();
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
            this.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this._list.Add(item);
            this.isSorted = this.Count == 1 || 
                // optimize sorted insert
                KeyValueComparer.Instance.Compare(this._list[this.Count - 2], item) >= 0;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this._list.Clear();
            this.isSorted = true;
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            this.EnsureSorted();

            int index = this._list.BinarySearch(item, KeyValueComparer.Instance);
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

        /// <summary>
        /// Forces the list to sort its contents.
        /// </summary>
        public void EnsureSorted()
        {
            if (this.isSorted)
            {
                return;
            }

            this._list.Sort(KeyValueComparer.Instance);
            this.isSorted = true;
        }

        private int FindItemIndex(TKey key)
        {
            return this.BinarySearch(key);
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

        private class KeyValueComparer : IComparer<KeyValuePair<TKey, TValue>>
        {
            public static readonly KeyValueComparer Instance = new KeyValueComparer();

            private readonly IComparer<TKey> customComparer;

            private KeyValueComparer()
            {
            }

            public KeyValueComparer(IComparer<TKey> customKeyComparer)
            {
                this.customComparer = customKeyComparer;
            }

            public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            {
                if (this.customComparer == null)
                {
                    return x.Key.CompareTo(y.Key);
                }

                return this.customComparer.Compare(x.Key, y.Key);
            }
        }
    }
}
