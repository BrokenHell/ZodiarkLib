using UnityEngine;

namespace ZodiarkLib.Sound
{
    /// <summary>
    /// Sound Controller to wrap on Unity Audio
    /// </summary>
    public interface ISoundController
    {
        /// <summary>
        /// Reference to audio source
        /// </summary>
        AudioSource AudioSource { get; set; }

        /// <summary>
        /// Reference to sound data
        /// </summary>
        SoundData SoundData { get; }

        /// <summary>
        /// Sound Id this controller is handle
        /// </summary>
        string SoundId { get; }

        /// <summary>
        /// Check if this controller is playing sound
        /// </summary>
        bool IsPlaying { get; }
        /// <summary>
        /// Check if this controller is paused
        /// </summary>
        bool IsPaused { get; }
        /// <summary>
        /// Check if this controller is stopped
        /// </summary>
        bool IsStopped { get; }
        /// <summary>
        /// Check if this controller is loop
        /// </summary>
        bool IsLooped { get; set; }

        /// <summary>
        /// Initialize controller with sound data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="masterVolume"></param>
        void Initialize(SoundData data, float masterVolume);

        /// <summary>
        /// Play the sound
        /// </summary>
        /// <param name="isLoop"></param>
        /// <param name="masterVolume"></param>
        void Play(bool isLoop, float masterVolume);
        /// <summary>
        /// Pause this controller
        /// </summary>
        void Pause();
        /// <summary>
        /// Resume this controller
        /// </summary>
        void Resume();
        /// <summary>
        /// Stop this controller
        /// </summary>
        void Stop();
        /// <summary>
        /// Set volume for this controller
        /// </summary>
        /// <param name="volume"></param>
        void SetVolume(float volume);
        /// <summary>
        /// Set mute status for this controller
        /// </summary>
        /// <param name="isMute"></param>
        void SetMute(bool isMute);
        /// <summary>
        /// Free up this sound from memory
        /// </summary>
        void Free();
        /// <summary>
        /// Self clean up
        /// </summary>
        void Cleanup();

        /// <summary>
        /// Transition fade into desire volume
        /// </summary>
        /// <param name="toVolume"></param>
        /// <param name="duration"></param>
        /// <param name="callback"></param>
        void Fade(float toVolume, float duration, System.Action callback = null);
    }
}