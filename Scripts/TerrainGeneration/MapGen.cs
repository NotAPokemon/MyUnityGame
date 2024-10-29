using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class MapGen : MonoBehaviour
{

    public enum DrawMode {NoiseMap, Mesh, FalloffMap};
    public DrawMode drawMode;

    public TerrainData terrainData;
    public NoiseData noiseData;
    public TextureData textureData;

    public bool shadeFlat = false;

    public static int mapChunckSize;

    public Material terrainMat;


    [Range(0,6)]
    public int levelOfDetailPre;

    

    public bool autoUpdate;


    float[,] falloff;


    Queue<MapThreadInfo<MapData>> mapDataThreadQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadQueue = new Queue<MapThreadInfo<MeshData>>();


    void Awake()
    {
        textureData.ApplyToMaterial(terrainMat);
        textureData.UpdateMeshHeights(terrainMat, terrainData.minHeight, terrainData.maxHeight);
        falloff = FalloffGenerator.GenerateFalloffMap(mapChunckSize);
        mapChunckSize = shadeFlat ? 95 : 239;
    }

    void onValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }

    void OnTextureValuesUpdated()
    {
        textureData.ApplyToMaterial(terrainMat);
    }

    public void DrawMapInEditor()
    {
        textureData.UpdateMeshHeights(terrainMat, terrainData.minHeight, terrainData.maxHeight);
        MapData mapData = genMapData(Vector2.zero);

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.hMap));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.hMap, terrainData.MeshHeightMultiplyer, terrainData.meshcurve, levelOfDetailPre, shadeFlat));

        } else if (drawMode == DrawMode.FalloffMap)
        {
            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunckSize)));
        }
    }

    public void RequestMapData(Vector2 center, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(center, callback);
        };
        new Thread(threadStart).Start();
    }


    void MapDataThread(Vector2 center, Action<MapData> callback)
    {
        MapData mapData = genMapData(center);
        lock (mapDataThreadQueue)
        {
            mapDataThreadQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }

    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, lod, callback);
        };
        new Thread(threadStart).Start();
    }

    public void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.hMap, terrainData.MeshHeightMultiplyer, terrainData.meshcurve, lod, shadeFlat);
        lock (meshDataThreadQueue)
        {
            meshDataThreadQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData) );
        }
    }

    void Update()
    {
        if (mapDataThreadQueue.Count > 0)
        {
            for (int i = 0; i < mapDataThreadQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadQueue.Dequeue();
                threadInfo.callback(threadInfo.param);
            }
        }
        if (meshDataThreadQueue.Count > 0)
        {
            for (int i = 0;i < meshDataThreadQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadQueue.Dequeue();
                threadInfo.callback(threadInfo.param);
            }
        }

    }

    MapData genMapData(Vector2 center)
    {
        float[,] nostMap = Noise.GenNoiseMap(mapChunckSize + 2, mapChunckSize + 2, noiseData.seed, noiseData.scale, noiseData.octaves, noiseData.persistace, noiseData.lacunarity, center + noiseData.offset, noiseData.normalizeMode);

        Color[] cMap = new Color[mapChunckSize * mapChunckSize];

        for (int y = 0; y < mapChunckSize; y++)
        {
            for (int x = 0; x < mapChunckSize; x++)
            {

                if (terrainData.useFalloff)
                {
                    nostMap[x,y] = Mathf.Clamp(nostMap[y,x] - falloff[x,y],0,1);
                }           
            }
        }

        

        return new MapData(nostMap);

    }

    private void OnValidate()
    {
        if (terrainData != null)
        {
            terrainData.OnValuesUpdate -= onValuesUpdated;
            terrainData.OnValuesUpdate += onValuesUpdated;
        }
        if (noiseData != null)
        {
            noiseData.OnValuesUpdate -= onValuesUpdated;
            noiseData.OnValuesUpdate += onValuesUpdated;
        }
        if(textureData != null)
        {
            textureData.OnValuesUpdate -= OnTextureValuesUpdated;
            textureData.OnValuesUpdate += OnTextureValuesUpdated;
        }

        falloff = FalloffGenerator.GenerateFalloffMap(mapChunckSize);
        mapChunckSize = shadeFlat ? 95 : 239;
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T param;

        public MapThreadInfo(Action<T> callback, T param)
        {
            this.callback = callback;
            this.param = param;
        }

    }

}

public struct MapData
{
    public readonly float[,] hMap;

    public MapData(float[,] Hmap)
    {
        hMap = Hmap;
    }

}
