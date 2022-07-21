using UnityEngine.Events;

namespace ZodiarkLib.Event
{
    internal class Callback : UnityEvent { }
    internal class Callback<T> : UnityEvent<T> { }
    internal class Callback<T, U> : UnityEvent<T, U> { }
    internal class Callback<T, U, V> : UnityEvent<T, U, V> { }
    internal class Callback<T, U, V, K> : UnityEvent<T, U, V, K> { }

    /// <summary>
    /// Interface for event messenger which is use for broadcasting event
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Subscribe with the <paramref name="eventName"/>
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        void Subscribe(string eventName, UnityAction callback);

        /// <summary>
        /// Subscribe with the <paramref name="eventName"/> which had 1 param
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        void Subscribe<T>(string eventName, UnityAction<T> callback);

        /// <summary>
        /// Subscribe with the <paramref name="eventName"/> which had 2 params
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        void Subscribe<T, U>(string eventName, UnityAction<T, U> callback);

        /// <summary>
        /// Subscribe with the <paramref name="eventName"/> which had 3 params
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        /// <typeparam name="V">The 3rd type parameter.</typeparam>
        void Subscribe<T, U, V>(string eventName, UnityAction<T, U, V> callback);

        /// <summary>
        /// Subscribe with the <paramref name="eventName"/> which had 4 params
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        /// <typeparam name="V">The 3rd type parameter.</typeparam>
        /// <typeparam name="K">The 4th type parameter.</typeparam>
        void Subscribe<T, U, V, K>(string eventName, UnityAction<T, U, V, K> callback);

        /// <summary>
        /// Unsubscribe with the <paramref name="eventName"/>
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        void Unsubscribe(string eventName, UnityAction callback);

        /// <summary>
        /// Unsubscribe with the <paramref name="eventName"/> which had 1 param
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        void Unsubscribe<T>(string eventName, UnityAction<T> callback);

        /// <summary>
        /// Unsubscribe with the <paramref name="eventName"/> which had 2 params
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        void Unsubscribe<T, U>(string eventName, UnityAction<T, U> callback);

        /// <summary>
        /// Unsubscribe with the <paramref name="eventName"/> which had 3 params
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        /// <typeparam name="V">The 3rd type parameter.</typeparam>
        void Unsubscribe<T, U, V>(string eventName, UnityAction<T, U, V> callback);

        /// <summary>
        /// Unsubscribe with the <paramref name="eventName"/> which had 4 params
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        /// <typeparam name="V">The 3rd type parameter.</typeparam>
        /// <typeparam name="K">The 4th type parameter.</typeparam>
        void Unsubscribe<T, U, V, K>(string eventName, UnityAction<T, U, V, K> callback);

        /// <summary>
        /// Unsubscribes all the callback with the <paramref name="eventName"/>.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        void UnsubscribeAll(string eventName);

        /// <summary>
        /// Clear and unsubscribe all callback events.
        /// </summary>
        void Clear();

        /// <summary>
        /// Send the event with <paramref name="eventName"/>.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        void Send(string eventName);

        /// <summary>
        /// Send the event with <paramref name="eventName"/> which had 1 param.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="arg">Argument.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        void Send<T>(string eventName, T arg);

        /// <summary>
        /// Send the event with <paramref name="eventName"/> which had 2 params.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="U">The 2nd type parameter.</typeparam>
        void Send<T, U>(string eventName, T arg1, U arg2);

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
        void Send<T, U, V>(string eventName, T arg1, U arg2, V arg3);

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
        void Send<T, U, V, K>(string eventName, T arg1, U arg2, V arg3, K arg4);
    }
}
