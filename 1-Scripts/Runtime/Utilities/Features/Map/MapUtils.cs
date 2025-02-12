using UnityEngine;

namespace AnkleBreaker.Utils.Map
{
    public static class MapUtils
    {
        /// <summary>
        /// Calculate the size and center of the map from terrains in the scene.
        /// WARNING: This solution might not work correctly if your terrains are not set up in a grid.
        /// </summary>
        public static void CalculateSizeAndCenterFromTerrainsInScene(out Vector2 size, out Vector2 center)
        {
            CalculateSizeAndCenterFromTerrainsInScene(out size, out Vector3 vector3Center, out float _);
            
            center = new Vector2(vector3Center.x, vector3Center.z);
        }
        
        /// <summary>
        /// Calculate the size and center of the map from terrains in the scene.
        /// WARNING: This solution might not work correctly if your terrains are not set up in a grid.
        /// </summary>
        public static void CalculateSizeAndCenterFromTerrainsInScene(out Vector2 size, out Vector3 center,out float height)
        {
            // Find all terrains in the scene
            Terrain[] terrains = Object.FindObjectsByType<Terrain>(FindObjectsInactive.Include,FindObjectsSortMode.None);

            float minX = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxZ = float.MinValue;
            float maxHeight = float.MinValue;
            float minHeight = float.MaxValue;

            foreach (Terrain terrain in terrains)
            {
                Vector3 position = terrain.transform.position;
                Vector3 localSize = terrain.terrainData.size;

                minX = Mathf.Min(minX, position.x);
                minZ = Mathf.Min(minZ, position.z);
                maxX = Mathf.Max(maxX, position.x + localSize.x);
                maxZ = Mathf.Max(maxZ, position.z + localSize.z);
                maxHeight = Mathf.Max(maxHeight, position.y + localSize.y);
                minHeight = Mathf.Min(minHeight, position.y);
            }

            float mapWidth = maxX - minX;
            float mapLength = maxZ - minZ;
            height = maxHeight - minHeight;

            size = new Vector2(mapWidth, mapLength);
            center = new Vector3(minX + mapWidth / 2,minHeight + height / 2, minZ + mapLength / 2);
        }
    }
}