using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTextStart : MonoBehaviour
{

    public void Init()
    {
        DOTween.Sequence()
            .Append(transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 2f))
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}
