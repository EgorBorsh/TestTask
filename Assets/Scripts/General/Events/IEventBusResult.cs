using System;
using System.Threading.Tasks;

public interface IEventBusResult<TEvent, TResult>
{
    public IDisposable Subscribe(string eventName, Func<TEvent, TResult> handler);

    public void Publish(string eventName, TEvent eventData, Action<TResult> callback);
    public Task<TResult> Publish(string eventName, TEvent eventData);
}
