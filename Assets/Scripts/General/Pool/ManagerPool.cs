using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPool<T> where T : MonoBehaviour, IObjectPoolable
{
    private static Dictionary<int, Pool<T>> pools = new Dictionary<int, Pool<T>>();

    public static Pool<T> PopulatePool(T prefabs, int amount, Vector3 position)
    {
        var obj = pools[(int)prefabs.Type].PopulatePool(prefabs, amount, position);
        return obj;
    }

    public static Pool<T> AddPool(T prefabs, bool reparent = true)
    {
        Pool<T> pool;

        if (!pools.TryGetValue((int)prefabs.Type, out pool))
        {
            pool = new Pool<T>();
            
            pools.Add((int)prefabs.Type, pool);

            if (reparent)
            {
                var poolsPO = GameObject.Find("[POOLS]") ?? new GameObject("[POOLS]");
                var poolPO = new GameObject("Pool:" + prefabs.Type);
                poolPO.transform.SetParent(poolsPO.transform);
                pool.SetParent(poolPO.transform);
            }
        }

        return pool;
    }

    public static T Spawn(T prefabs, Vector3 position = default(Vector3),
        Quaternion rotation = default(Quaternion), Transform parent = null)
    {
        return pools[(int)prefabs.Type].Spawn(prefabs, position, rotation, parent);
    }

    public static void Despawn(T objectPool)
    {
        pools[(int)objectPool.Type].Despawn(objectPool);
    }

    public static void Dispose()
    {
        foreach (var poolsValue in pools.Values)
            poolsValue.Dispose();
        
        pools.Clear();
    }
}
