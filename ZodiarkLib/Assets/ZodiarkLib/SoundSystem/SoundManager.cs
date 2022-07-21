using UnityEngine;

namespace ZodiarkLib.Sound
{
    public class SoundManager : MonoBehaviour, ISoundManager
    {
        #region Fields
        
        private bool _isMute = false;
        private float _volume = 1f;
        private bool _isMuteBackground = false;
        private bool _isMuteSoundfx = false;

        #endregion

        #region Properties

        public ISoundPool SoundPool { get; set; }
        public SoundDatabase SoundDatabase { get; set; }

        public bool IsMute
        {
            get => _isMute;
            set
            {
                _isMute = value;
                if (SoundPool != null)
                {
                    foreach (var item in SoundPool.GetPlayingSoundControllers())
                    {
                        if (item.IsLooped)
                        {
                            item.SetMute(_isMute || _isMuteBackground);
                        }
                        else
                        {
                            item.SetMute(_isMute || _isMuteSoundfx);
                        }
                    }
                }
            }
        }

        public bool IsMuteBackground
        {
            get => _isMuteBackground;
            set
            {
                _isMuteBackground = value;
                if (SoundPool != null)
                {
                    foreach (var item in SoundPool.GetPlayingSoundControllers())
                    {
                        if (item.IsLooped) // Looped sound should be background music
                        {
                            item.SetMute(_isMute || _isMuteBackground);
                        }
                    }
                }
            }
        }

        public bool IsMuteSound
        {
            get => _isMuteSoundfx;
            set
            {
                _isMuteSoundfx = value;
                if (SoundPool != null)
                {
                    foreach (var item in SoundPool.GetPlayingSoundControllers())
                    {
                        if (!item.IsLooped) // non-Looped sound should be sound fx
                        {
                            item.SetMute(_isMute || _isMuteSoundfx);
                        }
                    }
                }
            }
        }

        public float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                if (SoundPool != null)
                {
                    foreach (var item in SoundPool.GetPlayingSoundControllers())
                    {
                        item.SetVolume(value);
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        public ISoundController GetPlaying(string soundId)
        {
            return SoundPool.GetPlayingSoundController(soundId);
        }

        public void Initialize()
        {
            if (SoundPool != null)
            {
                SoundPool.Initialize(() =>
                {
                    var soundGO = new GameObject("--SoundController--");
                    soundGO.transform.SetParent(this.transform);
                    soundGO.transform.localPosition = Vector3.zero;
                    return soundGO.AddComponent<SoundController>();
                });
            }
        }

        public void Pause(string soundId)
        {
            var controller = SoundPool.GetPlayingSoundController(soundId);
            if (controller != null && !controller.IsPaused)
            {
                controller.Pause();
            }
        }

        public void PauseAll()
        {
            foreach (var item in SoundPool.GetPlayingSoundControllers())
            {
                if (!item.IsPaused)
                {
                    item.Pause();
                }
            }
        }

        public void Transition(string fromSoundId, string toSoundId, float duration = 0.25f)
        {
            var fromSound = GetPlaying(fromSoundId);
            if (fromSound != null)
            {
                var controller = SoundPool.GetFreeSoundController();
                if (controller != null)
                {
                    var soundData = SoundDatabase.GetSound(toSoundId);
                    if (!soundData.enable)
                        return;
                    SoundPool.SetPlayingSoundController(controller);

                    controller.Initialize(soundData, 0);
                    controller.Play(true, 0f);
                    controller.SetMute(_isMute || _isMuteBackground);


                    controller.Fade(_volume, duration);
                    fromSound.Fade(0, duration, () =>
                    {
                        fromSound.Stop();
                    });
                }
            }
            else
            {
                PlayLoop(toSoundId);
            }
        }

        public void PlayLoop(string soundId)
        {
            var controller = SoundPool.GetFreeSoundController();
            if (controller != null)
            {
                var soundData = SoundDatabase.GetSound(soundId);
                if (!soundData.enable)
                    return;
                SoundPool.SetPlayingSoundController(controller);

                controller.Initialize(soundData, _volume);
                controller.Play(true, _volume);
                controller.SetVolume(_volume);
                controller.SetMute(_isMute || _isMuteBackground);

            }
        }

        public void PlayOnce(string soundId)
        {
            var controller = SoundPool.GetFreeSoundController();
            if (controller != null)
            {
                var soundData = SoundDatabase.GetSound(soundId);
                if (!soundData.enable)
                    return;
                SoundPool.SetPlayingSoundController(controller);

                controller.Initialize(soundData, _volume);
                controller.Play(false, _volume);
                controller.SetVolume(_volume);
                controller.SetMute(_isMute || _isMuteSoundfx);
            }
        }

        public void Resume(string soundId)
        {
            var controller = GetPlaying(soundId);
            if (controller != null && controller.IsPaused)
            {
                controller.Resume();
            }
        }

        public void ResumeAll()
        {
            foreach (var item in SoundPool.GetPlayingSoundControllers())
            {
                if (item.IsPaused)
                {
                    item.Resume();
                }
            }
        }

        public void Stop(string soundId)
        {
            var controller = GetPlaying(soundId);
            if (controller != null)
            {
                controller.Stop();
            }
        }

        public void StopAll()
        {
            foreach (var item in SoundPool.GetPlayingSoundControllers())
            {
                item.Stop();
            }
        }

        #endregion

        #region Private Methods
        
        private void Update()
        {
            SoundPool?.Update();
        }

        #endregion
    }
}