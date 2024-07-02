using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnkleBreaker.Utils.MaterialApplier
{
    public class MaterialApplier : IDisposable
    {
        private List<Renderer> _renderers = null;

        public MaterialApplier(List<Renderer> renderers)
        {
            _renderers = renderers;
        }

        public void ApplyMaterial(Material wishedMaterial)
        {
            if (_renderers == null) return;
            if (wishedMaterial == null) return;

            foreach (Renderer renderer in _renderers)
            {
                if (renderer == null)
                    continue;

                if (Application.isPlaying)
                {
                    // Release the previous material to prevent memory leaks
                    ReleaseMaterials(renderer.materials);
                }

                if (wishedMaterial == null || wishedMaterial == renderer.sharedMaterial)
                {
                    if(Application.isPlaying)
                        renderer.materials = renderer.sharedMaterials;
                }
                else
                {
                    Material[] materials = Application.isPlaying ? renderer.materials : renderer.sharedMaterials;
                    for (int i = 0; i < materials.Length; i++)
                        materials[i] = wishedMaterial;

                    if (Application.isPlaying)
                        renderer.materials = materials;
                    else
                        renderer.sharedMaterials = materials;
                }
            }
        }

        private void ReleaseMaterials(Material[] materials)
        {
            if (materials == null || Application.isPlaying == false)
                return;

            foreach (var material in materials)
            {
                if (material == null || material == material.shader)
                    continue;
                UnityEngine.Object.Destroy(material);
            }
        }

        public void Dispose()
        {
            if (_renderers == null)
                return;
            foreach (var renderer in _renderers)
            {
                if (renderer == null)
                    continue;
                ReleaseMaterials(renderer.materials);
            }
        }
    }
}
