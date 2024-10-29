using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer plane;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawTexture(Texture2D texture)
    {
       
        plane.sharedMaterial.mainTexture = texture;
        plane.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }
    public void DrawMesh(MeshData mesh)
    {
        meshFilter.sharedMesh = mesh.createMesh();

        meshFilter.transform.localScale = Vector3.one * FindObjectOfType<MapGen>().terrainData.uniformScale;
    }
}
