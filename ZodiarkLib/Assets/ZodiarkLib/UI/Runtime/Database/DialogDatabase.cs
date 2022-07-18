using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZodiarkLib.UI
{
    [CreateAssetMenu(fileName = "DialogDatabase", menuName = "Systems/UI/Database/Dialog Database",order = 10)]
    public class DialogDatabase : ScriptableObject
    {
        [SerializeField]
        private List<DialogInfo> _dialogInfos = new List<DialogInfo>();

        public IReadOnlyCollection<DialogInfo> DialogInfos => _dialogInfos;

        /// <summary>
        /// Add new info to dialog database
        /// </summary>
        /// <param name="info"></param>
        public void AddInfo(DialogInfo info)
        {
            _dialogInfos.Add(info);
        }

        public void CleanUp()
        {
            foreach (var info in _dialogInfos)
            {
                if (Application.isEditor)
                {
                    GameObject.DestroyImmediate(info, true);
                }
                else
                {
                    GameObject.Destroy(info);   
                }
            }

            _dialogInfos.Clear();
        }
        
        public DialogInfo GetDialogInfoWithType(System.Type type)
        {
            return _dialogInfos.FirstOrDefault(x => 
                type.FullName != null && x.typeName == type.FullName);
        }
        
        public DialogInfo GetDialogInfoWithType<T>()
        {
            return GetDialogInfoWithType(typeof(T));
        }
    }   
}