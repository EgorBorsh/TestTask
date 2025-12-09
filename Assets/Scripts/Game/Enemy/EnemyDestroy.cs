using UniRx;
using UnityEngine;
using Zenject;

public class EnemyDestroy : MonoBehaviour
{
    private IEventBusNotResult<Unit> _eventsBusU;
    private CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU)
    {
        _eventsBusU = eventsBusU;

        _eventsBusU.Subscribe(EventsName.EnemyLose, Observer.Create<Unit>(Lose)).AddTo(_disposables);
    }

    private void Lose(Unit unit)
    {
        int layerMask7 = (1 << 3);
        bool blocked = Physics.Raycast(transform.position, transform.forward, 2f, layerMask7);

        if (blocked)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
