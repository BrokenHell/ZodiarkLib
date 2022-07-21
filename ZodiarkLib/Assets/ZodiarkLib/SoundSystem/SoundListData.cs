using System.Collections.Generic;
using UnityEngine;

namespace ZodiarkLib.Sound
{
    [System.Serializable]
    public enum SoundListType
    {
        Randomize,
        Waterfall,
    }

    public class SoundListData : SoundData
    {
        public SoundListType listType;
        public List<AudioClip> _playables = new List<AudioClip>();

        [System.NonSerialized]
        private AudioClip _playingAudio;

        public override AudioClip AudioClip
        {
            get
            {
                if (_playables.Count == 0)
                {
                    Debug.LogError("No Audio clip found!!!");
                    return null;
                }

                if (_playables.Count == 1)
                {
                    _playingAudio = _playables[0];
                }
                else
                {
                    if (listType == SoundListType.Randomize)
                    {
                        var list = new List<AudioClip>(_playables);
                        if (_playingAudio != null)
                        {
                            list.Remove(_playingAudio);
                        }
                        var idx = Random.Range(0, list.Count);
                        _playingAudio = _playables[idx];
                    }
                    else if (listType == SoundListType.Waterfall)
                    {
                        if (_playingAudio == null)
                        {
                            _playingAudio = _playables[0];
                        }
                        else
                        {
                            var idx = _playables.IndexOf(_playingAudio);
                            idx++;
                            if (idx >= _playables.Count) idx = 0;
                            _playingAudio = _playables[idx];
                        }
                    }
                }

                return _playingAudio;
            }
        }
    }
}