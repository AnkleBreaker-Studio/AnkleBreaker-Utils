using System;
using UnityEngine;

namespace AnkleBreaker.Utils.Random
{
    public static class AB_Random
    {
        /// <summary>
        /// <para>Return a random int within (minInclusive..maxExclusive excluding excludedNbr) (Read Only).</para>
        /// </summary>
        /// <param name="minInclusive"></param>
        /// <param name="maxExclusive"></param>
        /// <param name="excludedNbr"></param>
        public static int Range(int minInclusive, int maxExclusive, int excludedNbr)
        {
            if (maxExclusive - minInclusive == 1 && minInclusive == excludedNbr)
            {
                throw new Exception("Can't find a random number : excludedNbr is the only nbr in range.");
            }

            int result = UnityEngine.Random.Range(minInclusive, maxExclusive);

            while (result == excludedNbr)
                result = UnityEngine.Random.Range(minInclusive, maxExclusive);

            return result;
        }

        public static Vector3 GetRandomPositionInTorus(float innerRadius, float outerRadius)
        {
            float wallRadius = (outerRadius - innerRadius) * 0.5f;
            float ringRadius = wallRadius + innerRadius;

            // get a random angle around the ring
            float rndAngle = UnityEngine.Random.value * 6.28f; // use radians, saves converting degrees to radians

            // determine position
            float cX = Mathf.Sin(rndAngle);
            float cZ = Mathf.Cos(rndAngle);

            Vector3 ringPos = new Vector3(cX, 0, cZ);
            ringPos *= ringRadius;

            // At any point around the center of the ring
            // a sphere of radius the same as the wallRadius will fit exactly into the torus.
            // Simply get a random point in a sphere of radius wallRadius,
            // then add that to the random center point
            Vector3 sPos = UnityEngine.Random.insideUnitSphere * wallRadius;

            return (ringPos + sPos);
        }

        public static Vector3 RandomPointInSphere(Vector3 center, float radius, Vector2 yOffsetRange = default)
        {
            // Generate a random point within a 2D circle
            Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * radius;
            // Convert the 2D point to 3D using the center position
            Vector3 randomPoint3D = new Vector3(randomPoint.x, 0, randomPoint.y);

            float yOffset = UnityEngine.Random.Range(yOffsetRange.x, yOffsetRange.y);
            randomPoint3D.y = yOffset;

            // Apply the offset from the center
            return center + randomPoint3D;
        }

        public static Vector3 RandomPointInSphere(System.Random random, Vector3 center, float radius,
            Vector2 yOffsetRange = default)
        {
            // Generate a random point within a 2D circle
            Vector2 randomPoint = GetRandomUnitCircle(random) * radius;
            // Convert the 2D point to 3D using the center position
            Vector3 randomPoint3D = new Vector3(randomPoint.x, 0, randomPoint.y);

            float yOffset = UnityEngine.Random.Range(yOffsetRange.x, yOffsetRange.y);
            randomPoint3D.y = yOffset;

            // Apply the offset from the center
            return center + randomPoint3D;
        }

        private static Vector2 GetRandomUnitCircle(System.Random random)
        {
            double angle = random.NextDouble() * 2 * Math.PI;
            float x = (float) Math.Cos(angle);
            float y = (float) Math.Sin(angle);
            return new Vector2(x, y);
        }

    }
}