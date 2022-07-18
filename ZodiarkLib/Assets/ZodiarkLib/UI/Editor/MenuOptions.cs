using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ZodiarkLib.UI.Editors
{
    internal static class MenuOptions
    {
        [MenuItem("GameObject/UI/Custom/UI Panel")]
        private static void CreateUIPanel(MenuCommand menuCommand)
        {
            var panelRoot = CreateEmptyPanel("UI Panel", menuCommand);
            var rectTransform = panelRoot.GetComponent<RectTransform>();

            var mask = new GameObject("Mask", typeof(RectTransform), typeof(Image));
            AddToParent(mask,rectTransform,true);
            var rectImage = mask.GetComponent<Image>();
            rectImage.color = new Color(0, 0, 0, 0.5f);

            var container = new GameObject("Container", typeof(RectTransform));
            AddToParent(container,rectTransform,false);

            var containerBg = new GameObject("Background", typeof(RectTransform), typeof(Image));
            AddToParent(containerBg, container.transform,true);
            var containerBgImage = containerBg.GetComponent<Image>();
            containerBgImage.color = new Color(1, 1, 1, 1f);
            
            SetLayerRecursively(panelRoot, panelRoot.transform.parent.gameObject.layer);
        }

        [MenuItem("GameObject/UI/Custom/UI Panel with 3D environment")]
        private static void CreateUIPanel3D(MenuCommand menuCommand)
        {
            var panelRoot = CreateEmptyPanel("UI Panel 3D", menuCommand);
            var rectTransform = panelRoot.GetComponent<RectTransform>();
            
            var container = new GameObject("Container", typeof(RectTransform));
            AddToParent(container,rectTransform,false);

            var modelContainer = new GameObject("3D", typeof(RectTransform));
            AddToParent(modelContainer, rectTransform,true);

            var cameraObj = new GameObject("3DCamera", typeof(Camera),typeof(RectTransform));
            AddToParent(cameraObj , rectTransform,false);

            int layer = panelRoot.transform.parent.gameObject.layer;
            
            var camera = cameraObj.GetComponent<Camera>();
            camera.cullingMask = 1 << layer;
            
            SetLayerRecursively(panelRoot, layer);
        }

        [MenuItem("GameObject/UI/Custom/UI Button")]
        private static void CreateCustomButton(MenuCommand menuCommand)
        {
            var panelRoot = CreateEmptyButton("Custom Button", menuCommand);
            var rectTransform = panelRoot.GetComponent<RectTransform>();
            
            var hitbox = new GameObject("Hitbox", typeof(RectTransform),typeof(TextMeshProUGUI));
            AddToParent(hitbox, rectTransform, true);

            var graphics = new GameObject("Graphics", typeof(RectTransform));
            AddToParent(graphics, rectTransform, true);

            var buttonBackground = new GameObject("Background", typeof(RectTransform), typeof(Image));
            AddToParent(buttonBackground, graphics.transform, true);

            var buttonLabel = new GameObject("Label (TMP)", typeof(RectTransform), typeof(TextMeshProUGUI));
            AddToParent(buttonLabel, graphics.transform, true);
            var labelText = buttonLabel.GetComponent<TMP_Text>();
            labelText.alignment = TextAlignmentOptions.CenterGeoAligned;
            
            SetLayerRecursively(panelRoot, panelRoot.transform.parent.gameObject.layer);
        }

        private static GameObject CreateEmptyPanel(string name, MenuCommand menuCommand)
        {
            var panelRoot = new GameObject(name, typeof(RectTransform));
            var rectTransform = panelRoot.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;

            PlaceUIElementRoot(panelRoot, menuCommand);

            panelRoot.AddComponent<Canvas>();
            panelRoot.AddComponent<CanvasGroup>();
            panelRoot.AddComponent<GraphicRaycaster>();
            FitParent(rectTransform);

            return panelRoot;
        }

        private static GameObject CreateEmptyButton(string name, MenuCommand menuCommand)
        {
            var panelRoot = new GameObject(name, typeof(RectTransform));
            var rectTransform = panelRoot.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.one * 0.5f;
            rectTransform.anchorMax = Vector2.one * 0.5f;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(200, 120);

            PlaceUIElementRoot(panelRoot, menuCommand);

            return panelRoot;
        }

        private static GameObject AddEmptyChild(string name, Transform parent , 
            bool fitParent, 
            params System.Type[] comps)
        {
            var child = new GameObject(name, comps);
            AddToParent(child, parent,fitParent);

            return child;
        }
        
        private static void AddToParent(GameObject go, Transform parent, bool isFitParent)
        {
            go.transform.SetParent(parent);
            go.transform.localScale = Vector3.one;
            var rectMask = go.GetComponent<RectTransform>();
            if (isFitParent)
            {
                rectMask.anchorMin = Vector2.zero;
                rectMask.anchorMax = Vector2.one;
                FitParent(rectMask);
            }
            else
            {
                rectMask.anchorMin = Vector2.one * 0.5f;
                rectMask.anchorMax = Vector2.one * 0.5f;
                rectMask.anchoredPosition = Vector2.zero;
                rectMask.sizeDelta = new Vector2(300, 300);
            }
            go.transform.localPosition = Vector3.up;
            rectMask.anchoredPosition = Vector2.zero;
        }

        private static void FitParent(RectTransform rectTransform)
        {
            var parent = rectTransform.parent.GetComponent<RectTransform>();
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,parent.rect.width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,parent.rect.height);
            rectTransform.anchoredPosition = Vector2.zero;
        }
        
        private static void SetPositionVisibleOnSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
        {
            SceneView sceneView = SceneView.lastActiveSceneView;

            // Couldn't find a SceneView. Don't set position.
            if (sceneView == null || sceneView.camera == null)
                return;

            // Create world space Plane from canvas position.
            Vector2 localPlanePosition;
            Camera camera = sceneView.camera;
            Vector3 position = Vector3.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
            {
                // Adjust for canvas pivot
                localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
                localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

                localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
                localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

                // Adjust for anchoring
                position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
                position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

                Vector3 minLocalPosition;
                minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
                minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

                Vector3 maxLocalPosition;
                maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
                maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

                position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
                position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
            }

            itemTransform.anchoredPosition = position;
            itemTransform.localRotation = Quaternion.identity;
            itemTransform.localScale = Vector3.one;
        }

        private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
        {
            GameObject parent = menuCommand.context as GameObject;
            bool explicitParentChoice = true;
            if (parent == null)
            {
                parent = GetOrCreateCanvasGameObject();
                explicitParentChoice = false;

                // If in Prefab Mode, Canvas has to be part of Prefab contents,
                // otherwise use Prefab root instead.
                PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null && !prefabStage.IsPartOfPrefabContents(parent))
                    parent = prefabStage.prefabContentsRoot;
            }
            if (parent.GetComponentsInParent<Canvas>(true).Length == 0)
            {
                // Create canvas under context GameObject,
                // and make that be the parent which UI element is added under.
                GameObject canvas = MenuOptions.CreateNewUI();
                canvas.transform.SetParent(parent.transform, false);
                parent = canvas;
            }

            // Setting the element to be a child of an element already in the scene should
            // be sufficient to also move the element to that scene.
            // However, it seems the element needs to be already in its destination scene when the
            // RegisterCreatedObjectUndo is performed; otherwise the scene it was created in is dirtied.
            SceneManager.MoveGameObjectToScene(element, parent.scene);

            Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);

            if (element.transform.parent == null)
            {
                Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
            }

            GameObjectUtility.EnsureUniqueNameForSibling(element);

            // We have to fix up the undo name since the name of the object was only known after reparenting it.
            Undo.SetCurrentGroupName("Create " + element.name);

            GameObjectUtility.SetParentAndAlign(element, parent);
            if (!explicitParentChoice) // not a context click, so center in sceneview
                SetPositionVisibleOnSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

            Selection.activeGameObject = element;
        }
        
        // Helper function that returns a Canvas GameObject; preferably a parent of the selection, or other existing Canvas.
        private static GameObject GetOrCreateCanvasGameObject()
        {
            var selectedGo = Selection.activeGameObject;
            // Try to find a gameobject that is the selected GO or one if its parents.
            var canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
            if (IsValidCanvas(canvas))
                return canvas.gameObject;

            // No canvas in selection or its parents? Then use any valid canvas.
            // We have to find all loaded Canvases, not just the ones in main scenes.
            var canvasArray = StageUtility.GetCurrentStageHandle().FindComponentsOfType<Canvas>();
            foreach (var c in canvasArray)
                if (IsValidCanvas(c))
                    return c.rootCanvas.gameObject;

            // No canvas in the scene at all? Then create a new one.
            return MenuOptions.CreateNewUI();
        }
        
        private static GameObject CreateNewUI()
        {
            // Root for the UI
            var root = new GameObject("Canvas");
            root.layer = LayerMask.NameToLayer("UI");
            Canvas canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            root.AddComponent<CanvasScaler>();
            root.AddComponent<GraphicRaycaster>();

            // Works for all stages.
            StageUtility.PlaceGameObjectInCurrentStage(root);
            bool customScene = false;
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                root.transform.SetParent(prefabStage.prefabContentsRoot.transform, false);
                customScene = true;
            }

            Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

            // If there is no event system add one...
            // No need to place event system in custom scene as these are temporary anyway.
            // It can be argued for or against placing it in the user scenes,
            // but let's not modify scene user is not currently looking at.
            if (!customScene)
                CreateEventSystem(false,null);
            return root;
        }
        
        private static void CreateEventSystem(bool select, GameObject parent)
        {
            StageHandle stage = parent == null ? StageUtility.GetCurrentStageHandle() : StageUtility.GetStageHandle(parent);
            var esys = stage.FindComponentOfType<EventSystem>();
            if (esys == null)
            {
                var eventSystem = new GameObject("EventSystem");
                if (parent == null)
                    StageUtility.PlaceGameObjectInCurrentStage(eventSystem);
                else
                    GameObjectUtility.SetParentAndAlign(eventSystem, parent);
                esys = eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();

                Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
            }

            if (select && esys != null)
            {
                Selection.activeGameObject = esys.gameObject;
            }
        }

        private static bool IsValidCanvas(Canvas canvas)
        {
            if (canvas == null || !canvas.gameObject.activeInHierarchy)
                return false;

            // It's important that the non-editable canvas from a prefab scene won't be rejected,
            // but canvases not visible in the Hierarchy at all do. Don't check for HideAndDontSave.
            if (EditorUtility.IsPersistent(canvas) || (canvas.hideFlags & HideFlags.HideInHierarchy) != 0)
                return false;

            return StageUtility.GetStageHandle(canvas.gameObject) == StageUtility.GetCurrentStageHandle();
        }
        
        private static void SetLayerRecursively(GameObject go, int layer)
        {
            go.layer = layer;
            Transform transform = go.transform;
            for (int index = 0; index < transform.childCount; ++index)
                SetLayerRecursively(transform.GetChild(index).gameObject, layer);
        }
    }   
}