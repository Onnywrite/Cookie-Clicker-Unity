using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Onnywrite.Primitives
{
    public class ThreadSafeList<T> : IList<T>
    {
        private readonly List<T> _base;

        private readonly SemaphoreSlim _semaphore;

        public ThreadSafeList() : this(1)
        {

        }

        public ThreadSafeList(int capacity)
        {
            _base = new(capacity);
            _semaphore = new(1, 1);
        }

        public T this[int index]
        {
            get => ElementAt(index);
            set
            {
                _semaphore.Wait();
                _base[index] = value;
                _semaphore.Release();
            }
        }

        public int Count
        {
            get
            {
                _semaphore.Wait();
                var count = _base.Count;
                _semaphore.Release();
                return count;
            }
        }

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(T item)
        {
            _semaphore.Wait();
            _base.Add(item);
            _semaphore.Release();
        }

        public void Clear()
        {
            _semaphore.Wait();
            _base.Clear();
            _semaphore.Release();
        }

        public bool Contains(T item)
        {
            _semaphore.Wait();
            var success = _base.Contains(item);
            _semaphore.Release();
            return success;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _semaphore.Wait();
            _base.CopyTo(array, arrayIndex);
            _semaphore.Release();
        }


        public int IndexOf(T item)
        {
            _semaphore.Wait();
            var index = _base.IndexOf(item);
            _semaphore.Release();
            return index;
        }

        public void Insert(int index, T item)
        {
            _semaphore.Wait();
            _base.Insert(index, item);
            _semaphore.Release();
        }

        public bool Remove(T item)
        {
            _semaphore.Wait();
            var success = _base.Remove(item);
            _semaphore.Release();
            return success;
        }

        public void RemoveAt(int index)
        {
            _semaphore.Wait();
            _base.RemoveAt(index);
            _semaphore.Release();
        }


        public T ElementAt(int index)
        {
            _semaphore.Wait();
            var el = _base[index];
            _semaphore.Release();
            return el;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator() => new ThreadSafeEnumerator(this);

        public struct ThreadSafeEnumerator : IEnumerator<T>
        {
            public T Current => _current;

            object IEnumerator.Current => Current;

            private readonly IList<T> _this;
            private int _index;
            private T _current;

            internal ThreadSafeEnumerator(IList<T> @this)
            {
                _this = @this;
                _index = 0;
                _current = default;
            }

            public void Dispose()
            {

            }

            public bool MoveNext()
            {
                if (_this.Count == 0 || _index >= _this.Count) return false;
                _current = _this[_index];
                bool isValid = _index < _this.Count;
                _index++;
                return isValid;
            }

            public void Reset()
            {
                _index = 0;
            }
        }
    }
}