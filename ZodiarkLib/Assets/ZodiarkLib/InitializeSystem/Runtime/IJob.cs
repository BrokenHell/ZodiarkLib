using System;
using System.Collections;
using System.Collections.Generic;

namespace ZodiarkLib.Initialize
{
    /// <summary>
    /// Base class for initialization job
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// Event fires when job is completed
        /// </summary>
        event EventHandler<IJob> OnJobCompleted;
        /// <summary>
        /// Event fires when job is progressing
        /// </summary>
        event EventHandler<IJob> OnJobProgressing;

        /// <summary>
        /// Unique Identifier
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Get list of dependencies
        /// </summary>
        IReadOnlyCollection<IJob> Dependencies { get; }

        /// <summary>
        /// Check if job have been completed or not
        /// </summary>
        bool Completed { get; set; }

        /// <summary>
        /// Check if job is running or not
        /// </summary>
        bool IsRunning { get; set; }

        /// <summary>
        /// Provide how long this job need to take to complete.
        /// </summary>
        float ProgressWeight { get; set; }

        /// <summary>
        /// Set or gets current progress percent ( from 0 ( not done ) - 1 ( done ) ) 
        /// </summary>
        float CurrentProgressPercentage { get; set; }

        /// <summary>
        /// Get list of dependency types this job is depend on
        /// </summary>
        System.Type[] DependencyTypes { get; }

        /// <summary>
        /// Use to setup initial values for the job
        /// </summary>
        void Setup();

        /// <summary>
        /// The job process
        /// </summary>
        /// <returns></returns>
        IEnumerator Process();

        /// <summary>
        /// Sometimes we need to reset our values to run it again.
        /// </summary>
        void Reset();

        /// <summary>
        /// Check if all the dependencies have been completed
        /// </summary>
        /// <returns></returns>
        bool IsDependenciesCompleted();

        /// <summary>
        /// Add new dependency
        /// </summary>
        /// <param name="job"></param>
        void AddDependency(IJob job);

        /// <summary>
        /// Add new dependencies
        /// </summary>
        /// <param name="jobs"></param>
        void AddDependencies(IJob[] jobs);

        /// <summary>
        /// Clear dependency tree
        /// </summary>
        void ClearDependencies();
    }   
}