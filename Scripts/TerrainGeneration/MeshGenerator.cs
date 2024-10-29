using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve curve, int levelOfDetail, bool useFlatShading)
    {
        AnimationCurve Hcurve = new AnimationCurve(curve.keys);
        int simplifcation = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;

        int BorderedSize = heightMap.GetLength(0);
        int meshSize = BorderedSize -  2*simplifcation;
        int meshSizeUnsimp = BorderedSize - 2;

        float topLeftX = (meshSizeUnsimp - 1) / -2f;
        float topLeftZ = (meshSizeUnsimp - 1) / 2f;

      
        int verticiesPerLine = (BorderedSize - 1) / simplifcation + 1;

        MeshData meshData = new MeshData(verticiesPerLine, useFlatShading);
        int[,] vertexIndexMap = new int[BorderedSize, BorderedSize];
        int meshVertexIndex = 0;
        int borderedVertexIndex = -1;

        for (int y = 0; y < BorderedSize; y += simplifcation)
        {
            for (int x = 0; x < BorderedSize; x += simplifcation)
            {
                bool isBorderVertex = y == 0 || y == BorderedSize - 1 || x == 0 || x == BorderedSize - 1;

                if (isBorderVertex)
                {
                    vertexIndexMap[x, y] = borderedVertexIndex;
                    borderedVertexIndex--;
                }
                else
                {
                    vertexIndexMap[x, y] = meshVertexIndex;
                    meshVertexIndex++;
                }

            }
        }

                for (int y = 0;  y < BorderedSize; y+=simplifcation)
        {
            for (int x = 0; x < BorderedSize; x+=simplifcation)
            {
                int vertexIndex = vertexIndexMap[x, y];

                Vector2 percent = new Vector2((x - simplifcation) / (float)meshSize, (y - simplifcation) / (float)meshSize);
                float height = Hcurve.Evaluate(heightMap[x, y]) * heightMultiplier;
                Vector3 vertexPos = new Vector3( topLeftX + percent.x * meshSizeUnsimp, height , topLeftZ - percent.y * meshSizeUnsimp);

                meshData.AddVertex(vertexPos, percent, vertexIndex);
                

                if( x < BorderedSize - 1 && y < BorderedSize - 1 )
                {
                    int a = vertexIndexMap[x, y];
                    int b = vertexIndexMap[x+ simplifcation, y];
                    int c = vertexIndexMap[x, y + simplifcation];
                    int d = vertexIndexMap[x + simplifcation, y + simplifcation];
                    meshData.AddTriangle(a,d,c);
                    meshData.AddTriangle(d,a,b);
                }
            }
        }

        meshData.finalize();

        return meshData;
    }
    
}


public class MeshData
{
    Vector3[] verticies;
    int[] triangles;
    Vector2[] uvs;
    Vector3[] bakedNormals;

    Vector3[] borderVertex;
    int[] borderTriangles;

    int borderTriangleIndex;
    int triangleIndex;
    bool useFlatShading;

    public MeshData(int vertexPerLine, bool useFlatShading)
    {
        this.useFlatShading = useFlatShading;
        verticies = new Vector3[vertexPerLine * vertexPerLine];
        uvs = new Vector2[vertexPerLine * vertexPerLine];
        triangles = new int[(vertexPerLine - 1) * (vertexPerLine - 1) * 6];


        borderVertex = new Vector3[vertexPerLine * 4 + 4];
        borderTriangles = new int[vertexPerLine * 24];

    }


    public void AddVertex(Vector3 pos, Vector2 uv, int index)
    {
        if(index < 0)
        {
            borderVertex[-index -1] = pos;
        }
        else
        {
            verticies[index] = pos;
            uvs[index] = uv;
        }
    }

    public void AddTriangle(int a, int b, int c)
    {
        if( a < 0 || b < 0 || c < 0)
        {
            borderTriangles[borderTriangleIndex] = a;
            borderTriangles[borderTriangleIndex + 1] = b;
            borderTriangles[borderTriangleIndex + 2] = c;
            borderTriangleIndex++;
            borderTriangleIndex++;
            borderTriangleIndex++;
        }
        else
        {
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;
            triangleIndex++;
            triangleIndex++;
            triangleIndex++;
        }
        
    }

    Vector3[] CalculateNormals()
    {
        Vector3[] vertexNormals = new Vector3[verticies.Length];
        int triangleCount = triangles.Length/3;
        for (int i = 0; i < triangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormal(vertexIndexA, vertexIndexB, vertexIndexC);
            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;
        }

        int bordertriangleCount = borderTriangles.Length / 3;
        for (int i = 0; i < bordertriangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertexIndexA = borderTriangles[normalTriangleIndex];
            int vertexIndexB = borderTriangles[normalTriangleIndex + 1];
            int vertexIndexC = borderTriangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormal(vertexIndexA, vertexIndexB, vertexIndexC);
            if( vertexIndexA >= 0){
                vertexNormals[vertexIndexA] += triangleNormal;
            }
            if (vertexIndexB >= 0)
            {
                vertexNormals[vertexIndexB] += triangleNormal;
            }
            if (vertexIndexC >= 0)
            {
                vertexNormals[vertexIndexC] += triangleNormal;
            }
        }


        for (int i = 0; i < vertexNormals.Length;i++)
        {
            vertexNormals[i].Normalize();
        }

        return vertexNormals;

    }

    Vector3 SurfaceNormal(int a, int b,int c)
    {
        Vector3 pointA = (a < 0 ) ? borderVertex[-a-1] : verticies[a];
        Vector3 pointB = (b < 0) ? borderVertex[-b - 1] : verticies[b];
        Vector3 pointC = (c < 0) ? borderVertex[-c - 1] : verticies[c];


        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;
        return Vector3.Cross(sideAB, sideAC).normalized;
    }

    public void finalize()
    {
        if (useFlatShading)
        {
            FlatShading();
        }
        else
        {
            BakeNoemals();
        }
    }

    void BakeNoemals()
    {
        bakedNormals = CalculateNormals();
    }

    void FlatShading()
    {
        Vector3[] flatSahdedVertex = new Vector3[triangles.Length];
        Vector2[] flatShadedUvs = new Vector2[triangles.Length];

        for (int i = 0;i < triangles.Length;i++)
        {
            flatSahdedVertex[i] = verticies[triangles[i]];
            flatShadedUvs[i] = verticies[triangles[i]];
            triangles[i] = i;
        }

        verticies = flatSahdedVertex;
        uvs = flatShadedUvs;

    }

    public Mesh createMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        if (useFlatShading)
        {
            mesh.RecalculateNormals();
        }
        else
        {
            mesh.normals = bakedNormals;
        }
        return mesh;
    }

}
