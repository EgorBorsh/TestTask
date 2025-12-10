using UniRx;
using UnityEngine;
using Zenject;

public class EnemyDestroy : MonoBehaviour
{
    [SerializeField]
    private string _nameClipDie;

    private Animator _animator;
    private bool _isDie = false;

    private IEventBusNotResult<Unit> _eventsBusU;
    private CompositeDisposable _disposables = new CompositeDisposable();

    public bool IsDie { get => _isDie; }

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU)
    {
        _eventsBusU = eventsBusU;
        _animator = GetComponent<Animator>();

        _eventsBusU.Subscribe(EventsName.EnemyLose, Observer.Create<Unit>(Lose)).AddTo(_disposables);
    }

    private void Lose(Unit unit)
    {
        int layerMask7 = (1 << 3);
        bool blocked = Physics.Raycast(transform.position, transform.forward, 2f, layerMask7);

        if (blocked)
        {
            _isDie = true;
            GetComponent<Collider>().enabled = false;
            _animator?.Play(_nameClipDie);
        }
    }

    public void onEndAnim()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
