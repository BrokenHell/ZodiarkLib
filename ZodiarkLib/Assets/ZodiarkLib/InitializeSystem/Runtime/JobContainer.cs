using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZodiarkLib.Initialize
{
    [CreateAssetMenu(menuName = "Systems/Initialize/Container",fileName = "LoaderContainer")]
    public sealed class JobContainer : ScriptableObject
    {
        #region Events & Delegates
        
        public delegate void LoadingCompleted();
        public delegate void LoadingProgress(float progress);

        public event LoadingCompleted OnInitializeComplete;
        public event LoadingProgress OnInitializeProgressing;

        #endregion

        #region Fields
        
        [SerializeField]
        private List<BaseScriptableJob> _initializers = new ();

        private MonoBehaviour _target;

        #endregion

        #region Properties
        
        /// <summary>
        /// Get list of scriptable initializer
        /// </summary>
        public IReadOnlyCollection<BaseScriptableJob> GetAllInitializers => _initializers.ToArray();  
        
        /// <summary>
        /// Check if this container is completed
        /// </summary>
        public bool HasCompleted
        {
            get
            {
                return _initializers.Aggregate(true, (current, item) => current & item.Completed);
            }
        }

        #endregion

        #region Unity Events
        
        private void OnEnable()
        {
            foreach (var item in _initializers)
            {
                item.Reset();
            }
        }

        public void Reset()
        {
            foreach (var item in _initializers)
            {
                item.Reset();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Register new scriptable job
        /// </summary>
        /// <param name="initializer"></param>
        public void Register(BaseScriptableJob initializer)
        {
            _initializers.Add(initializer);
        }

        /// <summary>
        /// Unregister scriptable job
        /// </summary>
        /// <param name="initializer"></param>
        public void UnRegister(BaseScriptableJob initializer)
        {
            _initializers.Remove(initializer);
        }

        /// <summary>
        /// Get current process
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetProcess<T>()
            where T : BaseScriptableJob
        {
            var type = typeof(T);
            foreach (var initalize in _initializers)
            {
                var itemType = initalize.GetType();
                if (itemType == type || type.IsAssignableFrom(itemType))
                {
                    return (T)initalize;
                }
            }

            return null;
        }

        /// <summary>
        /// Start to run this job container
        /// </summary>
        /// <param name="target"></param>
        public void RunInitialize(MonoBehaviour target)
        {
            foreach (var item in _initializers)
            {
                item.Reset();
            }

            foreach (var item in _initializers)
            {
                item.Setup();
            }

            target.StartCoroutine(OnUpdate());
            target.StartCoroutine(OnCalculateProgressUpdate());
            _target = target;
        }
        
        /// <summary>
        /// Update dependency tree for all the job in this container
        /// </summary>
        public void UpdateDependencies()
        {
            foreach (var item in _initializers)
            {
                var previousList = new List<IJob>(item.Dependencies);
                item.ClearDependencies();
                var dependTypes = item.DependencyTypes;

                foreach (var type in dependTypes)
                {
                    foreach (var depend in _initializers)
                    {
                        if (depend == item)
                            continue;
                        if (type.IsInstanceOfType(depend))
                        {
                            item.AddDependency(depend);
                            previousList.Remove(depend);
                        }
                    }
                }

                item.AddDependencies(previousList.ToArray());
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(item);
#endif
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator OnUpdate()
        {
            var completeAll = false;
            while (!completeAll)
            {
                completeAll = true;
                foreach (var item in _initializers)
                {
                    if (!item.Completed && !item.IsRunning && item.IsDependenciesCompleted())
                    {
                        if (_target != null)
                        {
                            _target.StartCoroutine(item.Process());   
                        }
                    }
                    completeAll &= item.Completed;
                }

                if (completeAll)
                {
                    OnInitializeComplete?.Invoke();
                }

                yield return null;
            }
        }

        private IEnumerator OnCalculateProgressUpdate()
        {
            var completeAll = false;
            var totalProgress = _initializers.Sum(item => item.ProgressWeight);

            while (!completeAll)
            {
                completeAll = true;
                var currentProgress = 0f;
                foreach (var item in _initializers)
                {
                    currentProgress += item.CurrentProgressPercentage * item.ProgressWeight;
                    completeAll &= item.Completed;
                }

                OnInitializeProgressing?.Invoke(currentProgress / totalProgress);

                yield return null;
            }
        }

        #endregion
    }   
}