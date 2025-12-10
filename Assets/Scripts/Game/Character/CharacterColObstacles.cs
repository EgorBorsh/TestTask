using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class CharacterColObstacles : MonoBehaviour
{

    private IEventBusNotResult<Unit> _eventsBusU;
    private CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU)
    {
        _eventsBusU = eventsBusU;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Obstacle>())
        {
            _eventsBusU.Publish(EventsName.DisableMove, Unit.Default);
            _eventsBusU.Publish(EventsName.CharacterKillMove, Unit.Default);
            _eventsBusU.Publish(EventsName.CharacterDie, Unit.Default);
        }
    }
}
