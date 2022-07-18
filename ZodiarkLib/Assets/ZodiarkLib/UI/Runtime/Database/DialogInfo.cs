using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZodiarkLib.UI
{
    public class DialogInfo : ScriptableObject
    {
        public string assetKey;
        public string typeName;
        public string canvasName;
        [Tooltip("Dialog will not destroy during the whole game")]
        public bool keepInMemory;
        [Tooltip("Preload dialog in specific scene")]
        public string[] preloadSceneNames;
        [Tooltip("Lock other UIs interaction when play show/hide animation")]
        public bool lockInteractWhenTransition = true;
        public BaseDialogAnimation showAnimation;
        public BaseDialogAnimation hideAnimation;
    }   
}