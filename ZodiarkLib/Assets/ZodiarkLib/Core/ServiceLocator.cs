using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace ZodiarkLib.Core
{
    /// <summary>
    /// Service locator use contains services along with its interface
    /// </summary>
    public static class ServiceLocator
    {
        #region Fields

        /// <summary>
        /// Normal services map
        /// </summary>
        private static Dictionary<Type, object> s_map = new();
        /// <summary>
        /// Unity services map
        /// </summary>
        private static Dictionary<Type, object> s_unityObjectMap = new();
        /// <summary>
        /// Multiple services share same type but with different ids map
        /// </summary>
        private static Dictionary<Type, Dictionary<string,object>> s_multiMap = new();

        #endregion

        #region Public Methods

        /// <summary>
        /// Add multi service
        /// </summary>
        /// <param name="id">Unique id of this service</param>
        /// <param name="service"></param>
        /// <typeparam name="T">Type of service</typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DuplicateNameException"></exception>
        public static void AddMulti<T>(string id, T service)
        {
            var type = typeof(T);
            if (service == null)
                throw new ArgumentNullException(
                    $"[Service Locator] - Can't add null to service type {type.FullName}");
            
            if (s_multiMap.ContainsKey(type))
            {
                if (s_multiMap[type].ContainsKey(id))
                {
                    throw new DuplicateNameException(
                        $"[Service Locator] - Service with type {type.FullName} and ID {id} already exist!!!");   
                }
                
                s_multiMap[type].Add(id, service);
            }
            else
            {
                s_multiMap.Add(type, new Dictionary<string, object>{{id, service}});
            }
        }

        /// <summary>
        /// Add new service
        /// </summary>
        /// <param name="service"></param>
        /// <param name="replace"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DuplicateNameException"></exception>
        public static void Add<T>(T service , bool replace = false)
        {
            var type = typeof(T);
            if (service == null)
                throw new ArgumentNullException(
                    $"[Service Locator] - Can't add null to service type {type.FullName}");
            
            if (s_map.ContainsKey(type) && !replace )
            {
                throw new DuplicateNameException(
                    $"[Service Locator] - Service with type {type.FullName} already exist!!!");
            }
            
            s_map[type] = service;
        }

        /// <summary>
        /// Add service from Unity Game Object which is a Monobehavior
        /// </summary>
        /// <param name="go"></param>
        /// <param name="replace"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DuplicateNameException"></exception>
        /// <exception cref="MissingComponentException"></exception>
        public static void AddFromGO<T>(GameObject go , bool replace = false)
        {
            var type = typeof(T);
            if (go == null)
                throw new ArgumentNullException(
                    $"[Service Locator] - Can't add null object to service type {type.FullName}");
            
            if (s_unityObjectMap.ContainsKey(type) && !replace)
            {
                throw new DuplicateNameException(
                    $"[Service Locator] - Service with type {type.FullName} already exist!!!");
            }

            var component = go.GetComponent(type);
            if (component == null)
            {
                throw new MissingComponentException(
                    $"[Service Locator] - Can't bind since Object {go.name} is missing Component of type {type.FullName}");
            }
            
            UnityEngine.Object.DontDestroyOnLoad(go);
            s_unityObjectMap[type] = component;
        }
        
        /// <summary>
        /// Get Service by type either normal type or unity type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            var type = typeof(T);
            if (s_map.ContainsKey(type))
            {
                return (T)s_map[type];
            }

            if (s_unityObjectMap.ContainsKey(type))
            {
                return (T)s_unityObjectMap[type];
            }

            return default;
        }

        /// <summary>
        /// Get multi service instance by type and its Id
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetMulti<T>(string id)
        {
            var type = typeof(T);
            if (s_multiMap.ContainsKey(type))
            {
                if (s_multiMap[type].ContainsKey(id))
                {
                    return (T)s_multiMap[type][id];
                }
            }

            return default;
        }

        /// <summary>
        /// Remove service by type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Remove<T>()
        {
            var type = typeof(T);
            if (s_map.ContainsKey(type))
            {
                s_map.Remove(type);
                return;
            }

            if (s_unityObjectMap.ContainsKey(type))
            {
                DestroyInstance(s_unityObjectMap[type]);
                s_unityObjectMap.Remove(type);
            }
        }

        /// <summary>
        /// Remove multi service by Id
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        public static void RemoveMulti<T>(string id)
        {
            var type = typeof(T);
            if (s_multiMap.ContainsKey(type))
            {
                if (s_multiMap[type].ContainsKey(id))
                {
                    s_multiMap[type].Remove(id);
                }

                if (s_multiMap[type].Count == 0)
                {
                    s_multiMap.Remove(type);
                }
            }
        }

        /// <summary>
        /// Remove all services
        /// </summary>
        public static void RemoveAll()
        {
            s_map.Clear();

            foreach (var obj in s_unityObjectMap)
            {
                DestroyInstance(obj.Value);
            }
            
            s_unityObjectMap.Clear();
            s_multiMap.Clear();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Destroy Unity instance
        /// </summary>
        /// <param name="obj"></param>
        private static void DestroyInstance(object obj)
        {
            var mono = obj as MonoBehaviour;
            if (mono != null && mono.gameObject != null)
            {
                UnityEngine.Object.Destroy(mono.gameObject);
            }
        }

        #endregion
    }   
}