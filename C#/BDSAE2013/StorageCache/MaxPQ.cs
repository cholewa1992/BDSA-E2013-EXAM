using System;
using System.Collections;
using System.Collections.Generic;

namespace StorageCache
{
    internal class MaxPQ<TKey> : IEnumerable<TKey> where TKey : IComparable
    {
        private TKey[] _pq;
        private int _n;

        internal MaxPQ(int capacity)
        {
            _pq = new TKey[capacity + 1];
            _n = 0;
        }

        internal MaxPQ() : this(256)
        {
           
        }

        public bool IsEmpty()
        {
            return _n == 0;
        }

        public int Size()
        {
            return _n;
        }

        public TKey Max()
        {
            if (IsEmpty())
            {
                //TODO
            }
            return _pq[1];
        }

        private void Resize(int capacity)
        {
            if (capacity < _n)
            {
                //TODO EXCEPTION
            }
            var temp = new TKey[capacity];
            for (int i = 1; i <= _n; i++)
            {
                temp[i] = _pq[i];
            }
            _pq = temp;
        }

        public void Insert(TKey k)
        {
            if (_n >= _pq.Length - 1)
            {
                
                //Resize(2 * _pq.Length);
            }
            _pq[++_n] = k;
            Swim(_n);
            if (IsMaxHeap())
            {
                //TODO
            }
        }

        private void Swim(int k)
        {
            while (k > 1 && Less(k/2, k))
            {
                Exch(k, k/2);
                k = k/2;
            }
        }

        private bool Less(int i, int j)
        {
            return _pq[i].CompareTo(_pq[j]) < 0;
        }

        private void Exch(int i, int j)
        {
            var swap = _pq[i];
            _pq[i] = _pq[j];
            _pq[j] = swap;
        }

        private bool IsMaxHeap(int k = 1)
        {
            if (k > _n) return true;
            int left = 2*k, right = 2*k + 1;
            if (left <= _n && Less(k, left)) return false;
            if (right <= _n && Less(k, right)) return false;
            return IsMaxHeap(left) && IsMaxHeap(right);
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            return (IEnumerator<TKey>) _pq.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
