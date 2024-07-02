#if ODIN_INSPECTOR
using BaseScriptableObject = Sirenix.OdinInspector.SerializedScriptableObject;
#else
using BaseScriptableObject = UnityEngine.ScriptableObject;
#endif
using System.Collections.Generic;
using System.Linq;

namespace AnkleBreaker.Utils.Weighting
{
    /// <summary>
    /// Inherit from this class when you want your scriptable object to use a weighting system
    /// </summary>
    public abstract class AWeightedObjectsSO : BaseScriptableObject
    {
        private bool _isInitialized = false;
        public float TotalWeight { get; protected set; }

        protected void Initialize<T>(List<T> weightObjects,bool forceInitialize = false) where T : IWeightObject
        {
            if (!_isInitialized || forceInitialize)
            {
                TotalWeight = weightObjects.Sum(p => p.Weight);
                _isInitialized = true;
            }
        }

        protected T GetRandomObject<T>(List<T> weightObjects) where T : IWeightObject
        {
            Initialize(weightObjects);
            float diceRoll = UnityEngine.Random.Range(0f, TotalWeight);
            foreach (T prefabWeight in weightObjects)
            {
                if (prefabWeight.Weight >= diceRoll)
                    return prefabWeight;
                diceRoll -= prefabWeight.Weight;
            }

            return default;
        }
    }
}