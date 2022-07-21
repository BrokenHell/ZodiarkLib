using System.Collections.Generic;
using System.Linq;

namespace ZodiarkLib.Sound
{
    public class SoundPool : ISoundPool
    {
        #region [ Fields ]

        private List<ISoundController> _playingSoundControllers = new List<ISoundController>();
        private List<ISoundController> _freeSoundControllers = new List<ISoundController>();

        private System.Func<ISoundController> _provideInstance;

        #endregion

        #region Public Methods

        public void CleanUp()
        {
            foreach (var item in _freeSoundControllers)
            {
                item.Free();
            }

            foreach (var item in _playingSoundControllers)
            {
                item.Free();
            }

            _freeSoundControllers.Clear();
            _playingSoundControllers.Clear();
        }

        public ISoundController GetFreeSoundController(bool spawnIfNotFound = true)
        {
            if (_freeSoundControllers.Count > 0)
            {
                return _freeSoundControllers[0];
            }

            if (!spawnIfNotFound) 
                return null;
            
            _freeSoundControllers.Add(_provideInstance.Invoke());
            return _freeSoundControllers[0];

        }

        public ISoundController GetPlayingSoundController(string soundId)
        {
            return _playingSoundControllers.Find(x => x.SoundId == soundId);
        }

        public List<ISoundController> GetPlayingSoundControllers(string soundId)
        {
            return _playingSoundControllers.FindAll(x => x.SoundId == soundId);
        }

        public List<ISoundController> GetPlayingSoundControllers()
        {
            return _playingSoundControllers;
        }

        public void Initialize(System.Func<ISoundController> provideInstance, int initialSpawnCount = 5)
        {
            this._provideInstance = provideInstance;

            for (int i = 0; i < initialSpawnCount; ++i)
            {
                _freeSoundControllers.Add(provideInstance.Invoke());
            }
        }

        public void SetPlayingSoundController(ISoundController controller)
        {
            _freeSoundControllers.Remove(controller);
            _playingSoundControllers.Add(controller);
            controller.AudioSource.gameObject.SetActive(true);
        }

        public void Update()
        {
            var soundControllers = _playingSoundControllers.Where(item => 
                                                                                    item.IsStopped)
                                                                                .ToList();
            
            foreach (var item in soundControllers)
            {
                _playingSoundControllers.Remove(item);
                _freeSoundControllers.Add(item);
                item.AudioSource.gameObject.SetActive(false);
            }
            soundControllers.Clear();
        }

        #endregion
    }
}