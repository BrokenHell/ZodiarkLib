#if INITIALIZE_SYSTEM_ENABLE
using System;
using System.Collections;
using UnityEngine;
using ZodiarkLib.Core;
using ZodiarkLib.Data;
using ZodiarkLib.Initialize;
using ZodiarkLib.Sound.Extensions;

namespace ZodiarkLib.Sound
{
    public class SoundSystemInitialize : BaseScriptableJob
    {
        [SerializeField] private SoundDatabase _soundDatabase;
        
        protected override IEnumerator InternalProcess()
        {
            ISoundManager soundManager = null;

            var oldSoundManager = FindObjectOfType<SoundManager>();
            if (oldSoundManager != null)
            {
                GameObject.Destroy(oldSoundManager.gameObject);
            }

            soundManager = new GameObject("[SoundManager]").AddComponent<SoundManager>();
            soundManager.SoundPool = new SoundPool();
            soundManager.SoundDatabase = _soundDatabase;

            var saveData = LoadSaveData();
            soundManager.IsMute = saveData.isMute;
            soundManager.IsMuteBackground = saveData.isMuteBg;
            soundManager.IsMuteSound = saveData.isMuteFx;
            soundManager.Volume = saveData.volume;

            soundManager.Initialize();

            GameObject.DontDestroyOnLoad((soundManager as SoundManager).gameObject);
            
            ServiceLocator.Add<ISoundManager>(soundManager, true);
            yield break;
        }

        public override Type[] DependencyTypes => new[] {typeof(DataSystemInitialize)};

        private SoundUserData LoadSaveData()
        {
            GlobalDataProfile globalProfile = ServiceLocator.Get<GlobalDataProfile>();
            return globalProfile.SafeGet<SoundUserData>();
        }
    }   
}
#endif