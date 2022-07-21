using System.Collections.Generic;

namespace ZodiarkLib.Sound
{
    /// <summary>
    /// Sound pool
    /// </summary>
    public interface ISoundPool
    {
        /// <summary>
        /// Initialize sound pool
        /// </summary>
        /// <param name="provideInstance"></param>
        /// <param name="maxSoundSource"></param>
        void Initialize(System.Func<ISoundController> provideInstance, int maxSoundSource = 5);

        /// <summary>
        /// Get free sound controller with sound id
        /// </summary>
        /// <param name="spawnIfNotFound"></param>
        /// <returns></returns>
        ISoundController GetFreeSoundController(bool spawnIfNotFound = true);

        /// <summary>
        /// Set the input sound controller to playing state
        /// </summary>
        /// <param name="controller"></param>
        void SetPlayingSoundController(ISoundController controller);

        /// <summary>
        /// Get playing sound controller with sound id
        /// </summary>
        /// <param name="soundId"></param>
        /// <returns></returns>
        ISoundController GetPlayingSoundController(string soundId);

        /// <summary>
        /// Get all playing sound controller with sound id
        /// </summary>
        /// <param name="soundId"></param>
        /// <returns></returns>
        List<ISoundController> GetPlayingSoundControllers(string soundId);

        /// <summary>
        /// Get all playing sound controllers
        /// </summary>
        /// <returns></returns>
        List<ISoundController> GetPlayingSoundControllers();

        /// <summary>
        /// Cleanup sound pool
        /// </summary>
        void CleanUp();

        /// <summary>
        /// Update loop of sound pool
        /// </summary>
        void Update();
    }
}