using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class TileSpawner : MonoBehaviour
{
    private int mapSizeX;
    private int mapSizeY;
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

    [SerializeField]
    private float straightRoadProbability = 0.1f;
    [SerializeField]
    private float additionalCurvePossibility = 0.7f;
    [SerializeField]
    private float roadCurveUpPossibility = 0.5f;
    #endregion

    public void SetupTiles()    
    {
        mapSizeX = GameManager.instance.mapSizeX;
        mapSizeY = GameManager.instance.mapSizeY;
        FillMapWithTiles();
    }

    public void FillMapWithTiles()
    {
        ClearMap(bottomMap);
        ClearMap(roadMap);

       // bottomMap.SetTile(new Vector3Int(0, 0, 0), GetRandomBaseTile());
        //bottomMap.SetTile(new Vector3Int(2, 0, 0), roadTiles[0]);

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

    private Tile GetRandomBaseTile()
    {
        int randomTileID = Random.Range(0, regularGroundTiles.Length);
        return regularGroundTiles[randomTileID];
    }


    #region ROAD SPAWNING
    int x;
    int y;
    int tileID;
    bool yIncreased;
    bool yDecreased;
    int distanceFromLastRoadCurve;
    private void CreateRoad()
    {
        int roadLength = 40;
        tileID = 0;
        yIncreased = false;
        yDecreased = false;
        distanceFromLastRoadCurve = 0;

        // last two tiles
        SpawnLastRoadTile();
        SpawnRoadTileOnSmallerX();

        // rest of the road
        for (int i = 0; i < roadLength; i++)
        {
            if (x < -mapSizeX)
                return;

            // choose keep road straight or curve (up or down)
            if (Random.Range(0f,1f) < additionalCurvePossibility && (yIncreased||yDecreased))
            {
                ChooseBetweenCurveUpOrDown();
            }
            else if (Random.Range(0f, 1f) < straightRoadProbability)
            {
                SpawnRoadTileOnSmallerX();
            }
            else
            {
                ChooseBetweenCurveUpOrDown();
            }
                
        }
        
    }

    private void ChooseBetweenCurveUpOrDown()
    {
        if (!yIncreased && !yDecreased)
        {
            SpawnRoadTileOnDifferentY(true, true);
        }
        else if (distanceFromLastRoadCurve < 2)
        {
            if (yIncreased)
                SpawnRoadTileOnDifferentY(true, false);
            else if (yDecreased)
                SpawnRoadTileOnDifferentY(false, true);
        }
        else
        {
            SpawnRoadTileOnDifferentY(true, true);
        }
    }

    private void SpawnLastRoadTile()
    {
        y = Random.Range(-mapSizeY, mapSizeY);
        x = mapSizeX;
        SpawnRoadTile(x, y);
    }


    private void SpawnRoadTileOnDifferentY(bool allowIncreaseY, bool allowDecreaseY)
    {
        if (y >= mapSizeY)
            allowIncreaseY = false;
        else if (y <= -mapSizeY)
            allowDecreaseY = false;

        if (!allowDecreaseY && !allowIncreaseY)
        {
            SpawnRoadTileOnSmallerX();
            return;
        }

        if (allowDecreaseY && allowIncreaseY)
        {
            // curve up
            if (Random.Range(0f,1f) < roadCurveUpPossibility)
            {
                y += 1;
                yIncreased = true;
                yDecreased = false;
                
            }
            // curve down
            {
                y -= 1;
                yIncreased = false;
                yDecreased = true;
            }
            
        }
        else if (!allowDecreaseY)
        {
            y += 1;
            yIncreased = true;
            yDecreased = false;

        }
        else // (!allowIncreaseY)
        {
            y -= 1;
            yIncreased = false;
            yDecreased = true;
        }
        SpawnRoadTile(x, y);
        distanceFromLastRoadCurve = 0;
    }
    
    private void SpawnRoadTileOnSmallerX()
    {
        x -= 1;
        SpawnRoadTile(x, y);
        distanceFromLastRoadCurve++;
       
    }
    
    private void SpawnRoadTile(int x, int y)
    {
        roadMap.SetTile(new Vector3Int(x,y,0), roadTiles[0]);
    }
    #endregion


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
