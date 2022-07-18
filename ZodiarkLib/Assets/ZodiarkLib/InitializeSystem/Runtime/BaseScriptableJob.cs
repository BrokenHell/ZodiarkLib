using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZodiarkLib.Initialize
{
    public abstract class BaseScriptableJob : ScriptableObject , IJob
    {
        #region Fields

        [SerializeField] 
        private List<BaseScriptableJob> _dependencies = new();
        
        private float _currentProgress = 0f;

        #endregion
        
        #region Properties

        [field: SerializeField]
        public string Id { get; set; }

        public IReadOnlyCollection<IJob> Dependencies => _dependencies.Select(x => (IJob) x).ToList();
        [field: SerializeField] 
        public float ProgressWeight { get; set; } = 1f;

        public float CurrentProgressPercentage
        {
            get => _currentProgress;
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException("CurrentProgressPercentage",
                        "Progress value need to be normalized float which mean from 0->1 only!");
                }

                _currentProgress = value;
            }
        }

        public virtual Type[] DependencyTypes => System.Type.EmptyTypes;
        public bool Completed { get; set; }
        public bool IsRunning { get; set; }

        #endregion

        #region Events & Delegates
        
        public event EventHandler<IJob> OnJobCompleted;
        public event EventHandler<IJob> OnJobProgressing;

        #endregion

        #region Public Methods

        public virtual void Setup()
        {
        }

        public virtual IEnumerator Process()
        {
            IsRunning = true;
            yield return InternalProcess();
            CurrentProgressPercentage = 1f;
            Finish();
        }

        public void Reset()
        {
            Completed = false;
            IsRunning = false;
            CurrentProgressPercentage = 0f;
        }

        public bool IsDependenciesCompleted()
        {
            return Dependencies.All(job => job.Completed);
        }

        public void AddDependency(IJob job)
        {
            _dependencies.Add(job as BaseScriptableJob);
        }

        public void AddDependencies(IJob[] jobs)
        {
            _dependencies.AddRange(jobs.Select(x => x as BaseScriptableJob));
        }

        public void ClearDependencies()
        {
            _dependencies.Clear();
        }

        #endregion

        #region Protected Methods

        protected abstract IEnumerator InternalProcess();

        protected virtual void Finish()
        {
            Completed = true;
        }

        #endregion
    }
}