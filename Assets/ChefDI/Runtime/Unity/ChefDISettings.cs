using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace gs.ChefDI.Unity
{
    public sealed class ChefDISettings : ScriptableObject
    {
        public static ChefDISettings Instance { get; private set; }
        public static bool DiagnosticsEnabled => Instance != null && Instance.EnableDiagnostics;

        static LifetimeScope rootLifetimeScopeInstance;

        [SerializeField]
        [Tooltip("Set the Prefab to be the parent of the entire Project.")]
        public LifetimeScope RootLifetimeScope;

        [SerializeField]
        [Tooltip("Enables the collection of information that can be viewed in the ChefDIDiagnosticsWindow. Note: Performance degradation")]
        public bool EnableDiagnostics;

        [SerializeField]
        [Tooltip("Disables script modification for LifetimeScope scripts.")]
        public bool DisableScriptModifier;

        [SerializeField]
        [Tooltip("Removes (Clone) postfix in IObjectResolver.Instantiate() and IContainerBuilder.RegisterComponentInNewPrefab().")]
        public bool RemoveClonePostfix;

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/ChefDI/ChefDI Settings")]
        public static void CreateAsset()
        {
            var path = UnityEditor.EditorUtility.SaveFilePanelInProject(
                "Save ChefDISettings",
                "ChefDISettings",
                "asset",
                string.Empty);

            if (string.IsNullOrEmpty(path))
                return;

            var newSettings = CreateInstance<ChefDISettings>();
            UnityEditor.AssetDatabase.CreateAsset(newSettings, path);

            var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
            preloadedAssets.RemoveAll(x => x is ChefDISettings);
            preloadedAssets.Add(newSettings);
            UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }

        public static void LoadInstanceFromPreloadAssets()
        {
            var preloadAsset = UnityEditor.PlayerSettings.GetPreloadedAssets().FirstOrDefault(x => x is ChefDISettings);
            if (preloadAsset is ChefDISettings instance)
            {
                instance.OnDisable();
                instance.OnEnable();
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void RuntimeInitialize()
        {
            // For editor, we need to load the Preload asset manually.
            LoadInstanceFromPreloadAssets();
        }
#endif

        public LifetimeScope GetOrCreateRootLifetimeScopeInstance()
        {
            if (RootLifetimeScope != null && rootLifetimeScopeInstance == null)
            {
                var activeBefore = RootLifetimeScope.gameObject.activeSelf;
                RootLifetimeScope.gameObject.SetActive(false);

                rootLifetimeScopeInstance = Instantiate(RootLifetimeScope);
                DontDestroyOnLoad(rootLifetimeScopeInstance);
                rootLifetimeScopeInstance.gameObject.SetActive(true);

                RootLifetimeScope.gameObject.SetActive(activeBefore);
            }
            return rootLifetimeScopeInstance;
        }

        public bool IsRootLifetimeScopeInstance(LifetimeScope lifetimeScope) =>
            RootLifetimeScope == lifetimeScope || rootLifetimeScopeInstance == lifetimeScope;

        void OnEnable()
        {
            if (Application.isPlaying)
            {
                Instance = this;

                var activeScene = SceneManager.GetActiveScene();
                if (activeScene.isLoaded)
                {
                    OnFirstSceneLoaded(activeScene, default);
                }
                else
                {
                    SceneManager.sceneLoaded -= OnFirstSceneLoaded;
                    SceneManager.sceneLoaded += OnFirstSceneLoaded;
                }
            }
        }

        void OnDisable()
        {
            Instance = null;
        }

        void OnFirstSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (RootLifetimeScope != null &&
                RootLifetimeScope.autoRun &&
                (rootLifetimeScopeInstance == null || rootLifetimeScopeInstance.Container == null))
            {
                GetOrCreateRootLifetimeScopeInstance();
            }
            SceneManager.sceneLoaded -= OnFirstSceneLoaded;
        }
    }
}
