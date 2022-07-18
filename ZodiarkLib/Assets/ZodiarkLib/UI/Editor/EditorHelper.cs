using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ZodiarkLib.UI.Editors
{
    public static class EditorHelper
    {
        public static IEnumerable<Object> GetAssets(string type, System.Type componentType, string label = "")
        {
            var regex = "";
            regex = !string.IsNullOrEmpty(label) ? $"t:{type} l:{label}" : $"t:{type}";
            
            return AssetDatabase.FindAssets(regex)
                .Select(guid => AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), componentType))
                .Where(x => x != null);
        }
    }   
}