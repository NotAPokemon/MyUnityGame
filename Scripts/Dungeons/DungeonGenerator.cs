using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    float lastSpawnTime = 0;
    public float xSize = 0.5f;
    public float ySize = 0.5f;
    public GameObject roomPrefab;
    public static float y;
    public static float x;
    public static GameObject room;


    void Spawn()
    {
        bool chance = Calculator.ChanceOf(120);
        if (chance)
        {
            GameObject newGate = new GameObject("gate");
            Vector3 pos = Calculator.getPosInChunk(200,200);
            DungeonEntrance info = newGate.AddComponent<DungeonEntrance>();
            info.density = Mathf.Pow(Random.value,2);
            info.manaAmount = Calculator.randomDiv(1,1000) * info.density;
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



    public static GameObject getRoom()
    {
        return FindObjectOfType<DungeonGenerator>().roomPrefab;
    }

    void Update()
    {
        x = xSize;
        y = ySize;
        room = roomPrefab;
        lastSpawnTime += Time.deltaTime;
        if (lastSpawnTime >= 1)
        {
            lastSpawnTime = 0;
            Spawn();
        }
    }
}
