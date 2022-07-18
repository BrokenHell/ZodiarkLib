using System.Linq;
using UnityEditor;
using UnityEngine;
using ZodiarkLib.UI.Properties;

namespace ZodiarkLib.UI.Editors
{
    [CustomPropertyDrawer(typeof(DialogPickerProperty))]
    public class DialogPickerPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                var contents = GetEnums();
                int index = IndexOf(property.stringValue, contents);
                index = EditorGUI.Popup(position, label, index, contents);
                if (index == -1)
                {
                    property.stringValue = "";
                }
                else
                {
                    property.stringValue = contents[index].text;   
                }

                EditorGUI.indentLevel = indent;
            }
            EditorGUI.EndProperty();
        }

        private int IndexOf(string label , GUIContent[] contents)
        {
            int index = 0;
            foreach (var content in contents)
            {
                if (content.text == label)
                    return index;
                index++;
            }

            return -1;
        }

        private GUIContent[] GetEnums()
        {
            var dialogs = EditorHelper.GetAssets("prefab", typeof(BaseDialog)).ToList();
            return dialogs.Select(dialog => new GUIContent(dialog.GetType().FullName)).ToArray();
        }
    }   
}