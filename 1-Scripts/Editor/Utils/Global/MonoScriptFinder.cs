using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AnkleBreaker.Utils.Editor
{
    public static class MonoScriptFinder
    {
        public static MonoScript FindMonoScriptByClassName(string className)
        {
            var monoScripts = AssetDatabase.FindAssets("t:MonoScript") // Recherche tous les MonoScripts
                .Select(AssetDatabase.GUIDToAssetPath)                 // Convertit les GUID en chemins
                .Select(AssetDatabase.LoadAssetAtPath<MonoScript>)     // Charge chaque MonoScript
                .Where(script => script != null)                       // Filtre les résultats valides
                .ToList(); 

            return monoScripts.FirstOrDefault(script => script.GetClass() != null && script.GetClass().Name == className);
        }
    }
}
