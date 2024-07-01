using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class AnimationCurveExtensions
    {
        /// <summary>
        /// Inverts an animation curve
        /// </summary>
        /// <param name="curve">The curve to invert</param>
        public static void Invert(this AnimationCurve curve)
        {
            float minValue = float.MaxValue;
            float maxValue = float.MinValue;

            for (int i = 0; i < curve.keys.Length; i += curve.keys.Length - 1)
            {
                if (curve.keys[i].value > maxValue)
                {
                    maxValue = curve.keys[i].value;
                }
                if (curve.keys[i].value < minValue)
                {
                    minValue = curve.keys[i].value;
                }
            }

            float middleValue = (maxValue + minValue) * 0.5f;

            Keyframe[] keys = curve.keys;
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].value = keys[i].value + 2 * (middleValue - keys[i].value);
                keys[i].inTangent *= -1f;
                keys[i].inWeight *= -1f;
                keys[i].outTangent *= -1f;
                keys[i].outWeight *= -1f;
            }
            curve.keys = keys;
        }
    }
}