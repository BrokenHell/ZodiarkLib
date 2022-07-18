using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZodiarkLib.UI
{
    /// <summary>
    /// Base class for dialog animation
    /// </summary>
    public abstract class BaseDialogAnimation : ScriptableObject
    {
        #region Fields

        /// <summary>
        /// Reference to dialog
        /// </summary>
        protected BaseDialog _dialog;

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Init animation for specific dialog
        /// </summary>
        /// <param name="dialog"></param>
        public virtual void Initialize(BaseDialog dialog)
        {
            _dialog = dialog;
        }

        /// <summary>
        /// Clean up animation to reset values and other things.
        /// </summary>
        public virtual void Cleanup()
        {
            _dialog = null;
        }
        
        /// <summary>
        /// Play show animation
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator Show();
        /// <summary>
        /// Play hide animation
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator Hide();
        /// <summary>
        /// Stop dialog animation immediately and trigger callback.
        /// </summary>
        public abstract void StopImmediately();
        
        #endregion
    }   
}