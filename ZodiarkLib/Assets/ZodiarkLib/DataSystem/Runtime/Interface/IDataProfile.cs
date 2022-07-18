using System;
using System.Collections.Generic;

namespace ZodiarkLib.Data
{
    [System.Serializable]
    public class ProfileMetadata
    {
        public string name;
        public long modifiedDate;
        public int version;
        public Dictionary<string, string> meta = new();
        public HashSet<string> types = new();
    }
    
    /// <summary>
    /// Data profile contains all the data related to this profile
    /// </summary>
    public interface IDataProfile
    {
        /// <summary>
        /// Get/set current metadata
        /// </summary>
        ProfileMetadata Metadata { get; }
        /// <summary>
        /// Check if profile data had been change
        /// </summary>
        bool IsDirty { get; }
        /// <summary>
        /// Data map
        /// </summary>
        IReadOnlyDictionary<Type, IData> DataDict { get; }
    }   
}