using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Mob mob;
    public int State = 0;
    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject wall4;
    public bool cleared = false;
    public bool end;
    public float density;
    public int spawnAmount;
    int spawned;
    float cd = 1;
    public List<Mob> entities;
    public ThemeInfo theme;


    private void Start()
    {
        if (end)
        {
            spawnAmount = 1;
        } else
        {
            spawnAmount = Mathf.Clamp((int) Random.Range(2, 50 * density), 2, 100);
        }
        entities.Add(mob);
        Debug.Log((int)density * 25);
        entities[entities.Count - 1].level = Mathf.Clamp(Random.Range((int)density * 25, (int)density * 100), 1, (int)density * 100);
        mob = entities[entities.Count - 1];
        entities = new List<Mob>();
        
    }



    public void breakWall(int num)
    {
        switch (num)
        {
            case 2: Destroy(wall1); break;
            case 3: Destroy(wall2); break;
            case 4: Destroy(wall3); break;
            case 5: Destroy(wall4); break;
            case 6: Destroy(wall1); break;
            case 7: Destroy(wall2); break;
            case 8: Destroy(wall3); break;
            case 1: Destroy(wall4); break;
        }
    }


    public void breakWall(int num, bool editor)
    {
        switch (num)
        {
            case 2: DestroyImmediate(wall1); break;
            case 3: DestroyImmediate(wall2); break;
            case 4: DestroyImmediate(wall3); break;
            case 5: DestroyImmediate(wall4); break;
            case 6: DestroyImmediate(wall1); break;
            case 7: DestroyImmediate(wall2); break;
            case 8: DestroyImmediate(wall3); break;
            case 1: DestroyImmediate(wall4); break;
        }
    }


    private void Update()
    {
        if (Vector3.Distance(Player.player.transform.position, transform.position) <= 50)
        {
            if (cd >= 1 && spawned < spawnAmount)
            {
                Mob mob1 = Instantiate(mob, transform, true);
                entities.Add(mob1);
                mob1.transform.localPosition = Vector3.zero;
                mob1.gameObject.SetActive(true);
                mob1.handleSpawn();
                spawned++;
            }
            cd += Time.deltaTime;
            if (entities.Count < 1)
            {
                cleared = true;
            }
        }
        
    }

}
