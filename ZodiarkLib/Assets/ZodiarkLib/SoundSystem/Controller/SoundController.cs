using DG.Tweening;
using UnityEngine;

namespace ZodiarkLib.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundController : MonoBehaviour, ISoundController
    {
        #region [ Properties ]

        public AudioSource AudioSource { get; set; }

        public SoundData SoundData { get; private set; }

        public string SoundId => SoundData != null ? SoundData.id : string.Empty;

        public bool IsPlaying { get; private set; }

        public bool IsPaused { get; private set; }

        public bool IsStopped
        {
            get
            {
                if (AudioSource != null && !AudioSource.isPlaying)
                {
                    IsPlaying = false;
                    IsPaused = false;
                }

                return !IsPlaying && !IsPaused;
            }
        }

        public bool IsLooped { get; set; }

        #endregion

        #region [ Unity Events ]

        private void Awake()
        {
            if (AudioSource == null)
            {
                AudioSource = GetComponent<AudioSource>();
                AudioSource.playOnAwake = false;
            }
        }

        #endregion

        #region [ Public Methods ]
        public void Initialize(SoundData data, float masterVolume)
        {
            SoundData = data;
            if (AudioSource != null)
            {
                AudioSource.clip = data.AudioClip;
                AudioSource.name = data.id;
                AudioSource.playOnAwake = false;
                AudioSource.volume = masterVolume * GetVolumeMultiply();
            }
        }

        public void Pause()
        {
            if (AudioSource != null && AudioSource.isPlaying && IsPlaying && !IsPaused)
            {
                AudioSource.Pause();
                IsPaused = true;
            }
        }

        public void Play(bool isLoop, float masterVolume)
        {
            if (AudioSource != null)
            {
                AudioSource.clip = SoundData.AudioClip;
                AudioSource.loop = isLoop;
                AudioSource.name = SoundData.id;
                AudioSource.playOnAwake = false;
                AudioSource.volume = masterVolume * GetVolumeMultiply();

                IsLooped = isLoop;
                IsPlaying = true;
                IsPaused = false;

                AudioSource.Play();
            }
        }

        public void Resume()
        {
            if (AudioSource != null && IsPaused && IsPlaying)
            {
                AudioSource.UnPause();
                IsPaused = false;
            }
        }

        public void SetMute(bool isMute)
        {
            if (AudioSource != null)
            {
                AudioSource.mute = isMute;
            }
        }

        public void SetVolume(float volume)
        {
            if (AudioSource != null)
            {
                AudioSource.volume = volume * GetVolumeMultiply();
            }
        }

        public void Stop()
        {
            if (AudioSource != null && IsPlaying)
            {
                AudioSource.Stop();
                IsPlaying = false;
                IsPaused = false;
            }
        }

        public void Free()
        {
            Destroy(this.gameObject);
        }

        public void Cleanup()
        {
            SoundData = null;
            if (AudioSource != null)
            {
                AudioSource.clip = null;
            }
            IsPlaying = false;
            IsPaused = false;
        }

        public void Fade(float toVolume, float duration, System.Action callback = null)
        {
            AudioSource.DOFade(toVolume * GetVolumeMultiply(), duration).OnComplete(() => callback?.Invoke());
        }

        #endregion

        #region [ Private Fields ]

        private float GetVolumeMultiply()
        {
            return SoundData?.customVolume ?? 1f;
        }

        #endregion
    }
}