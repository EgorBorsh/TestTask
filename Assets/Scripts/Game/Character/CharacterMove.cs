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
    private bool _isMoving = false;

    private IEventBusNotResult<Unit> _eventsBusU;
    private IEventBusNotResult<float> _eventsBusF;
    private CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU, IEventBusNotResult<float> eventsBusF)
    {
        _eventsBusU = eventsBusU;
        _eventsBusF = eventsBusF;

        _rb=GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _eventsBusU.Subscribe(EventsName.CharacterMoveForward, Observer.Create<Unit>(MoveForward)).AddTo(_disposables);
        _eventsBusF.Subscribe(EventsName.CharacterMoveRightOrLeft, Observer.Create<float>(MoveRightOrLeft)).AddTo(_disposables);

    }

    public void MoveForward(Unit unit)
    {
        RotateAndMove();
    }

    public void MoveRightOrLeft(float target)
    {
        RotateAndMove(target);
    }

    private void RotateAndMove(float target = 0)
    {
        if (_isMoving) return;
        _isMoving = true;

        Vector3 newRot = target >= 0
            ? target == 0 ? new Vector3(0, 0, 0) : new Vector3(0, 90, 0)
            : new Vector3(0, -90, 0);

        Vector3 dir = Quaternion.Euler(newRot) * Vector3.forward;

        int layerMask = 1 << 7;
        bool blocked = Physics.Raycast(_rb.position, dir, _moveDistance, layerMask);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DORotate(newRot, 0.1f));

        if (!blocked)
        {
            Vector3 newPos = _rb.position + dir * _moveDistance;

            _animator.Play(_nameClipMove);
            seq.Append(_rb.DOMove(newPos, 0.25f).SetEase(Ease.Linear));
        }
        else
        {
            _isMoving = false;
        }

        seq.OnComplete(() =>
        {
            _animator.Play(_nameClipIdle);
            _isMoving = false;
        });
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
