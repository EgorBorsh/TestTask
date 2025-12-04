using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPoolable
{
    public PoolType Type { get;}

    public void Spawn();
    public void Despawn();

}
