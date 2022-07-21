using UnityEngine;

namespace ZodiarkLib.Sound
{
    [System.Serializable]
    public class SoundData
    {
        public string id;
        public bool enable = true;
        public AudioClip audioClip;
        public string assetGroup;
        public string assetPath;
        public float customVolume = 1f;

        public virtual AudioClip AudioClip => audioClip;
    }
}