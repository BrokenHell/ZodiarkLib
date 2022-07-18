using System;
using DG.Tweening;
using UnityEngine;

namespace ZodiarkLib.UI.Animations
{
    public abstract class BaseTweenAnimation : BaseDialogAnimation
    {
        #region Inner Classes

        [Serializable]
        public class TweenInfo
        {
            public Tween tween;
            public float valueTo;
            public float duration;
            public float delay;
            public Ease ease;
        }

        #endregion
        
        #region Fields

        [SerializeField]
        protected TweenInfo _showTween = new TweenInfo();
        [SerializeField]
        protected TweenInfo _hideTween = new TweenInfo();
        
        protected bool _waitForTween;

        #endregion

        #region Public Methods

        public override void StopImmediately()
        {
            _showTween?.tween?.Kill();
            _hideTween?.tween?.Kill();

            _waitForTween = false;
        }

        #endregion

        
    }   
}