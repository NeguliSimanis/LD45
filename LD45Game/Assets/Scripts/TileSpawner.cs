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
    private Tile[] regularTiles;

    void Start()    
    {
        FillMapWithTiles();
    }

    public void FillMapWithTiles()
    {
        ClearMap(bottomMap);

        // terrainMap = new int[mapSizeX, mapSizeY];
        //for (int i)

        bottomMap.SetTile(new Vector3Int(0, 0, 0), regularTiles[0]);

        // fill tiles on Northwest-Southeast axis
        for (int yCoord = -1 - mapSizeY; yCoord < 1 + mapSizeY; yCoord++)
        {
            for (int xCoord = -1 - mapSizeX; xCoord < 1 + mapSizeX; xCoord++)
            {
                if (yCoord != 0 || xCoord != 0)
                {
                    bottomMap.SetTile(new Vector3Int(xCoord, yCoord, 0), GetRandomTile());
                }
            }   
        }



        // fill 
        //bottomMap.SetTile(new Vector3Int(1, -1, 0), regularTiles[1]);
    }

    private Tile GetRandomTile()
    {
        int randomTileID = Random.Range(0, regularTiles.Length);
        return regularTiles[randomTileID];
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
