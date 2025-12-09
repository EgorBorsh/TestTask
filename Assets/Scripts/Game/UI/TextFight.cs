using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextFight : MonoBehaviour
{
    private TMP_Text _text;

    private Tween _blinkTween;

    private void OnEnable()
    {
        if (_text == null)
        {
            _text = GetComponent<TMP_Text>();
            Fade();
        }
        else Fade();
    }

    private void Fade()
    {
        _text.alpha = 1f;
        _blinkTween = _text.DOFade(0f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }

    private void OnDisable()
    {
        _blinkTween?.Kill(); 
        _text.alpha = 1f;
    }
}
