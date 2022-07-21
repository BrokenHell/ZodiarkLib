using System.Collections.Generic;

namespace ZodiarkLib.Database
{
    public interface IDatabaseManager
    {
        /// <summary>
        /// Get all sheets
        /// </summary>
        IReadOnlyDictionary<System.Type, IDatabase> Databases { get; }
        /// <summary>
        /// Initialize and fetch data into sheets
        /// </summary>
        void Initialize(JsonDatabaseCollectionSO collection);
        /// <summary>
        /// Add new sheet
        /// </summary>
        /// <param name="database"></param>
        void AddDatabase(IDatabase database);
        /// <summary>
        /// Remove sheet by type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void RemoveDatabase<T>() where T : IDatabase;
        /// <summary>
        /// Get sheet by type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T GetDatabase<T>() where T : IDatabase;
    }   
}
