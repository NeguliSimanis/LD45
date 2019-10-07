using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class TileSpawner : MonoBehaviour
{
    private bool roadReachedEndOfMap = false;
    private int mapSizeX;
    private int mapSizeY;
    private int[,] terrainMap; // array representation of the map

    [SerializeField]
    private Tilemap bottomMap;
    [SerializeField]
    private Tilemap roadMap;
    [SerializeField]
    private Tile[] simpleGroundTiles;
    [SerializeField]
    private Tile[] regularGroundTiles;
    [SerializeField]
    private Tile[] roadTiles;

    #region OBSTACLES
    [Header("OBSTACLES")]
    [SerializeField]
    private Tilemap obstacleTilemap;
    [SerializeField]
    private Tile invisibleBorderTile;
    #endregion

    #region GENOMES 
    [Header("GENOMES")]
    [SerializeField]
    private int minGenomeCount = 2;
    [SerializeField]
    private int maxGenomeCount = 9;
    [SerializeField]
    private int minGenomeSize = 3;
    [SerializeField]
    private int maxGenomeSize = 10;
    [SerializeField]
    private Tilemap genomeTilemap;
    [SerializeField]
    private Tile[] genomeDefaultTiles;
    [SerializeField]
    private Tile[] genome1Tiles;
    [SerializeField]
    private Tile[] genom2Tiles;
    List<TileCoordinates> nearbyTileCoordinates;
    #endregion

    #region TILE RULES
    float chanceToSpawnDefaultTile = 0.3f;
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
        CreateBackground();
        CreateGenome1();
        CreateRoad();
        CreateLevelBorder();
    }

    private void CreateLevelBorder()
    {
        for (int yCoord = -2 - mapSizeY; yCoord < 2 + mapSizeY; yCoord++)
        {
            for (int xCoord = -2 - mapSizeX; xCoord < 2 + mapSizeX; xCoord++)
            {
                if (xCoord == -2 - mapSizeX || xCoord == 1 + mapSizeX || yCoord == -2 - mapSizeY || yCoord == 1 + mapSizeY)
                    obstacleTilemap.SetTile(new Vector3Int(xCoord, yCoord, 0), invisibleBorderTile);
            }
        }
    }



    private void CreateBackground()
    {
        for (int yCoord = -1 - mapSizeY; yCoord < 1 + mapSizeY; yCoord++)
        {
            for (int xCoord = -1 - mapSizeX; xCoord < 1 + mapSizeX; xCoord++)
            {
                bottomMap.SetTile(new Vector3Int(xCoord, yCoord, 0), GetRandomBaseTile());
            }
        }
    }

    private void CreateGenome1()
    {
        int genomeCount = Random.Range(minGenomeCount, maxGenomeCount);

        if (genomeCount > 0)
        {
            for (int i = 0; i < genomeCount; i++)
            {
                // find first genome coordinate
                int initialX = Random.Range(-GameManager.instance.mapSizeX, GameManager.instance.mapSizeX);
                int initialY = Random.Range(-GameManager.instance.mapSizeY, GameManager.instance.mapSizeY);
                SpawnTileOnGenomeMap(initialX, initialY);
                nearbyTileCoordinates = new List<TileCoordinates>();
                AddNearbyTiles(initialX, initialY);

                // roll size of genome
                int genomeSize = Random.Range(minGenomeSize, maxGenomeSize);
                for (int n = 1; n < genomeSize; n++)
                {
                    int currentNeighborID = Random.Range(0, nearbyTileCoordinates.Count);
                    SpawnTileOnGenomeMap(nearbyTileCoordinates[currentNeighborID].x, nearbyTileCoordinates[currentNeighborID].y);
                    AddNearbyTiles(nearbyTileCoordinates[currentNeighborID].x, nearbyTileCoordinates[currentNeighborID].y);
                }
                
            }
        }       
    }

    private void AddNearbyTiles(int x, int y)
    {
        if (x + 1 <= GameManager.instance.mapSizeX)
        {
            TileCoordinates neighbourTile1 = new TileCoordinates();
            neighbourTile1.x = x + 1;
            neighbourTile1.y = y;
            if (!CheckIfTileNotInNeighborList(neighbourTile1))
                nearbyTileCoordinates.Add(neighbourTile1);
        }
        if (x - 1 >= -GameManager.instance.mapSizeX)
        {
            TileCoordinates neighbourTile1 = new TileCoordinates();
            neighbourTile1.x = x - 1;
            neighbourTile1.y = y;
            if (!CheckIfTileNotInNeighborList(neighbourTile1))
                nearbyTileCoordinates.Add(neighbourTile1);
        }
        if (y + 1 <= GameManager.instance.mapSizeY)
        {
            TileCoordinates neighbourTile1 = new TileCoordinates();
            neighbourTile1.x = x;
            neighbourTile1.y = y + 1;
            if (!CheckIfTileNotInNeighborList(neighbourTile1))
                nearbyTileCoordinates.Add(neighbourTile1);
        }
        if (y - 1 >= -GameManager.instance.mapSizeY)
        {
            TileCoordinates neighbourTile1 = new TileCoordinates();
            neighbourTile1.x = x;
            neighbourTile1.y = y - 1;
            if (!CheckIfTileNotInNeighborList(neighbourTile1))
                nearbyTileCoordinates.Add(neighbourTile1);
        }
    }

    private bool CheckIfTileNotInNeighborList(TileCoordinates coordinates)
    {
        for (int i = 0; i < nearbyTileCoordinates.Count; i++)
        {
            if (nearbyTileCoordinates[i].x == coordinates.x && nearbyTileCoordinates[i].y == coordinates.y)
                return true;
        }
        return false;
    }

    private Tile GetRandomBaseTile()
    {
        bool spawnDefaultTile = false;
        Tile randomTile;
        int randomTileID;
        if (Random.Range(0f,1f) < chanceToSpawnDefaultTile)
        {
            spawnDefaultTile = true;
        }
        if (spawnDefaultTile)
        {
           randomTileID = Random.Range(0, simpleGroundTiles.Length);
           randomTile = simpleGroundTiles[randomTileID];

        }
        else
        {
            randomTileID = Random.Range(0, regularGroundTiles.Length);
            randomTile = regularGroundTiles[randomTileID];
        }
        return randomTile;
    }

    private void SpawnTileOnGenomeMap(int x, int y)
    {
        Tile tile;
        if (Random.Range(0f, 1f) < chanceToSpawnDefaultTile)
        {
            tile = genomeDefaultTiles[Random.Range(0, genomeDefaultTiles.Length)];
        }
        else
        {   
            tile = genome1Tiles[Random.Range(0, genome1Tiles.Length)];
        }
        bottomMap.SetTile(new Vector3Int(x, y, 0), tile);
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
        int roadLength = 24450;
        tileID = 0;
        yIncreased = false;
        yDecreased = false;
        distanceFromLastRoadCurve = 0;

        // last two tiles
        SpawnLastRoadTile();
        SpawnRoadTileOnSmallerX();

        // rest of the road
        int i = 0;
        while (!roadReachedEndOfMap)
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
            i++;
                
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
        Tile roadTile = roadTiles[Random.Range(0, roadTiles.Length)];
        if (x < -GameManager.instance.mapSizeX)
        {
            roadReachedEndOfMap = true;
            SpawnPlayerAtEndOfRoad(x,y);
        }
        roadMap.SetTile(new Vector3Int(x,y,0), roadTile);
        DestroyBaseTile(x, y);
    }

    void SpawnPlayerAtEndOfRoad(int x, int y)
    {
        PlayerController player = GameManager.instance.playerController;
        CameraFolow cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFolow>();
        Vector3 tempPlayerCoordinates = roadMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
        Vector3 newPlayerCoordinates = new Vector3(tempPlayerCoordinates.x + 1f, tempPlayerCoordinates.y + 0.5f, tempPlayerCoordinates.z);

        cameraFollow.gameObject.transform.position = new Vector3(newPlayerCoordinates.x, newPlayerCoordinates.y + 0.66f, -10f);


       // cameraFollow.InitializeCamera();
        player.gameObject.transform.position = newPlayerCoordinates;
        GameManager.instance.playerEyesWork = true;

    }


    #endregion
    private void DestroyBaseTile(int x, int y)
    {
        bottomMap.SetTile(new Vector3Int(x,y,0),null);
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
