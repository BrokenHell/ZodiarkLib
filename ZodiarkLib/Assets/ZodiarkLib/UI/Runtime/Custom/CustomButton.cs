using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ZodiarkLib.UI.Custom
{
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
        IPointerExitHandler
    {
        #region Subtypes

        /// <summary>
        /// State of current button
        /// </summary>
        [System.Serializable]
        public enum State
        {
            Normal,
            Pressed,
            Disabled,
        }

        /// <summary>
        /// Custom event when button is click
        /// </summary>
        [System.Serializable]
        public class CustomButtonClickEvent : UnityEvent { }

        /// <summary>
        /// Custom event when button state is changed
        /// </summary>
        [System.Serializable]
        public class CustomButtonStateChangeEvent : UnityEvent<State> { }

        #endregion

        #region Fields

        [Header("Delay Click")]
        [Tooltip("Select to prevent multiple click through")]
        [SerializeField]
        protected bool _delayClickEvent = true;

        [Tooltip("Measure on how long to recover clickable state")]
        [SerializeField]
        protected float _delayTime = 0.15f;

        [Header("Interaction")]
        [SerializeField]
        protected bool _interactable = true;

        [SerializeField]
        [Tooltip("Select to send event after button click animation is finished.")]
        protected bool _sendClickEventAfterAnimation = false;

        /// <summary>
        /// Does control allow to click
        /// </summary>
        private bool _allowClickEvent = true;

        /// <summary>
        /// Does control is current disabled
        /// </summary>
        private bool _cancelled = false;

        /// <summary>
        /// Does control had been released
        /// </summary>
        private bool _isRelease = false;

        #endregion

        #region Properties

        /// <summary>
        /// Return true if control can be interact
        /// </summary>
        public bool Interactable
        {
            get
            {
                return _interactable;
            }
            set
            {
                _interactable = value;
                StateChangeEvent?.Invoke(_interactable ? State.Normal : State.Disabled);
            }
        }

        #endregion

        #region Events & Delegates

        /// <summary>
        /// Event fires when button process click event
        /// </summary>
        public CustomButtonClickEvent ClickEvent;

        /// <summary>
        /// Event fires when button state is changed
        /// </summary>
        [HideInInspector]
        public CustomButtonStateChangeEvent StateChangeEvent;

        /// <summary>
        /// Event fires when button being pressed down
        /// </summary>
        [HideInInspector]
        public UnityEvent OnButtonDownEvent;
        
        /// <summary>
        /// Event fires when button being release up
        /// </summary>
        [HideInInspector]
        public UnityEvent OnButtonUpEvent;

        /// <summary>
        /// Event fires when cursor exit
        /// </summary>
        [HideInInspector]
        public UnityEvent OnButtonExitEvent;

        /// <summary>
        /// Event fires when button being clicked
        /// </summary>
        [HideInInspector]
        public UnityEvent OnButtonClickEvent;

        #endregion

        #region Unity Events

        /// <summary>
        /// Unity OnEnable event
        /// </summary>
        protected virtual void OnEnable()
        {
            _allowClickEvent = true;
            if (Interactable)
            {
                StateChangeEvent?.Invoke(State.Normal);
            }
            else
            {
                StateChangeEvent?.Invoke(State.Disabled);
            }
        }

        private void OnDisable()
        {
            _allowClickEvent = false;
            StopAllCoroutines();
        }

        /// <summary>
        /// Unity Destroy event
        /// </summary>
        protected virtual void OnDestroy()
        {
            _allowClickEvent = false;
            StopAllCoroutines();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Unity event callback when pointer is down
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!_interactable)
                return;
            if(!_allowClickEvent)
                return;

            _isRelease = false;
            _cancelled = false;
            OnButtonDown();
            OnButtonDownEvent?.Invoke();
        }

        /// <summary>
        /// Unity event callback when pointer is up
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            _isRelease = true;
            OnButtonUp();
            OnButtonUpEvent?.Invoke();
        }

        /// <summary>
        /// Unity event callback when pointer is click
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!_interactable)
                return;
            if (_cancelled)
                return;

            if (!_sendClickEventAfterAnimation && _allowClickEvent)
            {
                if (_delayClickEvent && this.gameObject.activeInHierarchy)
                {
                    StartCoroutine(DelayButtonClick());
                }
                ClickEvent?.Invoke();
            }

            if (_allowClickEvent)
            {
                OnButtonClick();
                OnButtonClickEvent?.Invoke();
            }
        }

        /// <summary>
        /// Unity event callback when pointer is exit
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (_isRelease)
                return;

            _cancelled = true;
            OnButtonExit();
            OnButtonExitEvent?.Invoke();
        }

        /// <summary>
        /// Manual trigger button click event
        /// </summary>
        public void TriggerPostAnimationClickEvent()
        {
            if (!_sendClickEventAfterAnimation || _cancelled || !_interactable) 
                return;
            
            ClickEvent?.Invoke();
            if (_delayClickEvent && this.gameObject.activeInHierarchy)
            {
                StartCoroutine(DelayButtonClick());
            }
        }

        #endregion

        #region Private Methods

        protected virtual void OnButtonDown() { }
        protected virtual void OnButtonUp() { }
        protected virtual void OnButtonClick() { }
        protected virtual void OnButtonExit() { }

        /// <summary>
        /// Delay button click event
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayButtonClick()
        {
            _allowClickEvent = false;
            yield return new WaitForSeconds(_delayTime);
            _allowClickEvent = true;
        }

        #endregion
    }   
}