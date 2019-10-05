using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] items;
    [SerializeField]
    GridLayout gridLayout;

    public void SpawnItems()
    {
        ClearOldItems();
        SpawnGoodShrooms();

    }

    private void SpawnGoodShrooms()
    {
        // choose how many items we will spawn
        int goodShroomCount = Random.Range(GameManager.instance.minGoodShroomsInLevel,
            GameManager.instance.maxGoodShroomsInLevel + 1);

        Vector3 itemOffset = items[0].GetComponent<Item>().gridToWorldOffset;
        while (goodShroomCount > 0)
        {
            int xCoordinate = FindTileCoordinate();
            int yCoordinate = FindTileCoordinate(xCoordinate);

            if (!IsTileOccuppied(new Vector2Int(xCoordinate, yCoordinate)))
            {
                GameObject goodShroom = Instantiate(items[0], new Vector3(xCoordinate, yCoordinate * 0.5f, 0) + itemOffset, Quaternion.identity);
                goodShroom.gameObject.GetComponent<Item>().gridCoordinates = new Vector2Int(xCoordinate, yCoordinate);
                GameManager.instance.goodShrooms.Add(goodShroom);
                GameManager.instance.occupiedTiles.Add(new Vector2Int(xCoordinate, yCoordinate));
            }
            goodShroomCount--;
        }
    }

    private bool IsTileOccuppied(Vector2Int xy)
    {
        bool isTileOccuppied = false;

        for (int i = 0; i < GameManager.instance.occupiedTiles.Count; i++)
        {
            if (GameManager.instance.occupiedTiles[0] == xy)
            {
                return true;
            }
        }
        return isTileOccuppied;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="xCoordinate">if 0 then return coordinate for x, if not then return y coordinate knowing that x coordinate was this big</param>
    /// <returns></returns>
    private int FindTileCoordinate(int xCoordinate = 0)
    {
        int tileCoordinate = 0;

        if (xCoordinate == 0)
        {
            tileCoordinate = Random.Range(-GameManager.instance.mapSizeX, GameManager.instance.mapSizeX);
        }
        else
        {
            tileCoordinate = Random.Range(-GameManager.instance.mapSizeY + Mathf.Abs(xCoordinate),
                GameManager.instance.mapSizeY - Mathf.Abs(xCoordinate));
        }
        return tileCoordinate;
    }
                
    private void ClearOldItems()
    {
        for (int i = 0; i < GameManager.instance.goodShrooms.Count; i++)
        {
            Destroy(GameManager.instance.goodShrooms[i]);
        }
        GameManager.instance.goodShrooms.Clear();
        GameManager.instance.occupiedTiles.Clear();
    }
}
