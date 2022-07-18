using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ZodiarkLib.UI.Custom
{
    [RequireComponent(typeof(CustomButton))]
    public class CustomButtonTint : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Image _targetGraphic;
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _pressedColor = new Color32(200, 200, 200, 255);
        [SerializeField] private Color _disabledColor = new Color32(200, 200, 200, 128);
        [Range(1, 5)]
        [SerializeField] private float _colorMultiplier = 1;
        [SerializeField] private float _fadeDuration = 0.1f;

        private CustomButton _button;

        #endregion

        #region Unity Events

        private void Awake()
        {
            _button = GetComponent<CustomButton>();
            _button.OnButtonUpEvent.AddListener(OnButtonUp);
            _button.OnButtonDownEvent.AddListener(OnButtonDown);
            _button.OnButtonExitEvent.AddListener(OnButtonExit);
            _button.OnButtonClickEvent.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _button.OnButtonUpEvent.RemoveListener(OnButtonUp);
            _button.OnButtonDownEvent.RemoveListener(OnButtonDown);
            _button.OnButtonExitEvent.RemoveListener(OnButtonExit);
            _button.OnButtonClickEvent.RemoveListener(OnButtonClick);
        }

        private void OnEnable()
        {
            if (_targetGraphic != null)
            {
                _targetGraphic.CrossFadeColor(_button.Interactable ? _normalColor : _disabledColor * _colorMultiplier,
                    _fadeDuration, false, true);
            }
        }

        #endregion

        #region Private Methods

        private void OnButtonClick()
        {
            if (_targetGraphic == null) 
                return;
            
            StartCoroutine(ResetColorRoutine());
        }

        private void OnButtonDown()
        {
            if (_targetGraphic == null) 
                return;
            
            _targetGraphic.CrossFadeColor(_pressedColor * _colorMultiplier,
                _fadeDuration, false, true);
        }

        private void OnButtonExit()
        {
            if (_targetGraphic == null) 
                return;
            
            _targetGraphic.CrossFadeColor(_button.Interactable ? _normalColor : _disabledColor * _colorMultiplier,
                _fadeDuration, false, true);
        }

        private void OnButtonUp()
        {
            if (_targetGraphic == null) 
                return;
            
            _targetGraphic.CrossFadeColor(_button.Interactable ? _normalColor : _disabledColor * _colorMultiplier,
                _fadeDuration, false, true);
        }

        private IEnumerator ResetColorRoutine()
        {
            yield return new WaitForSeconds(_fadeDuration);
            _targetGraphic.CrossFadeColor(_button.Interactable ? _normalColor : _disabledColor * _colorMultiplier,
                _fadeDuration, false, true);
            _button.TriggerPostAnimationClickEvent();
        }

        #endregion
    }   
}