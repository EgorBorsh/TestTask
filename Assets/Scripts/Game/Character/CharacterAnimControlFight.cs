using UniRx;
using UnityEngine;
using Zenject;

public class CharacterAnimControlFight : MonoBehaviour
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
