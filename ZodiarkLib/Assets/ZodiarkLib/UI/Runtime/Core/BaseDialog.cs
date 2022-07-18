using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ZodiarkLib.UI
{
    /// <summary>
    /// Base class for all dialog need to be implemented
    /// </summary>
    [RequireComponent(typeof(CanvasGroup),
        typeof(GraphicRaycaster),
        typeof(Canvas))]
    [ExecuteInEditMode]
    public abstract class BaseDialog : MonoBehaviour
    {
        #region Struct

        [Serializable]
        public struct DialogHideArgs
        {
            public BaseDialog dialog;
            public object args;
        }

        #endregion
        
        #region Fields

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GraphicRaycaster _graphicRayCaster;
        [SerializeField] private RectTransform _rectPanel;
        [SerializeField] private RectTransform _mainContent;
        [SerializeField] private Image _blackMask;
        [SerializeField] private Camera _cam3D;

        /// <summary>
        /// Current show animation
        /// </summary>
        protected BaseDialogAnimation _showAnimation;
        /// <summary>
        /// Current hide animation
        /// </summary>
        protected BaseDialogAnimation _hideAnimation;

        #endregion

        #region Properties

        /// <summary>
        /// Canvas group for this dialog
        /// </summary>
        public CanvasGroup CanvasGroup => _canvasGroup;
        /// <summary>
        /// Graphic raycaster
        /// </summary>
        public GraphicRaycaster GraphicRaycaster => _graphicRayCaster;
        /// <summary>
        /// Rect transform
        /// </summary>
        public RectTransform Rect => _rectPanel;

        /// <summary>
        /// Main content rect transform
        /// </summary>
        public RectTransform MainContent => _mainContent;
        
        /// <summary>
        /// Dialog black mask
        /// </summary>
        public Image BlackMask => _blackMask;

        /// <summary>
        /// Dialog builder data
        /// </summary>
        public UIBuilder Builder { get; set; } = null;

        #endregion

        #region Event & Delegates

        /// <summary>
        /// Event fires when dialog request to hide
        /// </summary>
        public EventHandler<DialogHideArgs> RequestHide;

        #endregion

        #region Unity Events

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _graphicRayCaster = GetComponent<GraphicRaycaster>();
            _rectPanel = GetComponent<RectTransform>();

            if (_cam3D != null)
            {
                // var data = _cam3D.GetUniversalAdditionalCameraData();
                // data.cameraStack.Add(UIManager.Instance.UICamera);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup dialog data before run show animation
        /// </summary>
        /// <param name="args"></param>
        public virtual void Setup(object args = null)
        {
            _showAnimation = Builder?.ShowAnimation;
            _hideAnimation = Builder?.HideAnimation;
        }

        /// <summary>
        /// Callback when dialog is begin to run show animation
        /// </summary>
        public virtual void BeginTransitionIn() { }
        
        /// <summary>
        /// Callback when dialog is finish run show animation
        /// </summary>
        public virtual void TransitionInCompleted() { }
        
        /// <summary>
        /// Callback when dialog is begin to run hide animation
        /// </summary>
        public virtual void BeginTransitionOut() { }
        
        /// <summary>
        /// Callback when dialog is finish run hide animation
        /// </summary>
        public virtual void TransitionOutCompleted() { }

        /// <summary>
        /// Request to close this dialog
        /// </summary>
        public virtual void Close()
        {
            Close(null);
        }

        /// <summary>
        /// Request to close this dialog with data
        /// </summary>
        /// <param name="args"></param>
        public virtual void Close(object args)
        {
            RequestHide?.Invoke(this,new DialogHideArgs()
            {
                dialog = this,
                args = args
            });
        }

        /// <summary>
        /// Play dialog show animation
        /// </summary>
        public virtual IEnumerator PlayShowAnimation()
        {
            if (_showAnimation != null)
            {
                _showAnimation.Initialize(this);   
                yield return null;
            }
            
            this.gameObject.SetActive(true);
            BeginTransitionIn();

            if (_showAnimation != null)
            {
                yield return _showAnimation.Show();
            }
            
            TransitionInCompleted();
        }

        public virtual IEnumerator PlayHideAnimation()
        {
            if (_hideAnimation != null)
            {
                _hideAnimation.Initialize(this);
                yield return null;
            }
            
            BeginTransitionOut();
            if (_hideAnimation != null)
            {
                yield return _hideAnimation.Hide();
            }

            this.TransitionOutCompleted();
            this.gameObject.SetActive(false);
        }

        #endregion
    }   
}