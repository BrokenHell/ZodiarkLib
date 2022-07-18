using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ZodiarkLib.Initialize.Editors
{
 
    [CustomEditor(typeof(JobContainer))]
    public class JobContainerEditor : Editor
    {
        private JobContainer _jobContainer;
        
        private void OnEnable()
        {
            _jobContainer = target as JobContainer;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Update Dependencies", GUILayout.ExpandWidth(true)))
            {
                _jobContainer.UpdateDependencies();
            }
        }
    }   
}