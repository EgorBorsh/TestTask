using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class TextCountKey : MonoBehaviour
{
    private TMP_Text _text;

    private IEventBusNotResult<int> _eventsBusI;
    private CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    private void Contanier(IEventBusNotResult<int> eventsBusI)
    {
        _eventsBusI = eventsBusI;

        _text = GetComponent<TMP_Text>();
        _text.text = $"0/3";

        _eventsBusI.Subscribe(EventsName.UpdateTextKey, Observer.Create<int>(UpdateText)).AddTo(_disposables);
    }

    private void UpdateText(int count)
    {
        _text.text = $"{count}/3";
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
