using AnkleBreaker.Utils.TypeDefinitions;
using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.AB_Types
{
    public static class PositionRotationExtensions
    {
        /// <summary>
        ///   <para>Transforms position from world space to local space.</para>
        /// </summary>
        /// <param name="position"></param>
        public static Vector3 InverseTransformPointEquivalent(this PositionRotation positionRotation, Vector3 targetPoint)
        {
            Vector3 targetLocalPosition = Quaternion.Inverse(positionRotation.Rotation) * (targetPoint - positionRotation.Position);
            return targetLocalPosition;
        }

        public static Vector3 InverseTransformDirectionEquivalent(this PositionRotation positionRotation, Vector3 targetDirection)
        {
            Vector3 targetLocalDirection = Quaternion.Inverse(positionRotation.Rotation) * targetDirection;
            return targetLocalDirection;
        }

        public static Quaternion InverseTransformRotationEquivalent(this PositionRotation positionRotation, Quaternion targetRotation)
        {
            Quaternion targetLocalRotation = Quaternion.Inverse(positionRotation.Rotation) * targetRotation;
            return targetLocalRotation;
        }

        public static PositionRotation InverseTransformPositionRotationEquivalent(this PositionRotation positionRotation, PositionRotation target)
        {
            Quaternion inverse = Quaternion.Inverse(positionRotation.Rotation);
            Vector3 targetLocalPosition =  inverse * (target.Position - positionRotation.Position);
            Quaternion targetLocalRotation = inverse * target.Rotation;
            return new PositionRotation() { Position = targetLocalPosition, Rotation = targetLocalRotation };
        }

        /// <summary>
        ///   <para>Transforms position from local space to world space.</para>
        /// </summary>
        /// <param name="position"></param>
        public static Vector3 TransformPointEquivalent(this PositionRotation positionRotation, Vector3 localPoint)
        {
            Vector3 worldPoint = positionRotation.Rotation * localPoint;
            worldPoint += positionRotation.Position;
            return worldPoint;
        }

        public static Vector3 TransformDirectionEquivalent(this PositionRotation positionRotation, Vector3 localDirection)
        {
            Vector3 worldDirection = positionRotation.Rotation * localDirection;
            return worldDirection;
        }

        public static Quaternion TransformRotationEquivalent(this PositionRotation positionRotation, Quaternion localRotation)
        {
            Quaternion worldRotation = positionRotation.Rotation * localRotation;
            return worldRotation;
        }

        public static PositionRotation TransformPositionRotationEquivalent(this PositionRotation positionRotation, PositionRotation local)
        {
            Vector3 worldPoint = positionRotation.Rotation * local.Position;
            worldPoint += positionRotation.Position;
            Quaternion worldRotation = positionRotation.Rotation * local.Rotation;
            return new PositionRotation() { Position = worldPoint, Rotation = worldRotation };
        }

        public static PositionRotation FromTransform(Transform transform)
        {
            return new PositionRotation() { Position = transform.position, Rotation = transform.rotation };
        }
    }
}
