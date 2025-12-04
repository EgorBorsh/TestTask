using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour, IObjectPoolable
{
    [SerializeField]
    private PoolType _typePool;

    private float _speedMove = 5f;

    Sequence seq;
    private Vector3 _moveDir = Vector3.zero;

    public PoolType Type { get => _typePool; }
    public float SpeedMove {set => _speedMove = value; }

    public void SetDirection(Vector3 dir)
    {
        _moveDir = dir.normalized;
    }

    public void Despawn()
    {
        seq?.Kill();
    }

    public void Spawn()
    {

        seq = DOTween.Sequence();
    }

    public void Move()
    {

        float distance = 50f;
        float duration = distance / _speedMove;

        seq.Append(transform.DOMove(transform.position + _moveDir * distance, duration).SetEase(Ease.Linear))
           .OnComplete(() =>
           {
               ManagerPool<Obstacle>.Despawn(this);
           });
    }
}
