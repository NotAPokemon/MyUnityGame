using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    public float density;
    public float manaAmount;

    bool generated = false;

    Room[,] rooms;

    Room startRoom;

    int xSize;
    int ySize;

    string nameColor;

    void Start()
    {
        Sprite color;

        if (density >= 0.9)
        {
            color = DungeonImages.purple;
            nameColor = "S";
        }
        else if (density >= 0.75)
        {
            color = DungeonImages.red;
            nameColor = "A";
        }
        else if (density >= 0.6)
        {
            color = DungeonImages.orange;
            nameColor = "B";
        }
        else if (density >= 0.4)
        {
            color = DungeonImages.yellow;
            nameColor = "C";
        }
        else if (density >= 0.25)
        {
            color = DungeonImages.green;
            nameColor = "D";
        } else
        {
            color = DungeonImages.blue;
            nameColor = "E";
        }


        SpriteRenderer image = transform.GetComponent<SpriteRenderer>();
        if (image != null)
        {
            image.sprite = color;
        } else
        {
            image = transform.AddComponent<SpriteRenderer>();
            image.sprite = color;
        }

        xSize = (int)(DungeonGenerator.x * manaAmount);
        ySize = (int)(DungeonGenerator.y * manaAmount);

    }



    int[] pickRandomNeighbor(int x, int y)
    {
        List<Room> neighbors = new List<Room>();
        try
        {
            if (rooms[x + 1, y].State == 0)
            {
                neighbors.Add(rooms[x + 1, y]);
            }
        }
        catch { }

        try
        {
            if (rooms[x - 1, y].State == 0)
            {
                neighbors.Add(rooms[x - 1, y]);
            }
        }
        catch { }
        try
        {

            if (rooms[x, y + 1].State == 0)
            {
                neighbors.Add(rooms[x, y + 1]);
            }
        }
        catch { }
        try
        {
            if (rooms[x, y - 1].State == 0)
            {
                neighbors.Add(rooms[x, y - 1]);
            }
        }
        catch { }


        int num = Random.Range(1, neighbors.Count + 1);




        Debug.Log(num);
        Debug.Log(neighbors.Count);

        switch (num - 1)
        {
            case 0:
                x = x + 1;
                break;
            case 1:
                x = x - 1;
                break;
            case 2:
                y = y + 1;
                break;
            case 3:
                y = y - 1;
                break;
        }
        int[] result = new int[3];
        result[0] = x;
        result[1] = y;
        result[2] = num - 1;
        return result;
    }

    void handleMazeGen()
    {
        int x = Random.Range(1, xSize + 1);
        int y = Random.Range(1, ySize + 1);

        startRoom = rooms[x - 1, y - 1];

        Room currentRoom = startRoom;

        List<Room> completed = new List<Room>();

        while (completed.Count < xSize * ySize)
        {

            int[] randomNeighbor = pickRandomNeighbor(x,y);
            
            x = randomNeighbor[0];
            y = randomNeighbor[1];


            currentRoom.breakWall(randomNeighbor[2] + 1);
            currentRoom.State = 1;

            currentRoom = rooms[x, y];
            currentRoom.breakWall(randomNeighbor[2] + 2, true);

        }

    }


    void handleMazeGen(int fakex, int fakey)
    {
        int x = Random.Range(1, fakex + 1);
        int y = Random.Range(1, fakey + 1);

        startRoom = rooms[x - 1, y - 1];

        Debug.Log(x + " " + y);

        Room currentRoom = startRoom;

        List<Room> completed = new List<Room>();

        for (int i = 0; i < 120; i++)
        {

            int[] randomNeighbor = pickRandomNeighbor(x, y);

            x = randomNeighbor[0];
            y = randomNeighbor[1];


            currentRoom.breakWall(randomNeighbor[2] + 1, true);
            currentRoom.State = 1;

            currentRoom = rooms[x, y];
            currentRoom.breakWall(randomNeighbor[2] + 2, true);

        }

    }

    public void generate()
    {
        generated = true;
        rooms = new Room[xSize, ySize];
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                rooms[x, y] = Instantiate(DungeonGenerator.room, transform).GetComponent<Room>();
                rooms[x, y].transform.localPosition = new Vector3(20 * x,1000,20 * y);
                rooms[x, y].gameObject.SetActive(true);
                //add mob
            }
        }
        handleMazeGen();
    }


    public void generate(float xValue, float yValue, GameObject room)
    {
        generated = true;
        rooms = new Room[60,60];
        for (int x = 0; x < 60; x++)
        {
            for (int y = 0; y < 60; y++)
            {
                rooms[x, y] = Instantiate(DungeonGenerator.getRoom(), transform).GetComponent<Room>();
                rooms[x, y].transform.localPosition = new Vector3(20 * x, 1000, 20 * y);
                rooms[x, y].gameObject.SetActive(true);
            }
        }
        handleMazeGen(60,60);
    }

    void handleEnter()
    {
        Debug.Log(nameColor);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, manaAmount + transform.localRotation.eulerAngles.z);
        if (Vector3.Distance(transform.position, Player.player.transform.position) <= 2)
        {
            handleEnter();
        }
    }
}
