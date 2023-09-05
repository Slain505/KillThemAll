using System.Collections.Generic;
using UnityEngine;

namespace Code.Shared
{
    public class ObjectPool : IManager
    {
        private GameObject objectPrefab;
        private List<GameObject> pooledObjects = new List<GameObject>();
        private Transform parent;

        /// <summary>
        /// Initialize a new instance of the ObjectPool class.
        /// </summary>
        public ObjectPool(GameObject objectPrefab, int initialSize, Transform parent = null)
        {
            this.objectPrefab = objectPrefab;
            this.parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = CreateObject();
                obj.SetActive(false);
            }
        }
    
        /// <summary>
        /// Returns an object
        /// If there is no objects - create new one
        /// Objects are returned to the pool by setting their active state to false.
        /// </summary>
        /// <returns></returns>
        public GameObject GetObject()
        {
            GameObject obj = null;

            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeSelf)
                {
                    obj = pooledObjects[i];
                    break;
                }
            }

            if (obj == null)
            {
                obj = CreateObject();
            }

            obj.SetActive(true);

            return obj;
        }
    
        /// <summary>
        /// Return object of some type
        /// Objects are returned to the pool by setting their active state to false.
        /// </summary>
        public T GetObject<T>() where T : Component
        {
            return GetObject().GetComponent<T>();
        }
    
        /// <summary>
        /// Sets all instantiated GameObjects to de-active
        /// </summary>
        public void ReturnAllObjectsToPool()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                ReturnObjectToPool(pooledObjects[i]);
            }
        }
    
        /// <summary>
        /// Return the object to pool
        /// </summary>
        public void ReturnObjectToPool(GameObject obj)
        {
            if (obj)
            {
                obj.SetActive(false);
                obj.transform.SetParent(parent, false);
            }
        }
    
        /// <summary>
        /// Destroy all objects in pool
        /// </summary>
        public void DestroyAllObjects()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                GameObject.Destroy(pooledObjects[i]);
            }
        
            pooledObjects.Clear();
        }
    
        private GameObject CreateObject()
        {
            GameObject obj = GameObject.Instantiate(objectPrefab);
            obj.transform.SetParent(parent, false);
            pooledObjects.Add(obj);

            return obj;
        }
    }
}    