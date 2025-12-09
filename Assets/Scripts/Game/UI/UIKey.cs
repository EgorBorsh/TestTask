using UnityEngine;
using UnityEngine.EventSystems;

public class UIKey : MonoBehaviour, IKeyOrLock, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private int _indexColor;

    private RectTransform _transform;
    private CanvasGroup canvasGroup;

    public int IndexColor { get => _indexColor; set => _indexColor = value; }

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }
}
