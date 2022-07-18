using System;
using UnityEngine;
using UnityEngine.Events;
using ZodiarkLib.UI.Properties;

namespace ZodiarkLib.UI
{
    public class OpenUI : MonoBehaviour
    {
        #region Fields

        [DialogPickerProperty]
        [SerializeField] private string _dialogTypeName;
        [SerializeField] private bool _autoRun;
        [SerializeField] private UnityEvent<Type> _onUIBeginOpen;
        [SerializeField] private UnityEvent<Type> _onUIOpened;
        [SerializeField] private UnityEvent<Type, object> _onUIClosed;

        #endregion

        #region Unity Events

        private void Awake()
        {
            if (_autoRun)
            {
                Open();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Open dialog
        /// </summary>
        public void Open()
        {
            var type = Type.GetType(_dialogTypeName);
            _onUIBeginOpen?.Invoke(type);
            UIManager.Instance.Show(type)
                .AddEventOnShow(() =>
                {
                    _onUIOpened?.Invoke(type);
                })
                .AddEventOnHide(args =>
                {
                    _onUIClosed?.Invoke(type, args);
                });
        }

        #endregion
    }   
}