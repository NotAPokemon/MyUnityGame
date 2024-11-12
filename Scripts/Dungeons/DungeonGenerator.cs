using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    float lastSpawnTime = 0;



    void Spawn()
    {
        bool chance = Calculator.ChanceOf(120);
        if (chance)
        {
            GameObject newGate = new GameObject("gate");
            Vector3 pos = Calculator.getPosInChunk(200,200);
            DungeonEntrance info = newGate.AddComponent<DungeonEntrance>();
            info.density = Mathf.Pow(Random.value,2);
            info.manaAmount = Random.Range(1, 1000) * info.density;
            newGate.transform.localPosition = pos;
            if (Calculator.IsLayerAbove(newGate.gameObject))
            {
                pos.x += Random.Range(0, 1) * 20f;
                pos.z += Random.Range(0, 1) * 20f;
            }
            pos.y = 200 - (Calculator.GetDistanceToLayerBelow(newGate) - 2);
            newGate.transform.position = pos;
            newGate.gameObject.SetActive(true);
            newGate.transform.parent = transform;
        }
    }

    void Update()
    {
        lastSpawnTime += Time.deltaTime;
        if (lastSpawnTime >= 1)
        {
            lastSpawnTime = 0;
            Spawn();
        }
    }
}
