using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AnkleBreaker.Utils.ObjectFinder
{
    public static class ObjectFinder
    {
        public static IEnumerable<Component> FindAll(Type type, bool includeInactive = false)
        {
            return FindAll(new List<Type>() { type }, includeInactive);
        }
        
        public static IEnumerable<Component> FindAll(List<Type> types, bool includeInactive = false) 
        {
            Scene currentScene = SceneManager.GetActiveScene();
            
            GameObject[] rootObjects = currentScene.GetRootGameObjects();

            List<Component> allComponents = new List<Component>();
            
            foreach (GameObject rootObject in rootObjects)
            {
                foreach (Type type in types)
                {
                    Component[] localComponents = rootObject.transform.GetComponentsInChildren(type, includeInactive);
                
                    foreach (var component in localComponents) 
                    {
                        if (includeInactive) 
                        {
                            if (component == null)
                            {
                                continue;
                            }
                            
                            allComponents.Add(component);
                        }
                    }
                }
            }
            
            return allComponents;
        }
    }
}