using GPUInstancer;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace ANkleBreaker.GPUInstancer
{
    [RequireComponent(typeof(GPUInstancerPrefabManager))]
    public class AB_GPUInstancerPrefabManager : MonoBehaviour
    {
        [InfoBox("$" + nameof(_errorMessage), InfoMessageType.Error, nameof(FoundError))] [SerializeField]
        [ReadOnly]
        private GPUInstancerPrefabManager _gpuInstancerPrefabManager;

        public bool FoundError { get; private set; }
        private string _errorMessage;

        #region Initialize

        public void Start()
        {
            CheckErrors();
        }

        #endregion

        #region Editor

        [OnInspectorInit]
        private void OnInspectorInit()
        {
            if (gameObject.GetComponentIndex(this) != 0)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
            }

            CheckErrors();
        }
        
        [Button]
        public void CheckErrors()
        {
            CheckRegisteredPrefabs();
        }

        [Button("Register Instances in Scene & Save")]
        public void RegisterInstancesInSceneAndSave()
        {
            CleanRegisteredPrefabs();
            
            _gpuInstancerPrefabManager.RegisterPrefabsInScene();

            SaveGPUInstancer();
            CheckErrors();
        }

        [Button]
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

            foreach (RegisteredPrefabsData registeredPrefabsData in _gpuInstancerPrefabManager.registeredPrefabs)
            {
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