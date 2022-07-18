using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace ZodiarkLib.UI.Custom
{
    [RequireComponent(typeof(CustomButton))]
    public class CustomButtonScale : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Transform _target;
        [SerializeField] private float _toValue = 0.95f;
        [SerializeField] private float _scaleDuration = 0.15f;

        private Vector3 _originalScale;
        private CustomButton _button;

        #endregion

        #region Unity Events

        private void Awake()
        {
            _button = GetComponent<CustomButton>();
            _button.OnButtonUpEvent.AddListener(OnButtonUp);
            _button.OnButtonDownEvent.AddListener(OnButtonDown);
            _button.OnButtonExitEvent.AddListener(OnButtonExit);

            if (_target != null)
            {
                _originalScale = _target.localScale;
            }
        }

        private void OnDestroy()
        {
            _button.OnButtonUpEvent.RemoveListener(OnButtonUp);
            _button.OnButtonDownEvent.RemoveListener(OnButtonDown);
            _button.OnButtonExitEvent.RemoveListener(OnButtonExit);
        }

        #endregion

        #region Private Methods
        
        private void OnButtonClick()
        {
            if (_target == null) 
                return;
            
            StartCoroutine(ResetCoroutine());
        }

        private void OnButtonDown()
        {
            if (_target == null) 
                return;

            _target.DOKill();
            _target.localScale = _originalScale;
            _target.DOScale(Vector3.one * _toValue, _scaleDuration);
        }

        private void OnButtonExit()
        {
            if (_target == null) 
                return;

            _target.DOKill();
            _target.DOScale(_originalScale , _scaleDuration);
        }

        private void OnButtonUp()
        {
            if (_target == null) 
                return;
            
            _target.DOKill();
            _target.DOScale(_originalScale , _scaleDuration);
        }

        private IEnumerator ResetCoroutine()
        {
            yield return new WaitForSeconds(_scaleDuration);
            _target.DOKill();
            _target.localScale = _originalScale;
            _button.TriggerPostAnimationClickEvent();
        }

        #endregion
    }
}