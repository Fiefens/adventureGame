using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaze : MonoBehaviour
{
    const int N = 1, S = 2, E = 3, W = 4;

    int[,] grid;

    [SerializeField]
    [Range(5, 100)]
    int width, height;
    [SerializeField]
    [Range(5, 100)]
    int wallSize;
    public GameObject verticalWall, horizontalWall;

    public GameObject player, objectToCollect, npc;
    void AddNPCs()
    {
        float xOffset = -(width * wallSize) / 2;
        float zOffset = -(height * wallSize) / 2;

        GameObject npc1 = (GameObject)(Instantiate(npc, new Vector3(xOffset + wallSize * 1, 0.52f, zOffset + wallSize * 1), Quaternion.identity));

        // npc1.GetComponent<ControlNPCGuard>().guardType = ControlNPCGuard.GUARD_TYPE.IDLE;
        npc1.GetComponent<ControlNPCGuard>().guardType = ControlNPCGuard.GUARD_TYPE.CHASER;
        npc1.GetComponent<ControlNPCGuard>().player = GameObject.Find("Player");
    }


    void AddPlayer()
    {
        float xOffset = -(width * wallSize) / 2;
        float zOffset = -(height * wallSize) / 2;

        GameObject p = (GameObject)(Instantiate(player, new Vector3(xOffset, 1.0f, zOffset), Quaternion.identity));
        p.name = "player";
    }

    GameObject[,] gridObjectsH, gridObjectsV;

    GameObject[] allObjectsInScene;
    void drawFullGrid()
    {
        for (int i = 0; i <= height; i++)
        {
            for (int j = 0; j <= width; j++)
            {
                if (i < height)
                {
                    float vWallSize = verticalWall.transform.localScale.z;
                    float xOffset = -(width * vWallSize) / 2;
                    float zOffset = -(height * vWallSize) / 2;

                    gridObjectsV[j, i] = (GameObject)Instantiate(
                        verticalWall,
                        new Vector3(-vWallSize / 2 + j * vWallSize + xOffset, wallSize / 2, i * vWallSize + zOffset),
                        Quaternion.identity
                    );

                    gridObjectsV[j, i].active = true;
                    gridObjectsV[j, i].name = "v" + i + j;
                    gridObjectsV[j, i].tag = "wall";
                }

                if (j < width)
                {
                    float hWallSize = horizontalWall.transform.localScale.x;
                    float xOffset = -(width * hWallSize) / 2;
                    float zOffset = -(height * hWallSize) / 2;

                    gridObjectsH[j, i] = (GameObject)Instantiate(
                        horizontalWall,
                        new Vector3(j * hWallSize + xOffset, wallSize / 2, -(hWallSize / 2) + i * hWallSize + zOffset),
                        Quaternion.identity
                    );

                    gridObjectsH[j, i].active = true;
                    gridObjectsH[j, i].name = "h" + i + j;
                    gridObjectsH[j, i].tag = "wall";
                }
            }
        }
    }
    void GenerateMazeBinary()
    {
        for (int row = 0; row < height; row++)
        {
            for (int cell = 0; cell < width; cell++)
            {
                float randomNumber = Random.Range(0, 100);
                int carvingDirection;

                if (randomNumber > 30)
                    carvingDirection = N;
                else
                    carvingDirection = E;

                if (cell == width - 1)
                {
                    if (row < height - 1)
                        carvingDirection = N;
                    else
                        carvingDirection = -1;
                }
                else if (row == height - 1)
                {
                    if (cell < width - 1)
                        carvingDirection = E;
                    else
                        carvingDirection = -1;
                }

                grid[cell, row] = carvingDirection;
            }
        }
    }

    void DisplayGrid()
    {
        for (int row = 0; row < height; row++)
        {
            for (int cell = 0; cell < width; cell++)
            {
                if (grid[cell, row] == N)
                    gridObjectsH[cell, row + 1].active = false;

                if (grid[cell, row] == E)
                    gridObjectsV[cell + 1, row].active = false;
            }
        }
    }

    float wallHeight;
    private void Init()
    {
        wallSize = Mathf.Clamp(wallSize, 5, 100);
        height = width;
        wallHeight = 4;

        height = width;
        wallHeight = 4;

        grid = new int[width, height];
        gridObjectsV = new GameObject[width + 1, height + 1];
        gridObjectsH = new GameObject[width + 1, height + 1];

        drawFullGrid();

        int sizeX = (width + 1) * wallSize;
        int sizeZ = (height + 1) * wallSize;

        GameObject ground = GameObject.Find("ground");
        if (ground)
        {
            ground.transform.localScale = new Vector3(sizeX, 1, sizeZ);
            ground.transform.position = new Vector3(0, -0.5f, 0); // optional to make it visible under walls
        }

        GameObject ceiling = GameObject.Find("ceiling");
        if (ceiling)
        {
            ceiling.transform.localScale = new Vector3(sizeX, 1, sizeZ);
            ceiling.transform.position = new Vector3(0, wallHeight + 0.5f, 0);
        }

        GameObject.Find("ceiling").transform.localScale = new Vector3((width + 1) * wallSize, 1, (height + 1) * wallSize);

        GameObject.Find("ceiling").transform.position = new Vector3(
            GameObject.Find("ceiling").transform.position.x,
            wallSize - 1,
            GameObject.Find("ceiling").transform.position.z
        );

    }


    void AddObjects()
    {
        float xOffset = -(width * wallSize) / 2;
        float zOffset = -(height * wallSize) / 2;

        GameObject object1 = (GameObject)(Instantiate(objectToCollect, new Vector3(xOffset + wallSize * 1, 1.5f, zOffset + wallSize), Quaternion.identity));
        object1.GetComponent<ObjectToBeCollected>().type = Item.ItemType.GOLD;

        GameObject object2 = (GameObject)(Instantiate(objectToCollect, new Vector3(xOffset + wallSize * 2, 1.5f, zOffset + wallSize * 2), Quaternion.identity));
        object2.GetComponent<ObjectToBeCollected>().type = Item.ItemType.RED_DIAMOND;

        GameObject object3 = (GameObject)(Instantiate(objectToCollect, new Vector3(xOffset + wallSize * 3, 1.5f, zOffset + wallSize * 3), Quaternion.identity));
        object3.GetComponent<ObjectToBeCollected>().type = Item.ItemType.BLUE_DIAMOND;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        GenerateMazeBinary();
        DisplayGrid();
        //AddPlayer();
        AddObjects();
        AddNPCs();

        float xOffset = -(width * wallSize) / 2;
        float zOffset = -(height * wallSize) / 2;

        GameObject.Find("Player").transform.position = new Vector3(xOffset + wallSize * 4, 0.52f, zOffset + wallSize * 4);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
