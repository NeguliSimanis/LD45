using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class TileSpawner : MonoBehaviour
{
    private int mapSizeX = 7;
    private int mapSizeY = 7;
    private int[,] terrainMap; // array representation of the map

    [SerializeField]
    private Tilemap bottomMap;
    [SerializeField]
    private Tilemap roadMap;
    [SerializeField]
    private Tile[] regularGroundTiles;
    [SerializeField]
    private Tile[] roadTiles;

    #region TILE RULES
    [Header("Rules for ROAD spawning")]
    [SerializeField]
    private int minStartDistanceFromMapSide;
    #endregion

    void Start()    
    {
        FillMapWithTiles();
    }

    public void FillMapWithTiles()
    {
        ClearMap(bottomMap);
        ClearMap(roadMap);

        // terrainMap = new int[mapSizeX, mapSizeY];
        for (int yCoord = -1 - mapSizeY; yCoord < 1 + mapSizeY; yCoord++)
        {
            for (int xCoord = -1 - mapSizeX; xCoord < 1 + mapSizeX; xCoord++)
            {
                bottomMap.SetTile(new Vector3Int(xCoord, yCoord, 0), GetRandomBaseTile());
            }   
        }
        CreateRoad();
    }

    int y; // current
    int x; // current
    bool changeX = true;
    bool yIncreased = true;
    private void CreateRoad()
    {
        // choose ending tile
        y = Random.Range(-mapSizeY, mapSizeY);
        x = mapSizeX;
        SpawnRoadTile(x, y);

        // choose -1 ending tile
        x -= 1;
        SpawnRoadTile(x, y);

        // choose other tiles
        for (int roadLength = 25; roadLength > 0; roadLength--)
        {
            if (x <= -mapSizeX)
                return;
            if (changeX)
            {
                FindNextRoadTileCoordinate(true,true);
            }
            else if (yIncreased)
            {
                FindNextRoadTileCoordinate(false,true);
            }
            else
            {
                FindNextRoadTileCoordinate(true, false);
            }
        }

        // choose road
    }

    private void FindNextRoadTileCoordinate(bool allowYincrease = true, bool allowYdecrease = true)
    {
        if (Random.Range(0f, 1f) > 0.5f)
        {
            changeX = false;
        }
        else
        {
            changeX = true;
        }
        if (changeX || (!allowYdecrease && !allowYincrease))
        {
            x -= 1;
            SpawnRoadTile(x, y);
        }
        else
        {

            // y increases
            if ((Random.Range(0, 1) > 0.5f || !allowYdecrease) && y < mapSizeY)
            {
                y += 1;
                yIncreased = true;
            }
            // y decreases
            else if ((y > -mapSizeY))
            {
                y -= 1;
                yIncreased = false;
            }
            // y increases
            else
            {
                y += 1;
                yIncreased = true;
            }
            SpawnRoadTile(x, y);
        }
    }

    private void SpawnRoadTile(int x, int y)
    {
        roadMap.SetTile(new Vector3Int(x,y,0), roadTiles[0]);
    }

    private Tile GetRandomBaseTile()
    {
        int randomTileID = Random.Range(0, regularGroundTiles.Length);
        return regularGroundTiles[randomTileID];
    }



    /// <summary>
    /// removes any previously placed tiles from a tilemap
    /// </summary>
    /// <param name="mapToClear"></param>
    public void ClearMap(Tilemap mapToClear)
    {
        mapToClear.ClearAllTiles();
        //terrainMap = null;
    }
}
