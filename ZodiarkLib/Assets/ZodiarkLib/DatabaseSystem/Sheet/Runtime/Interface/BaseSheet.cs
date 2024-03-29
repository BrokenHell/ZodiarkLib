using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ZodiarkLib.Database.Sheet
{
    public abstract class BaseSheet<T,K> : ISheet where T : IRecord<K>
    {
        #region Fields
        protected Dictionary<K, T> _dict = new();
        #endregion

        #region Properties

        public IReadOnlyDictionary<K, T> Records => _dict;

        #endregion

        #region Public Methods

        /// <summary>
        /// Add new record
        /// </summary>
        /// <param name="record"></param>
        public void Add(T record)
        {
            if (_dict.ContainsKey(record.Id))
            {
                return;
            }

            _dict[record.Id] = record;
        }

        /// <summary>
        /// Get record with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(K id)
        {
            return _dict.ContainsKey(id) ? _dict[id] : default;
        }

        /// <summary>
        /// Remove record with id
        /// </summary>
        /// <param name="id"></param>
        public void Remove(K id)
        {
            _dict.Remove(id);
        }

        /// <summary>
        /// Load records
        /// </summary>
        /// <param name="lines"></param>
        public abstract void Load(string[] lines);

        #endregion
        
        #region Private Methods

        protected string[] SplitContent(string content)
        {
            return Regex.Split(content, SheetManager.PHASE_SPLIT_RE);
        }

        #endregion
    }
}