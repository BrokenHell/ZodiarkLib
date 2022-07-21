using System.Linq;
using UnityEngine;

namespace ZodiarkLib.Sound
{
    [CreateAssetMenu(menuName = "Systems/Sound/Database", fileName = "SoundDatabase")]
    public sealed class SoundDatabase : ScriptableObject
    {
        [SerializeField] private SoundData[] _soundDatas;

        public SoundData[] SoundDatas => _soundDatas;

        public SoundData GetSound(string id)
        {
            return _soundDatas.FirstOrDefault(x => x.id == id);
        }
    }
}