#if INITIALIZE_SYSTEM_ENABLE
using System.Collections;
using UnityEngine;
using ZodiarkLib.Initialize;
using ZodiarkLib.UI;

namespace PlayMobil.UI
{
    [CreateAssetMenu(menuName = "Systems/UI/UI Initialize",fileName = "UIInitialize")]
    public class UISystemInitialize : BaseScriptableJob
    {
        [SerializeField] private UIManager _uiManagerPrefab;
        [Header("Databases Config")]
        [SerializeField] private CanvasDatabase _canvasDatabase;
        [SerializeField] private DialogDatabase _dialogDatabase;
        
        protected override IEnumerator InternalProcess()
        {
            var uiManager = GameObject.Instantiate(_uiManagerPrefab);
            yield return uiManager.Initialize(_canvasDatabase, _dialogDatabase);
        }
    }   
}
#endif