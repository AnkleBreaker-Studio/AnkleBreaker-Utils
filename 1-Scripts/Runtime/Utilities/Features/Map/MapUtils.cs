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
            // Find all terrains in the scene
            Terrain[] terrains = Object.FindObjectsOfType<Terrain>(true);

            float minX = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxZ = float.MinValue;

            foreach (Terrain terrain in terrains)
            {
                Vector3 position = terrain.transform.position;
                Vector3 localSize = terrain.terrainData.size;

                minX = Mathf.Min(minX, position.x);
                minZ = Mathf.Min(minZ, position.z);
                maxX = Mathf.Max(maxX, position.x + localSize.x);
                maxZ = Mathf.Max(maxZ, position.z + localSize.z);
            }

            float mapWidth = maxX - minX;
            float mapLength = maxZ - minZ;

            size = new Vector2(mapWidth, mapLength);
            center = new Vector2(minX + mapWidth / 2, minZ + mapLength / 2);
        }
    }
}