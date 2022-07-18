using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace ZodiarkLib.UI.Animations
{
    [CreateAssetMenu(fileName = "FadeNScaleAnimation", menuName = "Systems/UI/Animation/Fade n Scale",order = 10)]
    public class FadeNScaleAnimation : BaseTweenAnimation
    {
        public override IEnumerator Show()
        {
            if(_dialog == null)
                yield break;
            if(_dialog.CanvasGroup == null)
                yield break;

            _waitForTween = true;
            _dialog.CanvasGroup.alpha = 0.25f;
            var rect = _dialog.MainContent != null ? _dialog.MainContent : _dialog.Rect;
            rect.localScale = Vector3.one * 0.25f;
            
            var seq = DOTween.Sequence();
            seq.Append(_dialog.CanvasGroup.DOFade(_showTween.valueTo, _showTween.duration)
                .SetEase(_showTween.ease)
                .SetDelay(_showTween.delay));
            seq.Join(rect.DOScale(Vector3.one * _showTween.valueTo, _showTween.duration)
                .SetEase(_showTween.ease)
                .SetDelay(_showTween.delay));

            seq.OnComplete(() =>
            {
                _waitForTween = false;
            });
            
            _showTween.tween = seq;
            yield return new WaitWhile(() => _waitForTween);
        }

        public override IEnumerator Hide()
        {
            if(_dialog == null)
                yield break;
            if(_dialog.CanvasGroup == null)
                yield break;

            _waitForTween = true;
            _dialog.CanvasGroup.alpha = 1;
            var rect = _dialog.MainContent != null ? _dialog.MainContent : _dialog.Rect;
            rect.localScale = Vector3.one;
            
            var seq = DOTween.Sequence();
            seq.Append(_dialog.CanvasGroup.DOFade(_hideTween.valueTo, _hideTween.duration)
                .SetEase(_hideTween.ease)
                .SetDelay(_hideTween.delay));
            seq.Join(rect.DOScale(Vector3.one * _hideTween.valueTo, _hideTween.duration)
                .SetEase(_hideTween.ease)
                .SetDelay(_hideTween.delay));
            seq.OnComplete(() =>
            {
                _waitForTween = false;
            });

            _hideTween.tween = seq;
            yield return new WaitWhile(() => _waitForTween);
        }
    }
}