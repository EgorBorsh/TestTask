using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

public class CharacterColFinish : MonoBehaviour
{
    [SerializeField]
    private string _nameClipMove;

    private Rigidbody _rb;
    private Animator _animator;

    private IEventBusNotResult<Unit> _eventsBusU;

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU, IEventBusNotResult<float> eventsBusF)
    {
        _eventsBusU = eventsBusU;

        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Finish>())
        {
            _eventsBusU.Publish(EventsName.DisableMove, Unit.Default);
            _eventsBusU.Publish(EventsName.CharacterKillMove, Unit.Default);

            Vector3 newRot = new Vector3(0, 0, 0);

            Vector3 dir = Quaternion.Euler(newRot) * Vector3.forward;

            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DORotate(newRot, 0.1f));

            Vector3 newPos = _rb.position + dir * 3.5f;

            _animator?.Play(_nameClipMove);
            seq.Append(_rb?.DOMove(newPos, 2f)
                .SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    _eventsBusU.Publish(EventsName.LoadFight, Unit.Default);
                });
            
        }
    }
}
