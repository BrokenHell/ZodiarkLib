namespace ZodiarkLib.Sound
{
    /// <summary>
    /// Interface for sound manager
    /// </summary>
    public interface ISoundManager
    {
        /// <summary>
        /// Reference to sound pool
        /// </summary>
        ISoundPool SoundPool { get; set; }

        /// <summary>
        /// Reference to sound database
        /// </summary>
        SoundDatabase SoundDatabase { get; set; }

        /// <summary>
        /// Check if the system is mute or not
        /// </summary>
        bool IsMute { get; set; }

        /// <summary>
        /// Check if the background is mute or not
        /// </summary>
        bool IsMuteBackground { get; set; }

        /// <summary>
        /// Check if the sound effect is mute or not
        /// </summary>
        bool IsMuteSound { get; set; }

        /// <summary>
        /// Set or get the volume value
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// Initialize the manager
        /// </summary>
        void Initialize();

        /// <summary>
        /// Play sound with id
        /// </summary>
        /// <param name="soundId"></param>
        void PlayOnce(string soundId);

        /// <summary>
        /// Play loop sound with id
        /// </summary>
        /// <param name="soundId"></param>
        void PlayLoop(string soundId);

        /// <summary>
        /// Transition between sounds
        /// </summary>
        /// <param name="fromSoundId"></param>
        /// <param name="toSoundId"></param>
        /// <param name="duration"></param>
        void Transition(string fromSoundId, string toSoundId, float duration = 0.25f);

        /// <summary>
        /// Pause sound with id
        /// </summary>
        /// <param name="soundId"></param>
        void Pause(string soundId);
        /// <summary>
        /// Resume sound with id
        /// </summary>
        /// <param name="soundId"></param>
        void Resume(string soundId);
        /// <summary>
        /// Stop sound with id
        /// </summary>
        /// <param name="soundId"></param>
        void Stop(string soundId);

        /// <summary>
        /// Stop all sound sources
        /// </summary>
        void StopAll();
        /// <summary>
        /// Pause all sound sources
        /// </summary>
        void PauseAll();
        /// <summary>
        /// Resume all paused sound sources
        /// </summary>
        void ResumeAll();

        /// <summary>
        /// Get current playing sound id
        /// </summary>
        /// <param name="soundId"></param>
        /// <returns></returns>
        ISoundController GetPlaying(string soundId);
    }
}