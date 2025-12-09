using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

public class MoveTextStart : MonoBehaviour
{

    private IEventBusNotResult<Unit> _eventsBusU;

    [Inject]
    private void Contanier(IEventBusNotResult<Unit> eventsBusU, IEventBusNotResult<float> eventsBusF)
    {
        _eventsBusU = eventsBusU;
    }

    public void StartAnim()
    {
        gameObject.SetActive(true);

        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-1533, 715);

        DOTween.Sequence()
            .Append(transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 2f));
    }

    public void EndAnim()
    {
        DOTween.Sequence()
            .AppendInterval(2f)
            .Append(transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector2(1533, -715), 2f))
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                _eventsBusU.Publish(EventsName.EnebaleMove, Unit.Default);
            });
    }
}
