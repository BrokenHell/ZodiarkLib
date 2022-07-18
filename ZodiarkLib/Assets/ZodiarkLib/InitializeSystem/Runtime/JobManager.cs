using UnityEngine;

namespace ZodiarkLib.Initialize
{
    public sealed class JobManager : MonoBehaviour
    {
        #region Event & Delegates
        
        public delegate void LoadingCompleted(string hubId);
        public delegate void LoadingProgress(string hubId, float progress);
        
        public event LoadingCompleted OnCompleted;
        public event LoadingProgress OnProgress;

        #endregion

        #region Fields
        
        [SerializeField]
        private JobContainer _loader;
        [SerializeField]
        private bool _runAtStartup = true;
        
        private bool _hasRunningProcess = false;

        #endregion

        #region Unity Events

        private void Start()
        {
            if (!_runAtStartup)
                return;

            if (_loader == null) return;
            _loader.Reset();
            Run();
        }

        #endregion

        #region Public Methods

        public void Run()
        {
            if (_hasRunningProcess) return;
            
            _hasRunningProcess = true;
            _loader.OnInitializeComplete += HandleInitializeComplete;
            _loader.OnInitializeProgressing += HandleInitializeProgressing;
            _loader.RunInitialize(this);
        }

        #endregion

        #region Private Methods

        void HandleInitializeComplete()
        {
            if (_loader != null)
            {
                _loader.OnInitializeComplete -= HandleInitializeComplete;
                _loader.OnInitializeProgressing -= HandleInitializeProgressing;

                if (OnCompleted != null)
                {
                    OnCompleted.Invoke(_loader.name);
                }
            }

            _hasRunningProcess = false;
        }

        void HandleInitializeProgressing(float progress)
        {
            if (_loader != null && OnProgress != null)
            {
                OnProgress.Invoke(_loader.name, progress);
            }
        }

        #endregion
    }
}