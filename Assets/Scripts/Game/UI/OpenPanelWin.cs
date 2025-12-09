using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class OpenPanelWin : MonoBehaviour
{
    [SerializeField] private GameObject _panelChest;
    private IEventBusNotResult<bool> _eventsBusB;
    private CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    private void Contanier(IEventBusNotResult<bool> eventsBusB)
    {
        _eventsBusB = eventsBusB;

        _eventsBusB.Subscribe(EventsName.PlayerWin, Observer.Create<bool>(OpenPanel)).AddTo(_disposables);
    }

    private void OpenPanel(bool isOpen)
    {
        _panelChest.SetActive(isOpen);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
