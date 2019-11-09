using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] Grid mapGrid;
    TileSpawner tileSpawner;
    int playerOffSetX = -1;
    int playerOffSetY = -1;
    int playerSightRadiusInCells;


    private void Start()
    {
        tileSpawner = gameObject.GetComponent<TileSpawner>();
        SetPlayerSightRadius();
    }

    private void SetPlayerSightRadius()
    {
        playerSightRadiusInCells = GameManager.instance.fogOfWarRadius;
    }

    public void RevealAroundCoordinate(Vector3Int coordinate)
    {
        if (tileSpawner == null)
        {
            SetPlayerSightRadius();
            tileSpawner = gameObject.GetComponent<TileSpawner>();
        }

        coordinate = new Vector3Int(coordinate.x + playerOffSetX, coordinate.y + playerOffSetY, coordinate.z);
        int circleRadiusInCells = playerSightRadiusInCells;
        int r2 = circleRadiusInCells * circleRadiusInCells;
        float circleRadius = (float)circleRadiusInCells + 0.5f;

        for (int x = -circleRadiusInCells; x <= circleRadiusInCells; x++)
        {
            int y = (int)(Mathf.Sqrt(r2 - x * x) + 0.5);
            // reveal 1st and 2nd quadrant
            RevealTile(coordinate.x + x, coordinate.y + y);
            ClearCircle(x, y, coordinate);

            // reveal 3rd and 4th quadrant
            RevealTile(coordinate.x + x, coordinate.y - y);
            ClearCircle(x, -y, coordinate);
        }

        for (int y = -circleRadiusInCells; y <= circleRadiusInCells; y++)
        {
            int x = (int)(Mathf.Sqrt(r2 - y * y) + 0.5);
            // reveal 1st and 4th quadrant
            RevealTile(coordinate.x + x, coordinate.y + y);
            ClearCircle(x, y, coordinate, true);

            // reveal 2nd and 3rd quadrant
            RevealTile(coordinate.x - x, coordinate.y + y);
            ClearCircle(-x, y, coordinate, true);
        }
    }

    /// <summary>
    /// based on this http://groups.csail.mit.edu/graphics/classes/6.837/F98/Lecture6/circle.html
    /// </summary>
    /// <param name="coordinate"></param>
    public void TestingCircle(Vector3Int coordinate)
    {

        int circleRadiusInCells = playerSightRadiusInCells;
        int r2 = circleRadiusInCells * circleRadiusInCells;
        float circleRadius = (float)circleRadiusInCells + 0.5f;

        for (int x = -circleRadiusInCells; x <= circleRadiusInCells; x++)
        {
            int y = (int)(Mathf.Sqrt(r2 - x * x) + 0.5);
            // fill 1st and 2nd quadrant
            tileSpawner.SpawnFogTile(x, y);
            FillCircle(x, y);

            // fill 3rd and 4th quadrant
            tileSpawner.SpawnFogTile(x, -y);
            FillCircle(x, -y);
        }

        for (int y = -circleRadiusInCells; y <= circleRadiusInCells; y++)
        {
            int x = (int)(Mathf.Sqrt(r2 - y * y) + 0.5);
            // fill 1st and 4th quadrant
            tileSpawner.SpawnFogTile(x,  y);
            FillCircle(x, y, true);

            // fill 2nd and 3rd quadrant
            tileSpawner.SpawnFogTile(-x, y);
            FillCircle(-x, y, true);
        }
    }

    void ClearCircle(int xBorder, int yBorder, Vector3Int center, bool yIsConstant = true)
    {
        if (yIsConstant)
        {
            if (xBorder < 0)
            {
                for (int x = xBorder; x < 0; x++)
                {
                    RevealTile(center.x + x, center.y + yBorder);
                }
            }
            else
            {
                for (int x = xBorder; x > -1; x--)
                {
                    RevealTile(center.x + x, center.y + yBorder);
                }
            }
        }
        else
        {
            if (yBorder < 0)
            {
                for (int y = yBorder; y < 0; y++)
                {
                    RevealTile(center.x + xBorder, center.y + y);
                }
            }
            else
            {
                for (int y = yBorder; y > -1; y--)
                {
                    RevealTile(center.x + xBorder, center.y + y);
                }
            }
        }
    }

    void RevealTile(int x, int y)
    {
        tileSpawner.fogOfWarMap.SetTile(new Vector3Int(x, y, 0), null);
        for (int i = 0; i < GameManager.instance.itemsOnMap.Count; i++)
        {
            Item currItem = GameManager.instance.itemsOnMap[i];
            if (currItem.gridCoordinates.x == x && currItem.gridCoordinates.y == y)
            {
                currItem.RevealOnMap();
            }
        }
    }

    void FillCircle(int xBorder, int yBorder, bool yIsConstant = true)
    {
        if (yIsConstant)
        {
            if (xBorder < 0)
            {
                for (int x = xBorder; x < 0; x++)
                {
                    tileSpawner.SpawnFogTile(x, yBorder);
                }
            }
            else
            {
                for (int x = xBorder; x > -1; x--)
                {
                    tileSpawner.SpawnFogTile(x, yBorder);
                }
            }
        }
        else
        {
            if (yBorder < 0)
            {
                for (int y = yBorder; y < 0; y++)
                {
                    tileSpawner.SpawnFogTile(xBorder, y);
                }
            }
            else
            {
                for (int y = yBorder; y > -1; y--)
                {
                    tileSpawner.SpawnFogTile(xBorder, y);
                }
            }
        }
    }
}
