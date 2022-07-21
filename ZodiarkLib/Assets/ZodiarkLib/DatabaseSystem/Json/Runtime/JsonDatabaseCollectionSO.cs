using UnityEngine;

namespace ZodiarkLib.Database
{
    [CreateAssetMenu(menuName = "Systems/Database/Json/Collection",fileName = "JsonDatabaseCollection")]
    public class JsonDatabaseCollectionSO : ScriptableObject
    {
        [SerializeField] private TextAsset[] _jsons;
        public TextAsset[] Jsons => _jsons;
    }
}