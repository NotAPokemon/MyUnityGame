using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EntityManager : MonoBehaviour
{
    public EntitySpawnData[] spawnDatas;
    float lastSpawnTime = 0;
    public Player player;


    private void Update()
    {
        if (lastSpawnTime >= 1)
        {
            Spawn();
        } else
        {
            lastSpawnTime = Time.time;
        }
    }

    private bool IsLayerAbove(BaseEntity entity)
    {
        Vector3 origin = entity.transform.position;
        Vector3 direction = Vector3.up;
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity, entity.ground))
        {

            return true;
        }

        return false;
    }


    private float GetDistanceToLayerBelow(BaseEntity entity)
    {
        Vector3 origin = entity.transform.position;
        Vector3 direction = Vector3.down;

        RaycastHit hit;
        Physics.Raycast(origin, direction, out hit, Mathf.Infinity, entity.ground);
        float distance = Vector3.Distance(origin, hit.point);
        return distance;
    }


    private Vector3 getPosInChunk(float yValue, float spawnRange)
    {
        float playerx = player.transform.position.x;
        float playerz = player.transform.position.z;
        float y = yValue;
        float x = UnityEngine.Random.Range(playerx - spawnRange/2f, playerx + spawnRange/2f);
        float z = UnityEngine.Random.Range(playerz - spawnRange/2f, playerz + spawnRange/2f);
        return new Vector3(x, y, z);

    }


    public void Spawn()
    {
        lastSpawnTime = 0;
        for (int i = 0; i < spawnDatas.Length; i++)
        {
            for (int j = 0; j < spawnDatas[i].spawnAmount; j++)
            {
                bool cahnce = ChanceOf(spawnDatas[i].spawnRate);
                if (cahnce)
                {
                    GameObject newEntityClone = Instantiate(spawnDatas[i].entity.gameObject);
                    BaseEntity newEntity = newEntityClone.GetComponent<BaseEntity>();
                    float posY = spawnDatas[i].maxSpawnHeight;
                    Vector3 newPos = getPosInChunk(posY, spawnDatas[i].spawnRange);
                    newEntity.transform.position = newPos;
                    if (IsLayerAbove(newEntity))
                    {
                        newPos.x += UnityEngine.Random.Range(0, 1) * 20f;
                        newPos.z += UnityEngine.Random.Range(0, 1) * 20f;
                    }
                    newPos.y = posY - (GetDistanceToLayerBelow(newEntity) - 1);
                    newEntity.transform.position = newPos;
                    newEntity.gameObject.SetActive(true);
                    newEntity.transform.SetParent(transform, true);

                }
            }
        }
    }

    public static bool ChanceOf(float chanceDenominator)
    {

        return UnityEngine.Random.value <= (1f / chanceDenominator);
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
