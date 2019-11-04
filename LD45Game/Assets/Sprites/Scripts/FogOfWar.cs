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
    float playerSightRadius;
    //int mapRadiusX;
    //int mapRadiusY;

    private void Start()
    {
        tileSpawner = gameObject.GetComponent<TileSpawner>();
        tileSpawner.SpawnFogTiles();
        SetPlayerSightRadius();

    }

    private void SetPlayerSightRadius()
    {
        playerSightRadiusInCells = GameManager.instance.fogOfWarRadius;
        playerSightRadius = GameManager.instance.fogOfWarRadius * mapGrid.cellSize.y;
    }

    public void RevealAroundCoordinate(Vector3Int coordinate)
    {
        coordinate = new Vector3Int(coordinate.x + playerOffSetX, coordinate.y + playerOffSetY, coordinate.z);
        tileSpawner.fogOfWarMap.SetTile(new Vector3Int(coordinate.x, coordinate.y, coordinate.z), null);
  
        for (int x = -playerSightRadiusInCells; x < playerSightRadiusInCells; x++)
        {
            for (int y = -playerSightRadiusInCells; y < playerSightRadiusInCells; y++)
            {
                tileSpawner.fogOfWarMap.SetTile(new Vector3Int(coordinate.x + x, coordinate.y + y, 0), null);
            }
        }
    }
}
