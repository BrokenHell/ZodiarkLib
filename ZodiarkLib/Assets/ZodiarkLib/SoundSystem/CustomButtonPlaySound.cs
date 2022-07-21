using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZodiarkLib.Sound
{
    public class CustomButtonPlaySound : MonoBehaviour
    {
// #if UNITY_EDITOR
//         [ValueDropdown("Availables")]
// #endif
//         [SerializeField]
//         private string _soundId = default;
//
//
// #if UNITY_EDITOR
//         [ValueDropdown("Availables")]
// #endif
//         [SerializeField]
//         private string _fromSoundId = default;
//
//         [SerializeField]
//         private bool _isLoop = default;
//
//         [SerializeField]
//         private bool _playOnWake = default;
//
//         [SerializeField]
//         [Tooltip("Only availables on Looped sound")]
//         [EnableIf("_isLoop")]
//         private bool _crossFade = default;
//
//         [SerializeField]
//         [Tooltip("Only availables on Looped sound")]
//         [EnableIf("_isLoop")]
//         private float _crossFadeDuration = 0.25f;
//
//         private Button _button;
//
//         private void Awake()
//         {
//             _button = GetComponent<Button>();
//             if (_button != null)
//             {
//                 _button.onClick.AddListener(Play);
//             }
//
//             if (_playOnWake)
//             {
//                 this.Play();
//             }
//         }
//
//         private void OnDestroy()
//         {
//             if (_button != null)
//             {
//                 _button.onClick.RemoveListener(Play);
//             }
//         }
//
//         /// <summary>
//         /// Play the sound
//         /// </summary>
//         public void Play()
//         {
//             var soundMgr = ServiceLocator.Get<ISoundManager>();
//             if (soundMgr != null && soundMgr.GetPlaying(_soundId) == null)
//             {
//                 if (_isLoop)
//                 {
//                     if (_crossFade)
//                     {
//                         soundMgr.Transition(_fromSoundId, _soundId, _crossFadeDuration);
//                     }
//                     else
//                     {
//                         soundMgr.PlayLoop(_soundId);
//                     }
//                 }
//                 else
//                 {
//                     soundMgr.PlayOnce(_soundId);
//                 }
//             }
//         }
//
// #if UNITY_EDITOR
//         private IEnumerable<string> Availables()
//         {
//             var soundDatabase = AssetsHelper.GetActiveTable<SoundDatabase>();
//
//             return soundDatabase.Records.Select(x => x.Key).ToList();
//         }
// #endif
    }
}