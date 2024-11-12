using System;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public EntitySpawnData[] spawnDatas;
    float lastSpawnTime = 0;
    public Player player;
    public Material hurtMatPublic;
    public static Material hurtMat;


    private void Start()
    {
        hurtMat = hurtMatPublic;
    }

    private void Update()
    {
        if (lastSpawnTime >= 1)
        {
            Spawn();
        } else
        {
            lastSpawnTime += Time.deltaTime;
        }
    }

    

    public void Spawn()
    {
        lastSpawnTime = 0;
        for (int i = 0; i < spawnDatas.Length; i++)
        {
            for (int j = 0; j < spawnDatas[i].spawnAmount; j++)
            {
                bool cahnce = Calculator.ChanceOf(spawnDatas[i].spawnRate);
                if (cahnce)
                {
                    GameObject newEntityClone = Instantiate(spawnDatas[i].entity.gameObject);
                    BaseEntity newEntity = newEntityClone.GetComponent<BaseEntity>();
                    float posY = spawnDatas[i].maxSpawnHeight;
                    Vector3 newPos = Calculator.getPosInChunk(posY, spawnDatas[i].spawnRange);
                    newEntity.transform.position = newPos;
                    if (Calculator.IsLayerAbove(newEntity.gameObject))
                    {
                        newPos.x += UnityEngine.Random.Range(0, 1) * 20f;
                        newPos.z += UnityEngine.Random.Range(0, 1) * 20f;
                    }
                    newPos.y = posY - (Calculator.GetDistanceToLayerBelow(newEntity.gameObject) - 1);
                    newEntity.transform.position = newPos;
                    newEntity.gameObject.SetActive(true);
                    if (newEntity is Mob)
                    {
                        ((Mob)newEntity).handleSpawn();
                    }
                    newEntity.transform.SetParent(transform, true);

                }
            }
        }
    }

    

}

[Serializable]
public class EntitySpawnData
{
    public BaseEntity entity;
    public float spawnRate;
    public float spawnAmount = 1f;
    public float maxSpawnHeight;
    public bool active = true;
    public float spawnRange = 150f;
}
