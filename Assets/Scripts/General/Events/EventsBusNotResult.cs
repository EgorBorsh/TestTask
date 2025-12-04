using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;


public class EventsBusNotResult<T> : IEventBusNotResult<T>
{
    private Dictionary<string, Subject<T>> _events = new Dictionary<string, Subject<T>>();

    public IDisposable Subscribe(string eventName, IObserver<T> observer)
    {
        if (!_events.ContainsKey(eventName))
        {
            _events[eventName] = new Subject<T>();
        }

        return _events[eventName].Subscribe(observer);
    }

    public void Publish(string eventName, T eventData)
    {
        if (_events.ContainsKey(eventName))
        {
            _events[eventName].OnNext(eventData);
        }
    }
}
