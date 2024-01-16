using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Pools
{
    public class ObjectPool<T> where T : Component
    {
        private readonly Queue<T> objectPool;
        private readonly T prefab;
        private readonly Transform parentTransform;
        private readonly Action<T> _initPool;
        private const int RefillCount = 15;
        private int countObject;

        public ObjectPool(T prefab, Transform parentTransform, int initialSize = 10, Action<T> initPool = null)
        {
            countObject = 0;
            this.prefab = prefab;
            this.parentTransform = parentTransform;
            _initPool = initPool;

            objectPool = new Queue<T>(initialSize);
            FillPool(initialSize);
        }

        private void FillPool(int fillCount)
        {
            for (var i = 0; i < fillCount; i++)
            {
                countObject++;
                var obj = Object.Instantiate(prefab, parentTransform);
                obj.name = prefab.name + countObject;
                _initPool?.Invoke(obj);

                ReturnObject(obj);
            }
        }

        public T GetObject(Vector3 position, Quaternion rotation, Transform parentObject = null)
        {
            if (objectPool.Count <= 0) FillPool(RefillCount);

            var obj = objectPool.Dequeue();
            obj.gameObject.transform.parent = parentObject;
            var newGameObjectTransform = obj.transform;
            newGameObjectTransform.position = position;
            newGameObjectTransform.rotation = rotation;
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReturnObject(T obj)
        {
            obj.gameObject.transform.parent = parentTransform;
            obj.gameObject.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }
}