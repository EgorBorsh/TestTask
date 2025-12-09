using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class UILock : MonoBehaviour, IKeyOrLock, IDropHandler
{
    private int _indexColor;

    private int _countKey = 0;

    public int IndexColor { get => _indexColor; set => _indexColor = value; }

    private IEventBusNotResult<int> _eventsBusI;
    private IEventBusNotResult<bool> _eventsBusB;
    [Inject]
    private void Contanier(IEventBusNotResult<int> eventsBusI, IEventBusNotResult<bool> eventsBusB)
    {
        _eventsBusI = eventsBusI;
        _eventsBusB = eventsBusB;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag;
        Debug.Log(_indexColor + "Lock");

        if (dragged != null)
        {
            Debug.Log(dragged.GetComponent<IKeyOrLock>().IndexColor);
            if(dragged.GetComponent<IKeyOrLock>().IndexColor == _indexColor)
            {
                _countKey++;
                _eventsBusI.Publish(EventsName.UpdateTextKey, _countKey);
                Destroy(dragged.gameObject);

                _eventsBusB.Publish(EventsName.PlayerWin, _countKey >= 3);
                return;
            }

            dragged.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }
    }
}
