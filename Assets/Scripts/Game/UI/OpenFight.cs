using UniRx;
using UnityEngine;
using Zenject;

public class OpenFight : MonoBehaviour
{
    [SerializeField] private GameObject _panelFight;
    private IEventBusNotResult<bool> _eventsBusB;
    private CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    private void Contanier(IEventBusNotResult<bool> eventsBusB)
    {
        _eventsBusB = eventsBusB;

        _eventsBusB.Subscribe(EventsName.ActivePanelFight, Observer.Create<bool>(OpenPanel)).AddTo(_disposables);
    }

    private void OpenPanel(bool isOpen)
    {
        _panelFight.SetActive(isOpen);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
