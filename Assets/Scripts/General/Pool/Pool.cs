using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour, IObjectPoolable
{
    private Transform parentPool;
    private Dictionary<int, Stack<T>> cachedObjects = new Dictionary<int, Stack<T>>();
    private Dictionary<int, int> cachedIds = new Dictionary<int, int>();

    public Pool<T> PopulatePool(T prefabs, int amount, Vector3 position)
    {
        var key = prefabs.GetInstanceID();
        Stack<T> stack;
        var stacked = cachedObjects.TryGetValue(key, out stack);
        
        if(!stacked)
            cachedObjects.Add(key, new Stack<T>());

        for (int i = 0; i < amount; i++)
        {
            var go = CreationObject(prefabs, position, Quaternion.identity, parentPool);
            go.gameObject.SetActive(false);
            var idObject = go.GetInstanceID();
            cachedIds.Add(idObject, key);
            cachedObjects[key].Push(go);
        }
        
        return this;
    }

    public void SetParent(Transform parent)
    {
        parentPool = parent;
    }

    public T Spawn(T prefabs, Vector3 position = default(Vector3),
        Quaternion rotation = default(Quaternion), Transform parent = null)
    {
        var key = prefabs.GetInstanceID();
        Stack<T> stack;
        var stacked = cachedObjects.TryGetValue(key, out stack);

        if (stacked && stack.Count > 0)
        {
            var objectSpawn = stack.Pop();
            objectSpawn.gameObject.transform.SetParent(parent);
            objectSpawn.gameObject.transform.rotation = rotation;
            objectSpawn.gameObject.SetActive(true);
            objectSpawn.gameObject.transform.position = position;
            
            objectSpawn.Spawn();

            return objectSpawn;
        }
        
        if(!stacked) cachedObjects.Add(key, new Stack<T>());

        var createPrefbs = CreationObject(prefabs, position, rotation, parent);
        
        cachedIds.Add(createPrefbs.GetInstanceID(), key);
        
        createPrefbs.Spawn();

        return createPrefbs;
    }

    public void Despawn(T objectPool)
    {
        objectPool.gameObject.SetActive(false);
        
        cachedObjects[cachedIds[objectPool.GetInstanceID()]].Push(objectPool);
        objectPool.Despawn();
        
        if(parentPool != null) objectPool.transform.SetParent(parentPool);
    }

    public void Dispose()
    {
        parentPool = null;
        cachedObjects.Clear();
        cachedIds.Clear();
    }

    private T CreationObject(T prefabs, Vector3 position = default(Vector3),
        Quaternion rotation = default(Quaternion), Transform parent = null)
    {
        return Object.Instantiate(prefabs, position, rotation, parent);
    }
    
}
