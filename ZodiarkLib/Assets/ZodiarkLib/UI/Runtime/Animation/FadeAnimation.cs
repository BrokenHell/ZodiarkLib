using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace ZodiarkLib.UI.Animations
{
    [CreateAssetMenu(fileName = "FadeAnimation", menuName = "Systems/UI/Animation/Fade",order = 10)]
    public class FadeAnimation : BaseTweenAnimation
    {
        public override IEnumerator Show()
        {
            if(_dialog == null)
                yield break;
            if(_dialog.CanvasGroup == null)
                yield break;

            _waitForTween = true;
            _dialog.CanvasGroup.alpha = 0;
            _showTween.tween = _dialog.CanvasGroup.DOFade(_showTween.valueTo, _showTween.duration)
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
            _dialog.CanvasGroup.alpha = 1;
            _hideTween.tween = _dialog.CanvasGroup.DOFade(_hideTween.valueTo, _hideTween.duration)
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