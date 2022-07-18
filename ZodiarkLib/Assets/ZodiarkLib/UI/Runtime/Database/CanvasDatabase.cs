using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZodiarkLib.UI
{
    [CreateAssetMenu(fileName = "CanvasDatabase", menuName = "Systems/UI/Database/Canvas Database",order = 10)]
    public class CanvasDatabase : ScriptableObject
    {
        [Serializable]
        public class CanvasInfo
        {
            public string canvasName;
            public int order;
        }
        
        [SerializeField]
        private CanvasInfo[] _canvasInfos;

        public CanvasInfo[] Canvases => _canvasInfos;
    }
}