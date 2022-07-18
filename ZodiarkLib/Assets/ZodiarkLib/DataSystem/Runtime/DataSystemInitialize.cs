#if INITIALIZE_SYSTEM_ENABLE
using System.Collections;
using ZodiarkLib.Core;
using ZodiarkLib.Data.Extensions;
using ZodiarkLib.Initialize;
using ZodiarkLib.SaveSystem.IO;
using ZodiarkLib.Utils;
using UnityEngine;

namespace ZodiarkLib.Data
{
    [CreateAssetMenu(menuName = "Systems/Data/Data Initialize", fileName = "DataInitialize")]
    public class DataSystemInitialize : BaseScriptableJob
    {
        [SerializeField] private string _defaultProfileName;
        
        protected override IEnumerator InternalProcess()
        {
            LoadGlobalData();
            LoadProfileData();
            LoadMemoryData();
            yield break;
        }

        private void LoadGlobalData()
        {
            var globalData = new GlobalDataProfile();
            globalData.LazyInit();

            ServiceLocator.Add(globalData);
        }

        private void LoadProfileData()
        {
            ServiceLocator.Add(PersistentProfileExtensions.LoadProfileData(_defaultProfileName));
        }

        private void LoadMemoryData()
        {
            ServiceLocator.Add(new MemoryDataProfile());
        }
    }
}
#endif