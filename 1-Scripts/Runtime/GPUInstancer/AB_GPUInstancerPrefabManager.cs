using GPUInstancer;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace AnkleBreaker.GPUInstancer
{
    [RequireComponent(typeof(GPUInstancerPrefabManager))]
    public class AB_GPUInstancerPrefabManager : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [InfoBox("$" + nameof(_errorMessage), InfoMessageType.Error, nameof(FoundError))]
        [ReadOnly]
#endif
        [SerializeField]
        private GPUInstancerPrefabManager _gpuInstancerPrefabManager;

        public bool FoundError { get; private set; }
        private string _errorMessage;

        #region Initialize

        public void Start()
        {
            #if UNITY_SERVER
            
            Destroy(gameObject);
            return;
            
            #endif
            
            CheckErrors();
        }

        #endregion

        #region Editor

#if ODIN_INSPECTOR
        [OnInspectorInit]
#endif
        private void OnInspectorInit()
        {
            #if UNITY_EDITOR
            if (gameObject.GetComponentIndex(this) != 0)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
            }

            CheckErrors();
            #endif
        }
        
#if ODIN_INSPECTOR
        [Button]
#endif
        public void CheckErrors()
        {
            CheckRegisteredPrefabs();
        }

#if ODIN_INSPECTOR
        [Button("Register Instances in Scene & Save")]
#endif
        public void RegisterInstancesInSceneAndSave()
        {
            CleanRegisteredPrefabs();
            
            _gpuInstancerPrefabManager.RegisterPrefabsInScene();

            SaveGPUInstancer();
            CheckErrors();
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        private void CleanRegisteredPrefabs()
        {
            InitGPUInstancerPrefabManager();
            
            _gpuInstancerPrefabManager.registeredPrefabs.Clear();
            
            SaveGPUInstancer();
            CheckErrors();
        }

        public void SaveGPUInstancer()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
            AssetDatabase.SaveAssetIfDirty(gameObject);
            EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
        }

        #endregion
        
        #region Core
        
        private void CheckRegisteredPrefabs()
        {
            InitGPUInstancerPrefabManager();

            FoundError = false;
            _errorMessage = "";

            if (_gpuInstancerPrefabManager.registeredPrefabs == null)
                return;
            
            foreach (RegisteredPrefabsData registeredPrefabsData in _gpuInstancerPrefabManager.registeredPrefabs)
            {
                if (registeredPrefabsData == null || registeredPrefabsData.Equals((Object)null))
                {
                    FoundError = true;
                }
                
                foreach (GPUInstancerPrefab gpuInstancerPrefab in registeredPrefabsData.registeredPrefabs)
                {
                    if (gpuInstancerPrefab == null || gpuInstancerPrefab.Equals((Object)null))
                    {
                        FoundError = true;
                    }
                }
            }

            if (FoundError)
            {
                _errorMessage =
                    "Found null or missing references inside registeredPrefabs ! Please use the \"Register Instances in Scene & Save\" button to fix it";

                if (!Application.isPlaying)
                {
                    Debug.LogError(_errorMessage);
                }
                else
                {
                    Debug.LogError("Found null or missing references inside registeredPrefabs ! Please check " + name);
                }
            }
        }

        public void InitGPUInstancerPrefabManager()
        {
            if (_gpuInstancerPrefabManager == null)
                _gpuInstancerPrefabManager = GetComponent<GPUInstancerPrefabManager>();
        }

        #endregion
    }
}