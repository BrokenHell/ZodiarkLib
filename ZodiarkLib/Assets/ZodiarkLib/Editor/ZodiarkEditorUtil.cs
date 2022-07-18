using System.Linq;
using UnityEditor;
using UnityEngine;
using ZodiarkLib.Utils;

namespace Zodiark.Editor
{
    public static class ZodiarkEditorUtil
    {
        [MenuItem("Tools/Scripting Symbols/Enable Initialize System")]
        private static void AddInitializeScriptingSymbols()
        {
            AddScriptingSymbols(BuildTargetGroup.Android, 
                "INITIALIZE_SYSTEM_ENABLE");
            AddScriptingSymbols(BuildTargetGroup.iOS, 
                "INITIALIZE_SYSTEM_ENABLE");
            AddScriptingSymbols(BuildTargetGroup.Standalone, 
                "INITIALIZE_SYSTEM_ENABLE");
            AddScriptingSymbols(BuildTargetGroup.PS4, 
                "INITIALIZE_SYSTEM_ENABLE");
            AddScriptingSymbols(BuildTargetGroup.PS5, 
                "INITIALIZE_SYSTEM_ENABLE");
            AddScriptingSymbols(BuildTargetGroup.Switch, 
                "INITIALIZE_SYSTEM_ENABLE");
        }
        
        [MenuItem("Tools/Scripting Symbols/Disable Initialize System")]
        private static void RemoveInitializeScriptingSymbols()
        {
            RemoveScriptingSymbols(BuildTargetGroup.Android, 
                "INITIALIZE_SYSTEM_ENABLE");
            RemoveScriptingSymbols(BuildTargetGroup.iOS, 
                "INITIALIZE_SYSTEM_ENABLE");
            RemoveScriptingSymbols(BuildTargetGroup.Standalone, 
                "INITIALIZE_SYSTEM_ENABLE");
            RemoveScriptingSymbols(BuildTargetGroup.PS4, 
                "INITIALIZE_SYSTEM_ENABLE");
            RemoveScriptingSymbols(BuildTargetGroup.PS5, 
                "INITIALIZE_SYSTEM_ENABLE");
            RemoveScriptingSymbols(BuildTargetGroup.Switch, 
                "INITIALIZE_SYSTEM_ENABLE");
        }
        

        [MenuItem("Tools/Delete/Delete All")]
        private static void DeleteAll()
        {
            if (EditorUtility.DisplayDialog("Warning", "Do you want to delete all persistent contents?", "Yes", "No"))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                DeletePersistentDataRecursive(Application.persistentDataPath);
                DeletePersistentDataRecursive(PathHelper.GetWritableFolder());
                Debug.Log("Delete successful!");
            }
        }

        [MenuItem("Tools/Delete/Delete PlayerPref")]
        private static void DeletePlayerPref()
        {
            if (EditorUtility.DisplayDialog("Warning", "Do you want to delete all PlayerPref contents?", "Yes", "No"))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                Debug.Log("Delete Unity PlayerPref successful!");
            }
        }

        [MenuItem("Tools/Delete/Delete Persistent Data")]
        private static void DeletePersistentData()
        {
            if (EditorUtility.DisplayDialog("Warning", "Do you want to delete all Game Database contents?", "Yes", "No"))
            {
                DeletePersistentDataRecursive(Application.persistentDataPath);
                DeletePersistentDataRecursive(PathHelper.GetWritableFolder());
                Debug.Log("Clear Persistent data successful!");
            }
        }

        private static void DeletePersistentDataRecursive(string parent)
        {
            var dirs = System.IO.Directory.GetDirectories(parent);

            foreach (var item in dirs)
            {
                if (item.Contains("Global") || item.Contains("Profile"))
                {
                    System.IO.Directory.Delete(item, true);
                }
                else
                {
                    DeletePersistentDataRecursive(item);
                }
            }
        }
        
        private static void AddScriptingSymbols(BuildTargetGroup targetGroup,string symbol)
        {
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            var splits = symbols.Split(";").ToList();
            if (splits.Contains(symbol))
                return;

            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, symbols + ";" + symbol);
        }
        
        private static void RemoveScriptingSymbols(BuildTargetGroup targetGroup,string symbol)
        {
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            var splits = symbols.Split(";").Where(x => x != symbol);
            symbols = string.Join(";", splits);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, symbols);
        }
    }
    
    public static class OpenFolder
    {
        private static bool IsInMacOS => 
            UnityEngine.SystemInfo.operatingSystem.IndexOf("Mac OS", System.StringComparison.Ordinal) != -1;

        private static bool IsInWinOS => 
            UnityEngine.SystemInfo.operatingSystem.IndexOf("Windows", System.StringComparison.Ordinal) != -1;

        [UnityEditor.MenuItem("Tools/Open Project folder")]
        public static void OpenProjectFolder()
        {
            Open(UnityEngine.Application.dataPath);
        }

        [UnityEditor.MenuItem("Tools/Open Persistent folder")]
        public static void OpenSaveDataFolder()
        {
            Open(UnityEngine.Application.persistentDataPath);
        }

        private static void OpenInMac(string path)
        {
            var openInsidesOfFolder = false;

            // try mac
            var macPath = path.Replace("\\", "/"); // mac finder doesn't like backward slashes

            if (System.IO.Directory.Exists(macPath)) // if path requested is a folder, automatically open insides of that folder
            {
                openInsidesOfFolder = true;
            }

            if (!macPath.StartsWith("\"", System.StringComparison.Ordinal))
            {
                macPath = $"\"{macPath}";
            }

            if (!macPath.EndsWith("\"", System.StringComparison.Ordinal))
            {
                macPath = $"{macPath}\"";
            }

            var arguments = $"{(openInsidesOfFolder ? "" : "-R ")}{macPath}";

            try
            {
                System.Diagnostics.Process.Start("open", arguments);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                // tried to open mac finder in windows
                // just silently skip error
                // we currently have no platform define for the current OS we are in, so we resort to this
                e.HelpLink = ""; // do anything with this variable to silence warning about not using it
            }
        }

        private static void OpenInWin(string path)
        {
            var openInsidesOfFolder = false;

            // try windows
            var winPath = path.Replace("/", "\\"); // windows explorer doesn't like forward slashes

            if (System.IO.Directory.Exists(winPath)) // if path requested is a folder, automatically open insides of that folder
            {
                openInsidesOfFolder = true;
            }

            try
            {
                System.Diagnostics.Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + winPath);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                // tried to open win explorer in mac
                // just silently skip error
                // we currently have no platform define for the current OS we are in, so we resort to this
                e.HelpLink = ""; // do anything with this variable to silence warning about not using it
            }
        }

        private static void Open(string path)
        {
            if (IsInWinOS)
            {
                OpenInWin(path);
            }
            else if (IsInMacOS)
            {
                OpenInMac(path);
            }
            else // couldn't determine OS
            {
                OpenInWin(path);
                OpenInMac(path);
            }
        }
    }
}