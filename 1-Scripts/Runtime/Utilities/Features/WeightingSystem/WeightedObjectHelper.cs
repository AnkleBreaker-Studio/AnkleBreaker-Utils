using System.Collections.Generic;
using System.Linq;

namespace AnkleBreaker.Utils.Weighting
{
    public static class WeightedObjectHelper
    {
        public static T GetRandomObject<T>(List<T> weightObjects,System.Random rnd, float totalWeight) where T : IWeightObject
        {
            if (weightObjects == null)
                return default;

            float diceRoll = (float)(rnd.NextDouble()) * totalWeight;
            
            foreach (T prefabWeight in weightObjects)
            {
                if (prefabWeight.Weight >= diceRoll)
                    return prefabWeight;
                diceRoll -= prefabWeight.Weight;
            }

            return default;
        }
        
        public static T GetRandomObject<T>(List<T> weightObjects, float totalWeight) where T : IWeightObject
        {
            if (weightObjects == null)
                return default;
            
            float diceRoll = UnityEngine.Random.Range(0f, totalWeight);
            foreach (T prefabWeight in weightObjects)
            {
                if (prefabWeight.Weight >= diceRoll)
                    return prefabWeight;
                diceRoll -= prefabWeight.Weight;
            }

            return default;
        }
        
        public static T GetRandomObject<T>(List<T> weightObjects) where T : IWeightObject
        {
            if (weightObjects == null)
                return default;
            
            float totalWeight = weightObjects.Sum(p => p.Weight);

            return GetRandomObject(weightObjects, totalWeight);
        }
    }
}