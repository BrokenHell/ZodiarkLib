using System;
using System.Collections.Generic;

namespace ZodiarkLib.Data
{
    public abstract class BaseDataProfile<T> : IDataProfile
                            where T : IData
    {
        #region Constructor

        /// <summary>
        /// Constructor to create new profile
        /// </summary>
        /// <param name="profileName"></param>
        protected BaseDataProfile(string profileName)
        {
            Metadata.name = profileName;
        }

        #endregion
        
        #region Fields

        /// <summary>
        /// Data dictionary
        /// </summary>
        protected Dictionary<Type, IData> _dataDict = new(10);

        #endregion
        
        #region Properties

        public ProfileMetadata Metadata { get; protected set; } = new ();
        public bool IsDirty { get; protected set; } = false;
        public IReadOnlyDictionary<Type, IData> DataDict => _dataDict;

        #endregion

        #region Public Methods

        /// <summary>
        /// Add list of data to profile
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="replaceDuplicate"></param>
        public virtual void Add(ICollection<IData> dataList, bool replaceDuplicate = true)
        {
            foreach (var data in dataList)
            {
                var type = data.GetType();
                if (replaceDuplicate)
                {
                    //Make sure its not duplicate
                    if (!Metadata.types.Contains(type.FullName))
                    {
                        Metadata.types.Add(type.FullName);   
                    }
                    _dataDict[type] = data;   
                }
                else
                {
                    if (!_dataDict.ContainsKey(type))
                    {
                        //Make sure its not duplicate
                        if (!Metadata.types.Contains(type.FullName))
                        {
                            Metadata.types.Add(type.FullName);   
                        }
                        _dataDict[type] = data;
                    }
                }
            }
        }

        /// <summary>
        /// Add new data to profile
        /// </summary>
        /// <param name="data"></param>
        /// <param name="replaceDuplicate"></param>
        /// <typeparam name="TK"></typeparam>
        /// <exception cref="ArgumentException"></exception>
        public virtual void Add<TK>(TK data, bool replaceDuplicate = true) where TK : T
        {
            _dataDict ??= new(10);
            
            var type = data.GetType();
            if (!replaceDuplicate && _dataDict.ContainsKey(type))
            {
                throw new ArgumentException($"Data with type {type} already exits!!!");
            }

            _dataDict[type] = data;
            
            //Make sure its not duplicate
            if (!Metadata.types.Contains(type.FullName))
            {
                Metadata.types.Add(type.FullName);   
            }
            IsDirty = true;
        }

        /// <summary>
        /// Remove data from profile
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        public virtual void Remove<TK>() where TK : T
        {
            _dataDict ??= new(10);
            
            var type = typeof(TK);
            if (_dataDict.ContainsKey(type))
            {
                IsDirty = true;
                Metadata.types.Remove(type.FullName);
                _dataDict.Remove(type);
            }
        }

        /// <summary>
        /// Get data from profile
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <returns></returns>
        public virtual TK Get<TK>() where TK : T
        {
            _dataDict ??= new(10);

            var type = typeof(TK);
            if (_dataDict.ContainsKey(type))
            {
                return (TK)_dataDict[type];
            }

            return default;
        }

        /// <summary>
        /// Get data from profile if there isn't exist one then it will create new instance
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <returns></returns>
        public TK SafeGet<TK>() where TK : T, new()
        {
            _dataDict ??= new(10);

            var type = typeof(TK);
            if (_dataDict.ContainsKey(type))
            {
                return (TK)_dataDict[type];
            }

            var data = new TK();
            Add(data);
            IsDirty = true;

            return data;
        }
        
        #endregion
    }
}