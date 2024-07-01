using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class DebugExtensions
    {
        private static Vector4[] MakeUnitSphere(int len)
        {
            Debug.Assert(len > 2);
            var v = new Vector4[len * 3];
            for (int i = 0; i < len; i++)
            {
                var f = i / (float)len;
                float c = Mathf.Cos(f * (float)(Mathf.PI * 2.0));
                float s = Mathf.Sin(f * (float)(Mathf.PI * 2.0));
                v[0 * len + i] = new Vector4(c, s, 0, 1);
                v[1 * len + i] = new Vector4(0, c, s, 1);
                v[2 * len + i] = new Vector4(s, 0, c, 1);
            }
            return v;
        }
        
        public static void DrawSphere(Vector4 pos, float radius, Color color, float duration = 0)
        {
            Vector4[] v = MakeUnitSphere(16);
            int len = v.Length / 3;
            for (int i = 0; i < len; i++)
            {
                var sX = pos + radius * v[0 * len + i];
                var eX = pos + radius * v[0 * len + (i + 1) % len];
                var sY = pos + radius * v[1 * len + i];
                var eY = pos + radius * v[1 * len + (i + 1) % len];
                var sZ = pos + radius * v[2 * len + i];
                var eZ = pos + radius * v[2 * len + (i + 1) % len];
                Debug.DrawLine(sX, eX, color, duration);
                Debug.DrawLine(sY, eY, color, duration);
                Debug.DrawLine(sZ, eZ, color, duration);
            }
        }
        
        private static void DrawRectangle(Vector3 position, Quaternion orientation, Vector2 extent, Color color, float duration = 0)
        {
            Vector3 rightOffset = Vector3.right * extent.x * 0.5f;
            Vector3 upOffset = Vector3.up * extent.y * 0.5f;
 
            Vector3 offsetA = orientation * (rightOffset + upOffset);
            Vector3 offsetB = orientation * (-rightOffset + upOffset);
            Vector3 offsetC = orientation * (-rightOffset - upOffset);
            Vector3 offsetD = orientation * (rightOffset - upOffset);
 
            Debug.DrawLine(position + offsetA, position + offsetB, color, duration);
            Debug.DrawLine(position + offsetB, position + offsetC, color, duration);
            Debug.DrawLine(position + offsetC, position + offsetD, color, duration);
            Debug.DrawLine(position + offsetD, position + offsetA, color, duration);
        }
        
        public static void DrawCube(Vector3 position, Quaternion orientation, float size, Color color, float duration = 0)
        {
            Vector3 offsetX = orientation * Vector3.right * size * 0.5f;
            Vector3 offsetY = orientation * Vector3.up * size * 0.5f;
            Vector3 offsetZ = orientation * Vector3.forward * size * 0.5f;
 
            Vector3 pointA = -offsetX + offsetY;
            Vector3 pointB = offsetX + offsetY;
            Vector3 pointC = offsetX - offsetY;
            Vector3 pointD = -offsetX - offsetY;
 
            DrawRectangle(position - offsetZ, orientation, Vector2.one * size, color, duration);
            DrawRectangle(position + offsetZ, orientation, Vector2.one * size, color, duration);
 
            Debug.DrawLine(pointA - offsetZ, pointA + offsetZ, color, duration);
            Debug.DrawLine(pointB - offsetZ, pointB + offsetZ, color, duration);
            Debug.DrawLine(pointC - offsetZ, pointC + offsetZ, color, duration);
            Debug.DrawLine(pointD - offsetZ, pointD + offsetZ, color, duration);
        }
        
        public static void DrawBox(Vector3 position, Quaternion orientation, Vector3 size, Color color, float duration = 0)
        {
            Vector3 offsetX = orientation * Vector3.right * size.x * 0.5f;
            Vector3 offsetY = orientation * Vector3.up * size.y * 0.5f;
            Vector3 offsetZ = orientation * Vector3.forward * size.z * 0.5f;
 
            Vector3 pointA = -offsetX + offsetY;
            Vector3 pointB = offsetX + offsetY;
            Vector3 pointC = offsetX - offsetY;
            Vector3 pointD = -offsetX - offsetY;
 
            DrawRectangle(position - offsetZ, orientation, new Vector2(size.x, size.y), color, duration);
            DrawRectangle(position + offsetZ, orientation, new Vector2(size.x, size.y), color, duration);
 
            Debug.DrawLine(pointA - offsetZ + position, pointA + offsetZ + position, color, duration);
            Debug.DrawLine(pointB - offsetZ + position, pointB + offsetZ + position, color, duration);
            Debug.DrawLine(pointC - offsetZ + position, pointC + offsetZ + position, color, duration);
            Debug.DrawLine(pointD - offsetZ + position, pointD + offsetZ + position, color, duration);
        }
    }
}