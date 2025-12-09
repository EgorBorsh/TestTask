using UniRx;
using UnityEngine;
using Zenject;

public class EnemyAnimControlFight : MonoBehaviour
{
    [SerializeField]
    private string _nameClipIdle;
    [SerializeField]
    private string _nameClipFight;

    private Animator _animator;

    private IEventBusNotResult<Unit> _eventsBusU;
    private CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU)
    {
        _eventsBusU = eventsBusU;

        _animator = GetComponent<Animator>();

        _eventsBusU.Subscribe(EventsName.EnebaleMove, Observer.Create<Unit>(AnimIdle)).AddTo(_disposables);
        _eventsBusU.Subscribe(EventsName.EnabledFight, Observer.Create<Unit>(AnimFight)).AddTo(_disposables);
    }

    private void AnimFight(Unit unit)
    {
        int layerMask7 = (1 << 3);
        bool blocked = Physics.Raycast(transform.position, transform.forward, 2f, layerMask7);

        if(blocked)
            _animator?.Play(_nameClipFight);
    }

    private void AnimIdle(Unit unit)
    {
        _animator?.Play(_nameClipIdle);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
