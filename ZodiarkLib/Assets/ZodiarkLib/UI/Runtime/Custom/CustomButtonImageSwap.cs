using UnityEngine;
using UnityEngine.UI;

namespace ZodiarkLib.UI.Custom
{
    [RequireComponent(typeof(CustomButton))]
    public class CustomButtonImageSwap : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Image targetGraphic;
        [SerializeField] private Sprite normalImage;
        [SerializeField] private Sprite disabledImage;

        #endregion

        #region Unity Events

        private void Awake()
        {
            var button = GetComponent<CustomButton>();
            button.StateChangeEvent.AddListener(OnStateChange);
        }

        private void OnDestroy()
        {
            var button = GetComponent<CustomButton>();
            button.StateChangeEvent.RemoveListener(OnStateChange);
        }

        #endregion

        #region Private Methods

        private void OnStateChange(CustomButton.State state)
        {
            targetGraphic.sprite = state switch
            {
                CustomButton.State.Normal => normalImage,
                CustomButton.State.Disabled => disabledImage,
                _ => targetGraphic.sprite
            };
        }

        #endregion 
    }   
}