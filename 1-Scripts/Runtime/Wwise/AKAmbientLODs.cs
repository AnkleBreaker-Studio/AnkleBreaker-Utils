using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnkleBreaker.Utils.Wwise
{
    #if UNITY_SERVER
    
    public class AKAmbientLODs : MonoBehaviour
    {
#if UNITY_EDITOR
        [OnInspectorGUI]
        private void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This script is a server version of AKAmbientLODs, it contains no logic ! Should not be used on server", MessageType.Warning);
        }
#endif

        private void Awake()
        {
            Destroy(this);
        }
    }
    
    #else
    [RequireComponent(typeof(AkAmbient))]
    public class AKAmbientLODs : MonoBehaviour
    {
        [Header("Enable or disable AkAmbient based on distance from the camera")] [SerializeField] [Range(0, 500)]
        private int _distance = 50;

        [Header("Number of tick between refresh")] [SerializeField] [Range(1, 100)]
        private int _refreshRate = 30;

        private Transform _camera;
        private Transform _current;
        private int _tickOffset;
        private int _currentTick;

        [Sirenix.OdinInspector.ReadOnly, ShowInInspector]
        private bool _isActive;

        [Sirenix.OdinInspector.ReadOnly, SerializeField, OnInspectorInit(nameof(InitAkAmbient))]
        private AkAmbient _ambient;

        [SerializeField] [ValueDropdown(nameof(InitTriggerDropDown))]
        private uint _triggerType;

        [SerializeReference,Sirenix.OdinInspector.ReadOnly, HideInInspector]
        private System.Collections.Generic.Dictionary<uint, string> _triggerTypes;

        private void InitAkAmbient()
        {
            _triggerTypes = AkTriggerBase.GetAllDerivedTypes();
            _ambient = GetComponent<AkAmbient>();
        }

        [Sirenix.OdinInspector.Button]
        private void SetupTrigger()
        {
            _triggerType = (uint)_ambient.triggerList[0];
            _ambient.triggerList[0] = 0;
        }

        private void Start()
        {
            _camera = FindFirstObjectByType<Camera>(FindObjectsInactive.Include).transform;
            _current = transform;
            if (_refreshRate > 1)
            {
                _tickOffset = Random.Range(0, _refreshRate);
                _currentTick = _tickOffset;
            }

            _isActive = false;

            _triggerTypes = AkTriggerBase.GetAllDerivedTypes();
        }

        private void FixedUpdate()
        {
            _currentTick++;
            if (_currentTick > _refreshRate + _tickOffset)
            {
                _currentTick = _tickOffset;
                CheckDistance();
            }
        }

        private void CheckDistance()
        {
            if (_camera == null || _current == null)
                return;

            float distance = Vector3.Distance(_camera.position, _current.position);
            UpdateLods(distance < _distance);
        }

        private void UpdateLods(bool visible)
        {
            if (_isActive == visible) return;
            _isActive = visible;

            _ambient.enabled = visible;
            if (_isActive)
            {
                if (_triggerTypes[_triggerType] == "Start" ||
                    _triggerTypes[_triggerType] == "Awake")
                {
                    _ambient.HandleEvent(_ambient.gameObject);
                }
            }

            if (!_isActive)
            {
                _ambient.Stop(5);
            }
        }

        private IEnumerable InitTriggerDropDown()
        {
            ValueDropdownList<uint> triggerDropDownList = new ValueDropdownList<uint>();

            foreach (var kv in _triggerTypes)
            {
                triggerDropDownList.Add(kv.Value, kv.Key);
            }

            return triggerDropDownList;
        }
    }
    #endif
}