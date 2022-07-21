using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZodiarkLib.Database
{
 
    public class DatabaseManager : IDatabaseManager
    {
        #region Fields
        private Dictionary<Type, IDatabase> _databases = new();
        #endregion

        #region Properties
        public IReadOnlyDictionary<Type, IDatabase> Databases => _databases;
        #endregion

        #region Public Methods
        
        public void Initialize(JsonDatabaseCollectionSO collection)
        {
            foreach (var jsonTxt in collection.Jsons)
            {
                var type = Type.GetType(jsonTxt.name);
                if(type == null)
                    continue;
                var database = ParseContent(type, jsonTxt.text);
                if (database == null) 
                    continue;
                AddDatabase(database);
            }
        }

        public void AddDatabase(IDatabase database)
        {
            var type = database.GetType();
            if (_databases.ContainsKey(type))
            {
                return;
            }

            _databases[type] = database;
        }

        public void RemoveDatabase<T>() where T : IDatabase
        {
            var type = typeof(T);
            _databases.Remove(type);
        }

        public T GetDatabase<T>() where T : IDatabase
        {
            var type = typeof(T);
            if (_databases.ContainsKey(type))
            {
                return (T)_databases[type];
            }

            return default;
        }
        
        #endregion

        #region Private Methods

        private IDatabase ParseContent(Type type, string json)
        {
            var instance = Activator.CreateInstance(type);
            JsonConvert.PopulateObject(json, instance);

            return (IDatabase)instance;
        }

        #endregion
    }   
}