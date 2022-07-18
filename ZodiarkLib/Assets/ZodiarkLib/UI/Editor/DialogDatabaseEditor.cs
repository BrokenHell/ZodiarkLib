using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ZodiarkLib.UI.Editors
{
    [CustomEditor(typeof(DialogDatabase))]
    public class DialogDatabaseEditor : Editor
    {
        private DialogDatabase _dialogDB;
        
        private void OnEnable()
        {
            _dialogDB = target as DialogDatabase;
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_dialogInfos"), 
                    GUILayout.ExpandWidth(true));
            }
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Fetch all dialogs", GUILayout.ExpandWidth(true)))
            {
                if (_dialogDB == null) 
                    return;
                
                var dialogs = EditorHelper.GetAssets("prefab", typeof(BaseDialog)).ToList();

                foreach (var item in dialogs)
                {
                    var setting = _dialogDB.DialogInfos?.FirstOrDefault(x => 
                                                x.typeName == item.GetType().FullName);
                    if (setting != null)
                    {
                        setting.typeName = item.GetType().FullName;
                        setting.assetKey = item.name;
                        setting.name = setting.typeName;
                    }
                    else
                    {
                        setting = ScriptableObject.CreateInstance<DialogInfo>();
                        setting.assetKey = item.name;
                        setting.typeName = item.GetType().FullName;
                        setting.canvasName = "main";
                        setting.lockInteractWhenTransition = true;
                        setting.name = setting.typeName;
                        
                        AssetDatabase.AddObjectToAsset(setting, _dialogDB);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                        _dialogDB.AddInfo(setting);
                    }
                }
            }

            if (GUILayout.Button("Clear", GUILayout.ExpandWidth(true)))
            {
                if (_dialogDB == null) 
                    return;
                _dialogDB.CleanUp();
            }
        }
    }   
}