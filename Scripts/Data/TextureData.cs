using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TextureData : UpdateableData
{

    public Layer[] layers;

    float savedMin;
    float savedMax;

    public void ApplyToMaterial(Material mat)
    {
        UpdateMeshHeights(mat, savedMin, savedMax);
    }

    public void UpdateMeshHeights(Material mat, float min, float max)
    {
        savedMax = max;
        savedMin = min;


        mat.SetFloat("_Base_Color_Count", layers.Length);
        for (int i = 0; i < layers.Length; i++)
        {
            mat.SetColor("_Base_Color_" + (i + 1), layers[i].tint);
            mat.SetFloat("_BaseWeight_" + (i+1), layers[i].startHeight);
            mat.SetFloat("_Base_Blend_" + (i + 1), layers[i].blendStr);
            mat.SetFloat("_Base_ColorStr_" + (i + 1), layers[i].tintStr);
            mat.SetFloat("_Base_TextureScale_" + (i + 1), layers[i].textureScale);
            mat.SetTexture("_Base_Texture_" + (i+1), layers[i].texture);
        }
        mat.SetFloat("_Min_Height", min);
        mat.SetFloat("_Max_Height", max);
    }


 

    [Serializable]
    public class Layer
    {
        public Texture texture;
        public Color tint;
        [Range(0, 1)]
        public float tintStr;
        [Range(0, 1)]
        public float startHeight;
        [Range(0, 1)]
        public float blendStr;
        public float textureScale;
    }
    
}