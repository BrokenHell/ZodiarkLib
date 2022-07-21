using UnityEngine;

namespace ZodiarkLib.Database.Sheet
{
    [CreateAssetMenu(menuName = "Systems/Database/Sheet/Collection",fileName = "SheetCollection")]
    public sealed class SheetCollectionSO : ScriptableObject
    {
        [SerializeField] private TextAsset[] _sheets;
        public TextAsset[] Sheets => _sheets;
    }   
}