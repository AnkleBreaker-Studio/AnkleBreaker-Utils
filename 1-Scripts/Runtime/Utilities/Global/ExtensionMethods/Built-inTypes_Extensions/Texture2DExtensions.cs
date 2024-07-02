using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class Texture2DExtensions
    {
        public static Texture2D CopyTexture(this Texture2D originalTexture)
        {
            if (originalTexture == null)
            {
                Debug.LogError("No texture to copy!");
                return null;
            }

            RenderTexture renderTexture = new RenderTexture(originalTexture.width, originalTexture.height, 0,
                RenderTextureFormat.ARGB32);
            Graphics.Blit(originalTexture, renderTexture);

            Texture2D copyTexture = new Texture2D(originalTexture.width, originalTexture.height);
            RenderTexture.active = renderTexture;

            copyTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

            RenderTexture.active = null;

            copyTexture.Apply();

            renderTexture.Release();

            return copyTexture;
        }
    }
}