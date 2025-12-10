using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class CharacterDie : MonoBehaviour
{
    [SerializeField]
    private string _nameClipDie;

    private Rigidbody _rb;
    private Animator _animator;

    private IEventBusNotResult<Unit> _eventsBusU;
    private CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU)
    {
        _eventsBusU = eventsBusU;

        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _eventsBusU.Subscribe(EventsName.CharacterDie, Observer.Create<Unit>(StartAnimDie)).AddTo(_disposables);
    }

    private void StartAnimDie(Unit unit)
    {
        _rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;

        _animator?.Play(_nameClipDie);
    }

    public void EndAnimDie()
    {
        GetComponent<CharacterMove>().Dispose();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
