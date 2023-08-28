using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Onnywrite.Primitives
{
    /// <summary>
    /// Thread-safe object pool class for <see cref="MonoBehaviour"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T> where T : MonoBehaviour
    {
        private readonly ConcurrentBag<T> _collection;
        private readonly Func<T> _create;
        private int _takenCount = 0;

        public Pool(Func<T> createFunc, int initialSize)
        {
            _create = createFunc;
            _collection = new();
            for (int i = 0; i < initialSize; i++)
            {
                var created = _create();
                created.gameObject.SetActive(false);
                _collection.Add(created);
            }
        }

        public Pool(Func<T> createFunc) : this(createFunc, 0)
        {
        }

        public int Taken => _takenCount;

        public T Take()
        {
            var item = _collection.TryTake(out var obj) ? obj : _create();
            item.gameObject.SetActive(true);
            _takenCount++;
            return item;
        }

        public T Take(Vector3 position, Quaternion rotation)
        {
            var item = Take();
            item.transform.localPosition = position;
            item.transform.localRotation = rotation;
            return item;
        }

        public T Take(Vector3 position) => Take(position, Quaternion.identity);

        public void Return(T item)
        {
            item.gameObject.SetActive(false);
            _collection.Add(item);
        }

        public void Clear() => ClearUpTo(0);

        public void ClearUpTo(int finalCount)
        {
            finalCount = Mathf.Clamp(finalCount, 0, int.MaxValue);
            while (_collection.Count != finalCount && !_collection.IsEmpty)
            {
                if (_collection.TryTake(out var item))
                {
                    GameObject.Destroy(item.gameObject);
                }
            }
        }
    }
}