using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private float _moveDistance = 2f;
    [Space(5)]
    [Header("Animation Clip")]
    [SerializeField]
    private string _nameClipIdle;
    [SerializeField]
    private string _nameClipMove;

    private Rigidbody _rb;
    private Animator _animator;

    private IEventBusNotResult<Unit> _eventsBusU;
    private CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusF)
    {
        _eventsBusU = eventsBusF;
        _rb=GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _eventsBusU.Subscribe(EventsName.CharacterMoveForward, Observer.Create<Unit>(MoveForward)).AddTo(_disposables);

    }

    public void MoveForward(Unit unit)
    {
        Vector3 newPos = _rb.position + transform.forward * _moveDistance;
        _animator.Play(_nameClipMove);

        DOTween.Sequence()
            .Append(_rb.DOMove(newPos, 0.5f).SetEase(Ease.Linear))
            .OnComplete(() => 
        {
            _animator.Play(_nameClipIdle);
        });
        
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
