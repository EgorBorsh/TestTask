using UniRx;
using UnityEngine;
using Zenject;

public class SearcgChest : MonoBehaviour
{
    private Rigidbody _rb;

    private IEventBusNotResult<Unit> _eventsBusU;
    private IEventBusNotResult<float> _eventsBusF;
    private IEventBusNotResult<bool> _eventsBusB;
    private CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU, IEventBusNotResult<float> eventsBusF, IEventBusNotResult<bool> eventsBusB)
    {
        _eventsBusU = eventsBusU;
        _eventsBusF = eventsBusF;
        _eventsBusB = eventsBusB;

        _rb = GetComponent<Rigidbody>();

        _eventsBusU.Subscribe(EventsName.CharacterMoveForward, Observer.Create<Unit>(CheckEnemy)).AddTo(_disposables);
        _eventsBusF.Subscribe(EventsName.CharacterMoveRightOrLeft, Observer.Create<float>(CheckEnemy)).AddTo(_disposables);
    }

    public void CheckEnemy(Unit unit)
    {
        Check();
    }

    public void CheckEnemy(float target)
    {
        Check(target);
    }

    private void Check(float target = 0)
    {
        Vector3 newRot = target >= 0
            ? target == 0 ? new Vector3(0, 0, 0) : new Vector3(0, 90, 0)
            : new Vector3(0, -90, 0);

        Vector3 dir = Quaternion.Euler(newRot) * Vector3.forward;

        int layerMask7 = (1 << 11);
        bool blocked = Physics.Raycast(_rb.position, dir, 2f, layerMask7);

        _eventsBusB.Publish(EventsName.ActivePanelChest, blocked);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
