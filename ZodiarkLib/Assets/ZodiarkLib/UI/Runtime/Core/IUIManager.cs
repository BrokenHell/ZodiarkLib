using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZodiarkLib.UI
{
    public class UIBuilder
    {
        #region Fields
        
        public System.Action onShow;
        public System.Action<object> onHide;

        public BaseDialogAnimation ShowAnimation { get; set; } = null;
        public BaseDialogAnimation HideAnimation { get; set; } = null;

        public Coroutine ShowRoutine { get; set; } = null;
        public Coroutine HideRoutine { get; set; } = null;

        public string CanvasName { get; set; } = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Add on show event trigger when dialog is load & show success
        /// </summary>
        /// <param name="onShow"></param>
        /// <returns></returns>
        public UIBuilder AddEventOnShow(System.Action callback)
        {
            this.onShow = callback;
            return this;
        }

        /// <summary>
        /// Add on hide event trigger when dialog is hide
        /// </summary>
        /// <param name="onHide"></param>
        /// <returns></returns>
        public UIBuilder AddEventOnHide(System.Action<object> callback)
        {
            this.onHide = callback;
            return this;
        }

        /// <summary>
        /// Override canvas layer for this dialog
        /// </summary>
        /// <param name="canvasName"></param>
        /// <returns></returns>
        public UIBuilder SetCanvas(string canvasName)
        {
            CanvasName = canvasName;
            return this;
        }

        /// <summary>
        /// Override show animation
        /// </summary>
        /// <param name="animation"></param>
        /// <returns></returns>
        public UIBuilder SetShowAnimation(BaseDialogAnimation animation)
        {
            ShowAnimation = animation;
            return this;
        }

        /// <summary>
        /// Override hide animation
        /// </summary>
        /// <param name="animation"></param>
        /// <returns></returns>
        public UIBuilder SetHideAnimation(BaseDialogAnimation animation)
        {
            HideAnimation = animation;
            return this;
        }

        #endregion
    }
    
    /// <summary>
    /// UI manager use to manage all UI including transitions & layers
    /// </summary>
    public interface IUIManager
    {
        [System.Serializable]
        public class DialogEventArgs : EventArgs
        {
            public Type type;
            public object args;
        }

        /// <summary>
        /// Event fires when dialog in UI system is shown
        /// </summary>
        event EventHandler<DialogEventArgs> OnDialogShown;
        /// <summary>
        /// Event fires when dialog in UI system is hide
        /// </summary>
        event EventHandler<DialogEventArgs> OnDialogHide;

        /// <summary>
        /// Current dialog stack
        /// </summary>
        List<BaseDialog> DialogStack { get; }
        /// <summary>
        /// Canvas setting database
        /// </summary>
        CanvasDatabase CanvasDatabase { get; }
        /// <summary>
        /// Dialog setting database
        /// </summary>
        DialogDatabase DialogDatabase { get; }
        /// <summary>
        /// Check if UI manager had been initialized
        /// </summary>
        bool HasInitialized { get; }
        
        /// <summary>
        /// Initialize ui system
        /// </summary>
        /// <returns></returns>
        IEnumerator Initialize(CanvasDatabase canvasDatabase, DialogDatabase dialogDatabase);

        /// <summary>
        /// Show dialog with type
        ///     Since the dialog loading will be async so you will need to wait for 1 frame before call "Find"
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        UIBuilder Show(System.Type type, object args = null);
        
        /// <summary>
        /// Show dialog with type
        ///     Since the dialog loading will be async so you will need to wait for 1 frame before call "Find"
        /// </summary>
        /// <param name="args"></param>
        /// <typeparam name="T"></typeparam>
        UIBuilder Show<T>(object args = null) where T : BaseDialog;
        
        /// <summary>
        /// Hide current showing dialog
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        void Hide(System.Type type, object args = null);
        
        /// <summary>
        /// Hide current showing dialog
        /// </summary>
        /// <param name="args"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        void Hide<T>(object args = null) where T : BaseDialog;
        
        /// <summary>
        /// Get current dialog
        /// </summary>
        /// <param name="includeInactive">Get dialog from pool which is inactive</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Find<T>(bool includeInactive = false) where T : BaseDialog;
    }   
}