using UnityEditor;
using UnityEngine;

namespace AnkleBreaker.Utils.Editor
{
    public static class AB_Gizmos
    {
        public static void DrawCapsule(Vector3 pos, Quaternion rot, float radius, float height,
            Color color = default(Color))
        {
            if (color != default(Color))
            {
                UnityEngine.Gizmos.color = color;
                Color halved = color;
                halved.a /= 2;
                Handles.color = halved;
            }

            Matrix4x4 angleMatrix = Matrix4x4.TRS(pos, rot, Handles.matrix.lossyScale);
            using (new Handles.DrawingScope(angleMatrix))
            {
                UnityEngine.Gizmos.matrix = Handles.matrix;
                var pointOffset = height / 2;

                Handles.DrawSolidArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, radius);
                UnityEngine.Gizmos.DrawCube(Vector3.zero, new Vector3(radius * 2, height, 0));
                Handles.DrawSolidArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, radius);
                //draw frontways
                Handles.DrawSolidArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, radius);
                UnityEngine.Gizmos.matrix *= Matrix4x4.Rotate(Quaternion.AngleAxis(90, Vector3.up));
                UnityEngine.Gizmos.DrawCube(Vector3.zero, new Vector3(radius * 2, height, 0));
                Handles.DrawSolidArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, radius);
                //draw center
                Handles.DrawSolidDisc(Vector3.up * pointOffset, Vector3.up, radius);
                Handles.DrawSolidDisc(Vector3.down * pointOffset, Vector3.up, radius);
                Handles.DrawSolidDisc(Vector3.zero, Vector3.up, radius);
            }
        }

        public static void DrawCapsule(CapsuleCollider caps, Color color = default(Color))
        {
            if (caps == null) return;
            DrawCapsule(caps.transform.position + caps.center, caps.transform.rotation, caps.radius,
                caps.height, color);
        }
        
        public static void DrawWireframeBox(Vector3 pos, Quaternion rot, Vector3 scale, Color c)
        {
            // create matrix
            Matrix4x4 m = new Matrix4x4();
            m.SetTRS(pos, rot, scale);

            var point1 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0.5f));
            var point2 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, 0.5f));
            var point3 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, -0.5f));
            var point4 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, -0.5f));

            var point5 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0.5f));
            var point6 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, 0.5f));
            var point7 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, -0.5f));
            var point8 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, -0.5f));

            Debug.DrawLine(point1, point2, c);
            Debug.DrawLine(point2, point3, c);
            Debug.DrawLine(point3, point4, c);
            Debug.DrawLine(point4, point1, c);

            Debug.DrawLine(point5, point6, c);
            Debug.DrawLine(point6, point7, c);
            Debug.DrawLine(point7, point8, c);
            Debug.DrawLine(point8, point5, c);

            Debug.DrawLine(point1, point5, c);
            Debug.DrawLine(point2, point6, c);
            Debug.DrawLine(point3, point7, c);
            Debug.DrawLine(point4, point8, c);
        }
    }
}