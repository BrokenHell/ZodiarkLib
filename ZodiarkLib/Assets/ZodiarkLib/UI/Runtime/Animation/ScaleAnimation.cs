using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace ZodiarkLib.UI.Animations
{
    [CreateAssetMenu(fileName = "ScaleAnimation", menuName = "Systems/UI/Animation/Scale",order = 10)]
    public class ScaleAnimation : BaseTweenAnimation
    {
        public override IEnumerator Show()
        {
            if(_dialog == null)
                yield break;
            if(_dialog.CanvasGroup == null)
                yield break;

            _waitForTween = true;
            var rect = _dialog.MainContent != null ? _dialog.MainContent : _dialog.Rect;
            rect.localScale = Vector3.zero;
            _showTween.tween = rect.DOScale(Vector3.one * _showTween.valueTo, _showTween.duration)
                .SetEase(_showTween.ease)
                .SetDelay(_showTween.delay)
                .OnComplete(() =>
                {
                    _waitForTween = false;
                });
            yield return new WaitWhile(() => _waitForTween);
        }

        public override IEnumerator Hide()
        {
            if(_dialog == null)
                yield break;
            if(_dialog.CanvasGroup == null)
                yield break;

            _waitForTween = true;
            var rect = _dialog.MainContent != null ? _dialog.MainContent : _dialog.Rect;
            rect.localScale = Vector3.one;
            _hideTween.tween = rect.DOScale(Vector3.one * _hideTween.valueTo, _hideTween.duration)
                .SetEase(_hideTween.ease)
                .SetDelay(_hideTween.delay)
                .OnComplete(() =>
                {
                    _waitForTween = false;
                });
            yield return new WaitWhile(() => _waitForTween);
        }
    }   
}