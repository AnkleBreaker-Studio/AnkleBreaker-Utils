using UnityEngine;
using UnityEngine.Animations;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class QuaternionExtensions
    {
        public static Quaternion SetValue(this Quaternion quaternion, Axis axis, float value)
        {
            Vector3 euler = quaternion.eulerAngles;
            switch (axis)
            {
                case Axis.X:
                    euler.x = value;
                    break;
                case Axis.Y:
                    euler.y = value;
                    break;
                case Axis.Z:
                    euler.z = value;
                    break;
                default:
                    return quaternion;
            }
            return Quaternion.Euler(euler);
        }

        public static Quaternion CapValue(this Quaternion quaternion, Axis axis, float value1, float value2)
        {
            if (value1 > value2)
            {
                float tmp = value2;
                value2 = value1;
                value1 = tmp;
            }
            Vector3 euler = quaternion.eulerAngles;

            switch (axis)
            {
                case Axis.X:
                    if (euler.x < value1) euler.x = value1;
                    if (euler.x > value2) euler.x = value2;
                    break;
                case Axis.Y:
                    if (euler.y < value1) euler.y = value1;
                    if (euler.y > value2) euler.y = value2;
                    break;
                case Axis.Z:
                    if (euler.z < value1) euler.z = value1;
                    if (euler.z > value2) euler.z = value2;
                    break;
                default:
                    return quaternion;
            }
            return Quaternion.Euler(euler);
        }

        public static Quaternion CapVersusReference(this Quaternion quaternion, Quaternion reference, float maxAngle)
        {
            float angle = Quaternion.Angle(quaternion, reference);

            if (angle <= maxAngle)
                return quaternion;

            return Quaternion.Slerp(quaternion, reference, 1f - maxAngle / angle);
        }
    }
}
