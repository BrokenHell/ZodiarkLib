using ZodiarkLib.Data;

namespace ZodiarkLib.Sound.Extensions
{
    [System.Serializable]
    public class SoundUserData : IGlobalData
    {
        public bool isMute = false;
        public float volume = 1f;
        public bool isMuteBg = false;
        public bool isMuteFx = false;
    }
}