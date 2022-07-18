using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using ZodiarkLib.Core;

namespace ZodiarkLib.UI
{
    public sealed class UIManager : MonoSingleton<UIManager>, IUIManager
    {
        #region Fields

        [Space] [Header("General Configs")] 
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private GameObject _canvasTemplate;
        [SerializeField] private Transform _canvasPanel;
        [SerializeField] private Transform _poolPanel;

        [Space]
        [Header("Input Configs")] 
        [SerializeField] private GameObject _uiInputLock;

        /// <summary>
        /// Current active canvases
        /// </summary>
        private readonly Dictionary<string, Canvas> _canvasMap = new();
        /// <summary>
        /// Dialog pool
        /// </summary>
        private readonly Dictionary<Type, BaseDialog> _dialogPool = new();
        /// <summary>
        /// Current dialog stack
        /// </summary>
        private readonly List<BaseDialog> _dialogStack = new(20);
        /// <summary>
        /// Current active builder
        /// </summary>
        private readonly Dictionary<Type, UIBuilder> _builderMap = new();

        /// <summary>
        /// Current lock request
        /// </summary>
        private int _lockCount = 0;

        #endregion

        #region Properties

        public event EventHandler<IUIManager.DialogEventArgs> OnDialogShown;
        public event EventHandler<IUIManager.DialogEventArgs> OnDialogHide;
        
        public List<BaseDialog> DialogStack => _dialogStack;
        public CanvasDatabase CanvasDatabase { get; private set; }
        public DialogDatabase DialogDatabase { get; private set; }
        /// <summary>
        /// Check if  the system had been initialized
        /// </summary>
        public bool HasInitialized { get; private set; }

        /// <summary>
        /// UI camera that render the whole UI
        /// </summary>
        public Camera UICamera => _uiCamera;

        /// <summary>
        /// Get canvas screen size
        /// </summary>
        public Vector2 CanvasSize
        {
            get
            {
                var rect = _canvasPanel.GetComponent<RectTransform>();
                return rect.rect.size;
            }
        }

        #endregion

        #region Unity Events

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            
            OnSceneLoaded(SceneManager.GetActiveScene(),LoadSceneMode.Single);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        #endregion

        #region Public Methods

        public IEnumerator Initialize(CanvasDatabase canvasDatabase, DialogDatabase dialogDatabase)
        {
            if (HasInitialized)
                yield break;
            HasInitialized = true;
            CanvasDatabase = canvasDatabase;
            DialogDatabase = dialogDatabase;

            //Spawn all the canvas has been configuration.
            foreach (var canvasInfo in CanvasDatabase.Canvases)
            {
                var go = Instantiate(_canvasTemplate, _canvasPanel);
                var canvas = go.GetComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingOrder = canvasInfo.order;
                canvas.gameObject.name = $"Layer {canvasInfo.canvasName}";
                canvas.transform.localScale = Vector3.one;
                FitParent(canvas.gameObject);
                
                _canvasMap.Add(canvasInfo.canvasName,canvas);
            }

            yield return null;

            //Preload all dialog marked as "keep in memory"
            int totalPreload = 0;
            int currentPreload = 0;
            foreach (var dialogInfo in DialogDatabase.DialogInfos)
            {
                if (dialogInfo.keepInMemory)
                {
                    totalPreload += 1;
                    StartCoroutine(SpawnDialogsAsync(dialogInfo, (dialog) =>
                    {
                        AddToPool(dialog.GetComponent<BaseDialog>());
                        currentPreload += 1;
                    }));
                }
            }

            yield return new WaitUntil(() => currentPreload >= totalPreload);
        }

        public UIBuilder Show(Type type, object args = null)
        {
            if (!CheckForInitialize())
            {
                return null;
            }
            
            if (_builderMap.ContainsKey(type))
            {
                Debug.LogWarning($"UI type {type} is being shown");
                return _builderMap[type];
            }
            
            var builder = new UIBuilder();
            _builderMap.Add(type,builder);

            StartCoroutine(ShowInternal(builder, type, args));

            return builder;
        }

        public UIBuilder Show<T>(object args = null) where T : BaseDialog
        {
            return Show(typeof(T), args);
        }

        public void Hide(Type type, object args = null)
        {
            if (!CheckForInitialize())
            {
                return;
            }

            StartCoroutine(HideInternal(type, args));
        }

        public void Hide<T>(object args = null) where T : BaseDialog
        {
            Hide(typeof(T), args);
        }

        public T Find<T>(bool includeInactive = false) where T : BaseDialog
        {
            if (!CheckForInitialize())
                return default;

            if (_builderMap.ContainsKey(typeof(T)))
            {
                Debug.LogWarning($"[UI system] - dialog is loading, please wait for next frame...");
                return default;
            }
            
            return (T)FindInternal(typeof(T), includeInactive);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Unity's on scene loaded event.
        /// We'll preload all dialogs require for loaded scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="loadMode"></param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            foreach (var dialogInfo in DialogDatabase.DialogInfos)
            {
                if(dialogInfo.keepInMemory)
                    continue;
                if (dialogInfo.preloadSceneNames.FirstOrDefault(x => x == scene.name) != null)
                {
                    StartCoroutine(SpawnDialogsAsync(dialogInfo, (dialog) =>
                    {
                        AddToPool(dialog.GetComponent<BaseDialog>());
                    }));
                }
            }

            _uiCamera.depth = Camera.main.depth + 1;
            _uiCamera.clearFlags = CameraClearFlags.Depth;

            // var cams = Camera.allCameras;
            // foreach (var cam in cams)
            // {
            //     if(cam == _uiCamera)
            //         continue;
            //
            //     var data = cam.GetUniversalAdditionalCameraData();
            //     if (data.renderType == CameraRenderType.Base)
            //     {
            //         data.cameraStack.Add(_uiCamera);
            //     }
            // }
        }
        
        /// <summary>
        /// Unity's on scene unloaded event
        /// We'll remove all dialog that is not "keep in memory"
        /// </summary>
        /// <param name="arg0"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnSceneUnloaded(Scene arg0)
        {
            foreach (var dialog in _dialogStack)
            {
                var setting = DialogDatabase.GetDialogInfoWithType(dialog.GetType());
                if (setting is {keepInMemory: true})
                {
                    AddToPool(dialog);
                }
                else
                {
                    GameObject.Destroy(dialog.gameObject);
                }
            }
            _dialogStack.Clear();

            var keysToRemoved = new List<Type>();
            foreach (var item in _dialogPool)
            {
                var setting = DialogDatabase.GetDialogInfoWithType(item.Key);
                if (setting is {keepInMemory: true}) 
                    continue;
                
                GameObject.Destroy(item.Value.gameObject);
                keysToRemoved.Add(item.Key);
            }

            foreach (var key in keysToRemoved)
            {
                _dialogPool.Remove(key);
            }
        }

        /// <summary>
        /// Begin process to load dialog, setup data & play show animation.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private IEnumerator ShowInternal(UIBuilder builder, Type type, object args = null)
        {
            var setting = DialogDatabase.GetDialogInfoWithType(type);
            var dialog = FindInternal(type, true);

            if (setting == null)
            {
                Debug.LogError("Setting is null " + type.FullName);
            }

            if (setting.lockInteractWhenTransition)
            {
                _lockCount += 1;
                _uiInputLock.SetActive(true);
            }

            if (dialog == null)
            {
                yield return SpawnDialogsAsync(setting, go =>
                {
                    dialog = go.GetComponent<BaseDialog>();
                    dialog.gameObject.SetActive(false);
                });

                yield return new WaitWhile(() => dialog == null);
            }
            else
            {
                yield return null; // Wait for user submit builder
            }

            dialog.Builder = builder;
            dialog.RequestHide = AutoHide;
            if (dialog.Builder.ShowAnimation == null)
            {
                dialog.Builder.ShowAnimation = setting.showAnimation;
            }
            if (dialog.Builder.HideAnimation == null)
            {
                dialog.Builder.HideAnimation = setting.hideAnimation;
            }
            
            AddToStack(dialog, setting);
            dialog.Setup(args);
            dialog.gameObject.SetActive(false);
            yield return dialog.PlayShowAnimation();

            _builderMap.Remove(type);
            
            _lockCount -= 1;
            if (_lockCount <= 0)
            {
                _uiInputLock.SetActive(false);
                _lockCount = 0;
            }
            
            dialog.Builder?.onShow?.Invoke();
            OnDialogShown?.Invoke(this, new IUIManager.DialogEventArgs()
            {
                type = type,
                args = null
            });
        }

        /// <summary>
        /// Begin process to hide dialog
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private IEnumerator HideInternal(Type type, object args = null)
        {
            // var setting = DialogDatabase.GetDialogInfoWithType(type);
            var dialog = FindInternal(type);

            if (dialog == null)
            {
                Debug.LogWarning($"[UI system] - Cant find dialog with type {type}" );
                yield break;
            }
            
            yield return dialog.PlayHideAnimation();
            
            // Debug.LogError($"[UI System] - Remove from Stack {dialog.GetType().FullName}");
            _dialogStack.Remove(dialog);
            AddToPool(dialog);
            dialog.Builder?.onHide?.Invoke(args);
            OnDialogHide?.Invoke(this, new IUIManager.DialogEventArgs()
            {
                type = type,
                args = args
            });
        }
        
        /// <summary>
        /// Find dialog with type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includePooled"></param>
        /// <returns></returns>
        private BaseDialog FindInternal(Type type, bool includePooled = false)
        {
            //Find from current stack
            foreach (var item in _dialogStack)
            {
                if (item.GetType() == type)
                {
                    // Debug.LogError($"[UI System] - Get from Stack {item.GetType().FullName}");
                    return item;
                }
            }

            //Find from pools
            return includePooled ? GetFromPool(type) : null;
        }

        private void AddToStack(BaseDialog dialog, DialogInfo info)
        {
            // Debug.LogError($"[UI System] - Add to Stack {dialog.GetType().FullName}");
            _dialogStack.Add(dialog);
            dialog.gameObject.SetActive(true);
            if (_canvasMap.ContainsKey(info.canvasName))
            {
                dialog.transform.SetParent(_canvasMap[info.canvasName].transform);
            }
            else
            {
                Canvas canvas = _canvasMap.FirstOrDefault().Value;
                dialog.transform.SetParent(canvas.transform);
            }
            dialog.transform.localScale = Vector3.one;
            FitParent(dialog.gameObject);
        }

        /// <summary>
        /// Add dialog to pool for reuse.
        /// NOTE: "keep in memory" dialog will always stored in pool.
        /// </summary>
        /// <param name="dialog"></param>
        private void AddToPool(BaseDialog dialog)
        {
            var type = dialog.GetType();
            // Debug.LogError($"[UI System] - Add to Pool {type.FullName}");
            if (_dialogPool.ContainsKey(type))
            {
                if (_dialogPool[type] != dialog)
                {
                    Debug.LogWarning($"[UI System] Dialog type {type.FullName} already exist! " +
                                     $"Destroy 1 instance");
                    Destroy(dialog.gameObject);
                }
                else
                {
                    Debug.LogWarning($"[UI System] Dialog type {type.FullName} already added to pool!");
                }
                return;
            }
            
            _dialogPool.Add(type,dialog);
            dialog.gameObject.SetActive(false);
            dialog.transform.SetParent(_poolPanel,true);
        }

        /// <summary>
        /// Get dialog from dialog pool
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private BaseDialog GetFromPool(Type type)
        {
            if (!_dialogPool.ContainsKey(type)) 
                return null;
            
            // Debug.LogError($"[UI System] - Get from Pool {type.FullName}");
            var dialog = _dialogPool[type];
            _dialogPool.Remove(type);
            return dialog;

        }

        /// <summary>
        /// Get dialog from dialog pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T GetFromPool<T>() where T : BaseDialog
        {
            return (T)GetFromPool(typeof(T));
        }

        /// <summary>
        /// Event called when dialog request to hide
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AutoHide(object sender, BaseDialog.DialogHideArgs args)
        {
            Hide(args.dialog.GetType(),args.args);
        }

        /// <summary>
        /// Load dialog async from Addressable
        /// </summary>
        /// <param name="info"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        private IEnumerator SpawnDialogsAsync(DialogInfo info , 
                                                Action<GameObject> onCompleted = null)
        {
            var op = Addressables.LoadAssetAsync<GameObject>(info.assetKey);
            yield return op;
            onCompleted?.Invoke(op.Result != null ? GameObject.Instantiate(op.Result) : null);
        }

        /// <summary>
        /// Check for initialization
        /// </summary>
        /// <returns></returns>
        private bool CheckForInitialize()
        {
            if (HasInitialized) return true;
            Debug.LogError($"[UI System] - You need to call Initialized() first!");
            return false;
        }
        
        private static void FitParent(GameObject go)
        {
            var rectTransform = go.GetComponent<RectTransform>();
            var parent = rectTransform.parent.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = Vector2.one * 0.5f;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,parent.rect.width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,parent.rect.height);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localPosition = Vector3.zero;
        }

        #endregion
    }   
}