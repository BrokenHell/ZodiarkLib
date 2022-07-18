using System;
using System.Collections.Generic;

namespace ZodiarkLib.Data
{
    public interface IDataManager
    {
        /// <summary>
        /// Event fire when new data is updated;
        /// </summary>
        event Action<IDataProfile> OnDataProfileAdded;
        /// <summary>
        /// Event fire when data is removed
        /// </summary>
        event Action<IDataProfile> OnDataProfileRemoved;
        /// <summary>
        /// Data profile dict
        /// </summary>
        IDictionary<Type, IDataProfile> DataProfiles { get; }
        /// <summary>
        /// Add new data profile
        /// </summary>
        /// <param name="dataProfile"></param>
        /// <typeparam name="T"></typeparam>
        void Add<T>(T dataProfile) where T : IDataProfile;
        /// <summary>
        /// Remove data profile
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Remove<T>() where T : IDataProfile;
        /// <summary>
        /// Get data profile
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Get<T>() where T : IDataProfile;
    }   
}