using System.Collections.Generic;

namespace ZodiarkLib.Database
{
    public abstract class BaseDatabase<T,K> : IDatabase where T : IDataRecord<K>
    {
        #region Fields
        public Dictionary<K, T> records = new();
        #endregion

        #region Public Methods

        /// <summary>
        /// Add new record
        /// </summary>
        /// <param name="record"></param>
        public void Add(T record)
        {
            if (records.ContainsKey(record.Id))
            {
                return;
            }

            records[record.Id] = record;
        }

        /// <summary>
        /// Get record with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(K id)
        {
            return records.ContainsKey(id) ? records[id] : default;
        }

        /// <summary>
        /// Remove record with id
        /// </summary>
        /// <param name="id"></param>
        public void Remove(K id)
        {
            records.Remove(id);
        }

        #endregion
    }
}