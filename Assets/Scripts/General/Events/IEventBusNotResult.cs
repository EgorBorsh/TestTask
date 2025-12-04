using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventBusNotResult<T> 
{
    public IDisposable Subscribe(string eventName, IObserver<T> observer);

    public void Publish(string eventName, T eventData);
}
