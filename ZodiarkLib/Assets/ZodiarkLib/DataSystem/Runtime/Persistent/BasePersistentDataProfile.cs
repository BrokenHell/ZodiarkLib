using System;
using System.Collections.Generic;
using ZodiarkLib.Core.IO;
using ZodiarkLib.SaveSystem;

namespace ZodiarkLib.Data
{
    public abstract class BasePersistentDataProfile<T> : BaseDataProfile<T> where T : IPersistentData
    {
        #region Internal Classes & Structs

        [System.Serializable]
        public struct Config
        {
            public IReader reader;
            public IWriter writer;
            public string savePath;
        }

        #endregion
        
        #region Fields

        /// <summary>
        /// Save manager instance
        /// </summary>
        protected ISaveManager _saveManager;

        #endregion

        #region Properties

        public Config Configuration { get; protected set; }

        #endregion

        #region Constructors

        protected BasePersistentDataProfile(string profileName) : base(profileName)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize profile data
        /// </summary>
        /// <param name="config"></param>
        public void Initialize(Config config)
        {
            Configuration = config;
            _saveManager = new SaveManager();
            _saveManager.Initialize(config.savePath, config.reader, config.writer);
        }

        /// <summary>
        /// Load profile metadata first!
        /// </summary>
        public void LoadMetadata()
        {
            var meta = _saveManager.Load<ProfileMetadata>(GetFolderWithProfileName());
            if (meta != null)
            {
                Metadata = meta;
            }
        }

        /// <summary>
        /// Save whole profile data
        /// </summary>
        public void Save()
        {
            if (_dataDict is not {Count: > 0}) 
                return;
            
            foreach (var (key, value) in _dataDict)
            {
                if (value == null) 
                    continue;

                _saveManager.Save(value, GetFolderWithProfileName());
            }

            Metadata.modifiedDate = System.DateTime.Now.ToFileTime();
            Metadata.version += 1;
            IsDirty = false;

            _saveManager.Save(Metadata, GetFolderWithProfileName());
        }

        /// <summary>
        /// Save single data in profile data
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        public void SaveSingle<TK>() where TK : T
        {
            if (_dataDict is not {Count: > 0}) 
                return;

            var type = typeof(TK);
            if (!_dataDict.ContainsKey(type))
                return;
            
            _saveManager.Save(_dataDict[type], GetFolderWithProfileName());
            
            Metadata.modifiedDate = System.DateTime.Now.ToFileTime();
            Metadata.version += 1;
            IsDirty = true;

            _saveManager.Save(Metadata, GetFolderWithProfileName());
        }

        /// <summary>
        /// Load all datas
        /// </summary>
        public void Load()
        {
            var typeNames = new List<string>(Metadata.types);
            foreach (var typeName in typeNames)
            {
                var type = Type.GetType(typeName);
                var data = (T)_saveManager.Load(type,GetFolderWithProfileName());
                if (data != null)
                {
                    Add(data);
                }
            }
        }

        /// <summary>
        /// Load single data
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        public void LoadSingle<TK>() where TK : T , new()
        {
            var data = _saveManager.Load<TK>(GetFolderWithProfileName());
            if (data != null)
            {
                Add(data);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the folder path of profile data.
        /// </summary>
        /// <returns>The folder path.</returns>
        private string GetFolderWithProfileName()
        {
            return $"Profile/{Metadata.name}";
        }

        #endregion

    }   
}