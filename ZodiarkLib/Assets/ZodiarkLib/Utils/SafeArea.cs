using UnityEngine;
using UnityEngine.UI;

namespace ZodiarkLib.Utils
{
    [System.Serializable]
    public enum ChangeType
    {
        Position,
        Size
    }

    [System.Serializable]
    public enum SafeAnchor
    {
        Top,
        Bot,
        Left,
        Right
    }

    public class SafeArea : MonoBehaviour
    {
        #region [ Fields ]

        public ChangeType changeType = ChangeType.Position;
        public SafeAnchor anchor;

        public RectTransform panel;

        private Rect _lastArea;

        #endregion

        #region [ Unity Lifecycle ]

        private void Awake()
        {
            if (changeType == ChangeType.Position)
            {
                ApplyChangePos();
            }
            else if (changeType == ChangeType.Size)
            {
                ApplyChangeSize();
            }
        }

        private void Update()
        {
            if (changeType == ChangeType.Position)
            {
                ApplyChangePos();
            }
            else if (changeType == ChangeType.Size)
            {
                ApplyChangeSize();
            }
        }

        #endregion

        #region [ Private Methods ]

        private void ApplyChangePos()
        {
            var safeArea = Screen.safeArea;

            if (_lastArea == safeArea)
            {
                return;
            }
            _lastArea = safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;
        }

        private void ApplyChangeSize()
        {
            var safeArea = Screen.safeArea;

            if (_lastArea == safeArea)
            {
                return;
            }
            _lastArea = safeArea;
            var root = transform.root.GetComponent<CanvasScaler>();
            var designResolution = root.referenceResolution;

            var increaseTop = safeArea.position.y;
            increaseTop /= Screen.height;
            increaseTop *= designResolution.y;

            if (anchor == SafeAnchor.Top)
            {
                panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panel.sizeDelta.y + increaseTop);
                return;
            }

            var increaseBot = Screen.height - (safeArea.position.y + safeArea.height);
            increaseBot /= Screen.height;
            increaseBot *= designResolution.y;

            if (anchor == SafeAnchor.Bot)
            {
                panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panel.sizeDelta.y + increaseBot);
                return;
            }
        }

        #endregion
    }
}