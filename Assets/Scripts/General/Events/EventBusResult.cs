using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;

public class EventBusResult<TEvent, TResult> : IEventBusResult<TEvent, TResult>
{
    private Dictionary<string, List<Func<TEvent, TResult>>> _events = new Dictionary<string, List<Func<TEvent, TResult>>>();

    public IDisposable Subscribe(string eventName, Func<TEvent, TResult> handler)
    {
        if (!_events.ContainsKey(eventName))
        {
            _events[eventName] = new List<Func<TEvent, TResult>>();
        }

        _events[eventName].Add(handler);

        return Disposable.Create( () => _events[eventName].Remove(handler));
    }

    public void Publish(string eventName, TEvent eventData, Action<TResult> callback)
    {
        if (_events.TryGetValue(eventName, out var handlers))
        {
            foreach (var handler in handlers)
            {
                var result = handler(eventData);
                callback?.Invoke(result);
            }
        }
        else
        {
            callback?.Invoke(default);
        }
    }
    public Task<TResult> Publish(string eventName, TEvent eventData)
    {
        var tcs = new TaskCompletionSource<TResult>();

        if (_events.TryGetValue(eventName, out var handlers))
        {
            foreach (var handler in handlers)
            {
                TResult result = handler(eventData);
                if (result != null)
                {
                    tcs.SetResult(result);
                    return tcs.Task;
                }
            }
        }

        tcs.SetResult(default);
        return tcs.Task;
    }
}
