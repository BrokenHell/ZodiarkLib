namespace ZodiarkLib.Core
{
    /// <summary>
    /// Support method for create pure C# singleton class
    /// </summary>
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance = new T();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                if (!_instance.HasInit)
                {
                    _instance.Init();
                    _instance.HasInit = true;
                }
                return _instance;
            }
        }

        public virtual void DestroyInstance()
        {
            _instance = null;
        }

        protected bool HasInit { get; set; } = false;

        /// <summary>
        /// This is working as constructor of this instant, so we need to override it.
        /// </summary>
        protected virtual void Init()
        {
        }
    }
}