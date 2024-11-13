using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public BaseEntity mob;
    public int State = 0;
    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject wall4;



    public void breakWall(int num)
    {
        switch (num)
        {
            case 1: Destroy(wall1); break;
            case 2: Destroy(wall2); break;
            case 3: Destroy(wall3); break;
            case 4: Destroy(wall4); break;
            case 5: Destroy(wall1); break;
            case 6: Destroy(wall2); break;
            case 7: Destroy(wall3); break;
            case 8: Destroy(wall4); break;
        }
    }


    public void breakWall(int num, bool editor)
    {
        switch (num)
        {
            case 1: DestroyImmediate(wall1); break;
            case 2: DestroyImmediate(wall2); break;
            case 3: DestroyImmediate(wall3); break;
            case 4: DestroyImmediate(wall4); break;
            case 5: DestroyImmediate(wall1); break;
            case 6: DestroyImmediate(wall2); break;
            case 7: DestroyImmediate(wall3); break;
            case 8: DestroyImmediate(wall4); break;
        }
    }


    private void Update()
    {
        
    }

}
