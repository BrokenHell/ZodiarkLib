using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZodiarkLib.Event
{
    /// <summary>
    /// Event messenger which is use for broadcasting event
    /// <seealso cref="IEventBus"/>
    /// </summary>
    public sealed class EventBus : IEventBus
    {
        /// <summary>
        /// The event map/
        /// </summary>
        private static Dictionary<string, UnityEventBase> _eventMap = new Dictionary<string, UnityEventBase>();

        /// <summary>
        /// Send the event with <paramref name="eventName"/>.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        public void Send(string eventName)
        {
            if (!_eventMap.ContainsKey(eventName)) 
                return;

            if (_eventMap[eventName] is Callback handler)
            {
                handler.Invoke();
            }
            else
            {
                Debug.LogError(
                    $"Broadcasting message \"{eventName}\" but listeners have " +
                    $"a different signature than the broadcaster.");
            }
        }

        /// <summary>
        /// Send the event with <paramref name="eventName"/> which had 1 param.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="arg">Argument.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Send<T>(string eventName, T arg)
        {
            if (!_eventMap.ContainsKey(eventName)) 
                return;

            if (_eventMap[eventName] is Callback<T> handler)
            {
                handler.Invoke(arg);
            }
            else
            {
                Debug.LogError(
                    $"Broadcasting message \"{eventName}\" but listeners have " +
                    $"a different signature than the broadcaster.");
            }
        }

        /// <summary>
        /// Send the event with <paramref name="eventName"/> which had 2 params.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        public void Send<T, U>(string eventName, T arg1, U arg2)
        {
            if (!_eventMap.ContainsKey(eventName)) 
                return;
            
            if (_eventMap[eventName] is Callback<T, U> handler)
            {
                handler.Invoke(arg1, arg2);
            }
            else
            {
                Debug.LogError(
                    $"Broadcasting message \"{eventName}\" but listeners have a " +
                    $"different signature than the broadcaster.");
            }
        }

        /// <summary>
        /// Send the event with <paramref name="eventName"/> which had 3 params.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <param name="arg3">Arg3.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        /// <typeparam name="V">The 3rd type parameter.</typeparam>
        public void Send<T, U, V>(string eventName, T arg1, U arg2, V arg3)
        {
            if (!_eventMap.ContainsKey(eventName)) 
                return;

            if (_eventMap[eventName] is Callback<T, U, V> handler)
            {
                handler.Invoke(arg1, arg2, arg3);
            }
            else
            {
                Debug.LogError(
                    $"Broadcasting message \"{eventName}\" but listeners have a " +
                    $"different signature than the broadcaster.");
            }
        }

        /// <summary>
        /// Send the event with <paramref name="eventName"/> which had 4 params.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <param name="arg3">Arg3.</param>
        /// <param name="arg4">Arg4.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        /// <typeparam name="V">The 3rd type parameter.</typeparam>
        /// <typeparam name="K">The 4th type parameter.</typeparam>
        public void Send<T, U, V, K>(string eventName, T arg1, U arg2, V arg3, K arg4)
        {
            if (!_eventMap.ContainsKey(eventName)) 
                return;

            if (_eventMap[eventName] is Callback<T, U, V, K> handler)
            {
                handler.Invoke(arg1, arg2, arg3, arg4);
            }
            else
            {
                Debug.LogError(
                    $"Broadcasting message \"{eventName}\" but listeners have a " +
                    $"different signature than the broadcaster.");
            }
        }

        /// <summary>
        /// Subscribe with the <paramref name="eventName"/>
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        public void Subscribe(string eventName, UnityAction callback)
        {
            if (!_eventMap.ContainsKey(eventName))
            {
                _eventMap.Add(eventName, new Callback());
            }

            var handler = _eventMap[eventName] as Callback;
            handler?.AddListener(callback);
        }

        /// <summary>
        /// Subscribe with the <paramref name="eventName"/> which had 1 param
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Subscribe<T>(string eventName, UnityAction<T> callback)
        {
            if (!_eventMap.ContainsKey(eventName))
            {
                _eventMap.Add(eventName, new Callback<T>());
            }

            var handler = _eventMap[eventName] as Callback<T>;
            handler?.AddListener(callback);
        }

        /// <summary>
        /// Subscribe with the <paramref name="eventName"/> which had 2 params
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        public void Subscribe<T, U>(string eventName, UnityAction<T, U> callback)
        {
            if (!_eventMap.ContainsKey(eventName))
            {
                _eventMap.Add(eventName, new Callback<T, U>());
            }

            var handler = _eventMap[eventName] as UnityEvent<T, U>;
            handler?.AddListener(callback);
        }

        /// <summary>
        /// Subscribe with the <paramref name="eventName"/> which had 3 params
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        /// <typeparam name="V">The 3rd type parameter.</typeparam>
        public void Subscribe<T, U, V>(string eventName, UnityAction<T, U, V> callback)
        {
            if (!_eventMap.ContainsKey(eventName))
            {
                _eventMap.Add(eventName, new Callback<T, U, V>());
            }

            var handler = _eventMap[eventName] as UnityEvent<T, U, V>;
            handler?.AddListener(callback);
        }

        /// <summary>
        /// Subscribe with the <paramref name="eventName"/> which had 4 params
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        /// <typeparam name="V">The 3rd type parameter.</typeparam>
        /// <typeparam name="K">The 4th type parameter.</typeparam>
        public void Subscribe<T, U, V, K>(string eventName, UnityAction<T, U, V, K> callback)
        {
            if (!_eventMap.ContainsKey(eventName))
            {
                _eventMap.Add(eventName, new Callback<T, U, V, K>());
            }

            var handler = _eventMap[eventName] as UnityEvent<T, U, V, K>;
            handler?.AddListener(callback);
        }

        /// <summary>
        /// Unsubscribe with the <paramref name="eventName"/>
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        public void Unsubscribe(string eventName, UnityAction callback)
        {
            if (!_eventMap.ContainsKey(eventName)) 
                return;

            if (_eventMap[eventName] is Callback listener)
            {
                listener.RemoveListener(callback);
            }
        }

        /// <summary>
        /// Unsubscribe with the <paramref name="eventName"/> which had 1 param
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Unsubscribe<T>(string eventName, UnityAction<T> callback)
        {
            if (!_eventMap.ContainsKey(eventName)) 
                return;

            if (_eventMap[eventName] is Callback<T> listener)
            {
                listener.RemoveListener(callback);
            }
        }

        /// <summary>
        /// Unsubscribe with the <paramref name="eventName"/> which had 2 params
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        public void Unsubscribe<T, U>(string eventName, UnityAction<T, U> callback)
        {
            if (!_eventMap.ContainsKey(eventName)) 
                return;

            if (_eventMap[eventName] is Callback<T, U> listener)
            {
                listener.RemoveListener(callback);
            }
        }

        /// <summary>
        /// Unsubscribe with the <paramref name="eventName"/> which had 3 params
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        /// <typeparam name="V">The 3rd type parameter.</typeparam>
        public void Unsubscribe<T, U, V>(string eventName, UnityAction<T, U, V> callback)
        {
            if (!_eventMap.ContainsKey(eventName)) 
                return;

            if (_eventMap[eventName] is Callback<T, U, V> listener)
            {
                listener.RemoveListener(callback);
            }
        }

        /// <summary>
        /// Unsubscribe with the <paramref name="eventName"/> which had 4 params
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        /// <typeparam name="V">The 3rd type parameter.</typeparam>
        /// <typeparam name="K">The 4th type parameter.</typeparam>
        public void Unsubscribe<T, U, V, K>(string eventName, UnityAction<T, U, V, K> callback)
        {
            if (!_eventMap.ContainsKey(eventName)) 
                return;

            if (_eventMap[eventName] is Callback<T, U, V, K> listener)
            {
                listener.RemoveListener(callback);
            }
        }

        /// <summary>
        /// Unsubscribes all the callback with the <paramref name="eventName"/>.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        public void UnsubscribeAll(string eventName)
        {
            if (!_eventMap.ContainsKey(eventName)) 
                return;
            
            var listener = _eventMap[eventName];
            listener?.RemoveAllListeners();
        }

        /// <summary>
        /// Clear and unsubscribe all callback events.
        /// </summary>
        public void Clear()
        {
            _eventMap.Clear();
        }
    }
}
