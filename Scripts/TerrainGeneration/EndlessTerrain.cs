using System;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{

    const float vmtfcu = 25f;
    const float vmtfcuSqr = vmtfcu * vmtfcu;


    public LODInfo[] levels;
    public static float maxViewDistance;

    public Transform viwer;
    public Material mapMat;

    static MapGen mapGen;

    public static Vector2 viewerPosistion;
    Vector2 viewerPosistionOld;
    int chunkSize;
    int chuncksVisibleInViewDist;

    Dictionary<Vector2, TerrainChunck> terrainChunckDict = new Dictionary<Vector2, TerrainChunck>();
    static List<TerrainChunck> terrainChuncksLast = new List<TerrainChunck>();

    void Start()
    {
        maxViewDistance = levels[levels.Length - 1].visibleDistMargin;
        mapGen = FindObjectOfType<MapGen>();
        chunkSize = MapGen.mapChunckSize - 1;
        chuncksVisibleInViewDist = Mathf.RoundToInt(maxViewDistance / chunkSize);
        UpdateVisibleChunks();
    }

    void Update()
    {
        viewerPosistion = new Vector2(viwer.position.x, viwer.position.z) / mapGen.terrainData.uniformScale;

        if((viewerPosistionOld-viewerPosistion).sqrMagnitude > vmtfcuSqr)
        {
            viewerPosistionOld = viewerPosistion;
            UpdateVisibleChunks();
        }
    }


    


    void UpdateVisibleChunks()
    {
        for (int i = 0; i < terrainChuncksLast.Count; i++)
        {
            terrainChuncksLast[i].setVisible(false);
        }
        terrainChuncksLast.Clear();

        int currentChunckCoordX = Mathf.RoundToInt(viewerPosistion.x/chunkSize);
        int currentChunckCoordY = Mathf.RoundToInt(viewerPosistion.y / chunkSize);
        for (int yOffset = -chuncksVisibleInViewDist;  yOffset <= chuncksVisibleInViewDist; yOffset++)
        {
            for (int xOffset = -chuncksVisibleInViewDist; xOffset <= chuncksVisibleInViewDist; xOffset++)
            {
                Vector2 veiwedChunkCoord = new Vector2(currentChunckCoordX + xOffset, currentChunckCoordY + yOffset);

                if (terrainChunckDict.ContainsKey(veiwedChunkCoord))
                {
                    terrainChunckDict[veiwedChunkCoord].UpdateTerrain();
                }
                else
                {
                    terrainChunckDict.Add(veiwedChunkCoord, new TerrainChunck(veiwedChunkCoord, chunkSize, levels, transform, mapMat));
                }

            }
        }

    }

    public class TerrainChunck
    {

        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        MeshCollider collider;

        LODInfo[] levels;
        LODMesh[] meshes;

        LODMesh col;

        MapData mapData;
        bool mapDataRecived;
        int previousLODIndex = -1;

        public TerrainChunck(Vector2 coord, int size, LODInfo[] levels, Transform parent, Material mat)
        {
            this.levels = levels;
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);

            Vector3 posV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            collider = meshObject.AddComponent<MeshCollider>();
            meshObject.layer = 3;
            meshRenderer.material = mat;
            meshObject.transform.position = posV3 * mapGen.terrainData.uniformScale;
            meshObject.transform.parent = parent;
            meshObject.transform.localScale = Vector3.one * mapGen.terrainData.uniformScale;
            setVisible(false);

            meshes = new LODMesh[levels.Length];
            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i] = new LODMesh(levels[i].lod, UpdateTerrain);
                if (levels[i].useForCollider)
                {
                    col = meshes[i];
                }
            }


            mapGen.RequestMapData(position, OnMapDataRecived);
        }


        void OnMapDataRecived(MapData mapData)
        {
            this.mapData = mapData;
            mapDataRecived = true;

            

            UpdateTerrain();
        }

        public void UpdateTerrain()
        {
            if (mapDataRecived)
            {
                float viewerDistFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosistion));
                bool visible = viewerDistFromNearestEdge <= maxViewDistance;
                if (visible)
                {
                    int lodIndex = 0;
                    for (int i = 0; i < levels.Length - 1; i++)
                    {
                        if (viewerDistFromNearestEdge > levels[i].visibleDistMargin)
                        {
                            lodIndex = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (previousLODIndex != lodIndex)
                    {
                        LODMesh lodMesh = meshes[lodIndex];
                        if (lodMesh.hasMesh)
                        {
                            meshFilter.mesh = lodMesh.mesh;
                            previousLODIndex = lodIndex;
                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RequestMesh(mapData);
                        }
                    }

                    if (lodIndex == 0)
                    {
                        if (col.hasMesh)
                        {
                            collider.sharedMesh = col.mesh;
                        }
                        else if (!col.hasRequestedMesh)
                        {
                            col.RequestMesh(mapData);
                        }
                    }

                    terrainChuncksLast.Add(this);

                }

                setVisible(visible);

            }
        }

        public void setVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool isVisible()
        {
            return meshObject.activeSelf;
        }

    }

    class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        Action updateCallback;

        public LODMesh(int lod, Action updateCallback)
        {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }

        void OnMeshDataRecived(MeshData mesData)
        {
            mesh = mesData.createMesh();
            hasMesh = true;

            updateCallback();
        }

        public void RequestMesh(MapData mapData)
        {
            hasRequestedMesh = true;
            mapGen.RequestMeshData(mapData, lod, OnMeshDataRecived);
        }

    }


    [System.Serializable]
    public struct LODInfo
    {
        public int lod;
        public int visibleDistMargin;
        public bool useForCollider;
    }

}
