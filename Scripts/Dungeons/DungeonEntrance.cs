using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    public float density;
    public float manaAmount;

    public ThemeInfo theme;

    bool generated = false;
    bool generating = false;

    Room[,] rooms;

    Room startRoom;
    Room lastroom;

    int xSize;
    int ySize;

    float cooldown = 3f;

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
        }
        else
        {
            color = DungeonImages.blue;
            nameColor = "E";
        }

        SpriteRenderer image = transform.GetComponent<SpriteRenderer>();
        if (image != null)
        {
            image.sprite = color;
        }
        else
        {
            image = transform.AddComponent<SpriteRenderer>();
            image.sprite = color;
        }

        xSize = (int)(DungeonGenerator.x * manaAmount);
        ySize = (int)(DungeonGenerator.y * manaAmount);

        xSize = Mathf.Clamp(xSize, 4, 100);
        ySize = Mathf.Clamp(ySize, 4, 100);

        xSize = Mathf.Max(xSize, ySize);
        ySize = xSize;
    }

    void handleEnter()
    {
        if (generated)
        {
            Player.player.transform.position = Calculator.addValue(startRoom.transform.position, y: 1);
            Player.player.inDungeonn = true;
        }
        else if (!generating)
        {
            StartCoroutine(GenerateDungeonCoroutine());
            generating = true;
        }
    }


    IEnumerator GenerateDungeonCoroutine()
    {

        rooms = new Room[xSize, ySize];


        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                rooms[x, y] = Instantiate(DungeonGenerator.room, transform.parent).GetComponent<Room>();
                rooms[x, y].transform.position = Calculator.addValue(transform.position, x: x * 20, y: 1000, z: y * 20);
                int index = Random.Range(0, theme.mobs.Count - 1);
                rooms[x,y].mob = theme.mobs[index];
                rooms[x,y].density = density;
            }
        }


        int startx = Random.Range(0, xSize);
        int starty = Random.Range(0, ySize);
        startRoom = rooms[startx, starty];

        yield return StartCoroutine(GenerateMazeCoroutine(startx,starty));

        generated = true;
    }

    IEnumerator GenerateMazeCoroutine(int sx, int sy)
    {

        List<Vector2> stack = new List<Vector2>();
        bool[,] visited = new bool[xSize, ySize];
        Vector2 currentCell = new Vector2(sx, sy);
        stack.Add(currentCell);
        visited[(int)currentCell.x, (int)currentCell.y] = true;
        lastroom = rooms[(int)currentCell.x, (int)currentCell.y];


        while (stack.Count > 0)
        {
            List<Vector2> unvisitedNeighbors = GetUnvisitedNeighbors(currentCell, visited);

            if (unvisitedNeighbors.Count > 0)
            {

                Vector2 neighbor = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];


                DestroyWallBetween(currentCell, neighbor);

                stack.Add(neighbor);
                visited[(int)neighbor.x, (int)neighbor.y] = true;
                currentCell = neighbor;


                lastroom = rooms[(int)currentCell.x, (int)currentCell.y];
            }
            else
            {

                currentCell = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
            }


            yield return null;
        }
        lastroom.end = true;
        lastroom.mob = theme.boss;
        
    }


    List<Vector2> GetUnvisitedNeighbors(Vector2 cell, bool[,] visited)
    {
        List<Vector2> neighbors = new List<Vector2>();

        Vector2[] directions = {
            new Vector2(1, 0), // Right
            new Vector2(-1, 0), // Left
            new Vector2(0, 1), // Up
            new Vector2(0, -1) // Down
        };

        foreach (var direction in directions)
        {
            Vector2 neighbor = cell + direction;

            if (IsInBounds(neighbor) && !visited[(int)neighbor.x, (int)neighbor.y])
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }


    bool IsInBounds(Vector2 cell)
    {
        return cell.x >= 0 && cell.x < xSize && cell.y >= 0 && cell.y < ySize;
    }


    void DestroyWallBetween(Vector2 currentCell, Vector2 neighbor)
    {

        if (neighbor.x == currentCell.x + 1) // Right
        {
            rooms[(int)currentCell.x, (int)currentCell.y].breakWall(2); // Destroy right wall of current cell
            rooms[(int)neighbor.x, (int)neighbor.y].breakWall(3); // Destroy left wall of neighbor cell
        }
        else if (neighbor.x == currentCell.x - 1) // Left
        {
            rooms[(int)currentCell.x, (int)currentCell.y].breakWall(3); // Destroy left wall of current cell
            rooms[(int)neighbor.x, (int)neighbor.y].breakWall(2); // Destroy right wall of neighbor cell
        }
        else if (neighbor.y == currentCell.y + 1) // Up
        {
            rooms[(int)currentCell.x, (int)currentCell.y].breakWall(4); // Destroy top wall of current cell
            rooms[(int)neighbor.x, (int)neighbor.y].breakWall(1); // Destroy bottom wall of neighbor cell
        }
        else if (neighbor.y == currentCell.y - 1) // Down
        {
            rooms[(int)currentCell.x, (int)currentCell.y].breakWall(1); // Destroy bottom wall of current cell
            rooms[(int)neighbor.x, (int)neighbor.y].breakWall(4); // Destroy top wall of neighbor cell
        }
    }


    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, manaAmount + transform.localRotation.eulerAngles.z);
        if (Vector3.Distance(transform.position, Player.player.transform.position) <= 2)
        {
            if (cooldown >= 1)
            {
                handleEnter();
                cooldown = 0;
            } else if (generating)
            {
                handleEnter();
            }
        }
        cooldown += Time.deltaTime;
        if (lastroom.cleared)
        {
            Player.player.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}