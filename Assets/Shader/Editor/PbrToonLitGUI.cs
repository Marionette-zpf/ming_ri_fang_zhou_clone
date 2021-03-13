using System;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;


namespace UnityEditor.Rendering.Custom.ShaderGUI
{
    public class PbrToonLitGUI : BaseLitShaderGUI
    {
        private static GUIContent g_rampMap = new GUIContent("Ramp Map");
        private static GUIContent g_specularRampMap = new GUIContent("Specular Ramp Map");
        private MaterialProperty m_rampMapProp;
        private MaterialProperty m_specularRampMapProp;

        public override void FindProperties(MaterialProperty[] properties)
        {
            base.FindProperties(properties);
            m_rampMapProp = FindProperty("_RampMap", properties, false);
            m_specularRampMapProp = FindProperty("_SpecularRampMap", properties, false);
        }


        public override void MaterialChanged(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            SetMaterialKeywords(material, LitGUI.SetMaterialKeywords);
        }

        public override void DrawSurfaceInputs(Material material)
        {
            materialEditor.TexturePropertySingleLine(g_rampMap, m_rampMapProp);
            materialEditor.TexturePropertySingleLine(g_specularRampMap, m_specularRampMapProp);
            base.DrawSurfaceInputs(material);
        }
        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            if (material.HasProperty("_RampMap"))
            {
                material.SetTexture("_RampMap", material.GetTexture("_RampMap"));
            }
        }
    }
}
