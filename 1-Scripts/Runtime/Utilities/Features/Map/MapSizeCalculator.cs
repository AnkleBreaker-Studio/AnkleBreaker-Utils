using UnityEngine;

namespace AnkleBreaker.Utils.Map
{
    public static class MapSizeCalculator
    {
        /// <summary>
        /// Calculate the size of the map from terrains in scene
        /// WARNING : This solution will probably not work correctly if your terrains are not setup in grid
        /// </summary>
        public static Vector2 CalculateSizeFromTerrainsInScene()
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
                Vector3 size = terrain.terrainData.size;

                minX = Mathf.Min(minX, position.x);
                minZ = Mathf.Min(minZ, position.z);
                maxX = Mathf.Max(maxX, position.x + size.x);
                maxZ = Mathf.Max(maxZ, position.z + size.z);
            }

            float mapWidth = maxX - minX;
            float mapLength = maxZ - minZ;

            return new Vector2(mapWidth, mapLength);
        }
    }
}
