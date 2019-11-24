using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject rudder;
    GameObject currentRudder;
    [SerializeField]
    GameObject compass;
    GameObject currentCompass;
    [SerializeField]
    GameObject anchor;
    GameObject currentAnchor;

    #region MUSHROOMS
    [SerializeField]
    GameObject mushroomGood;
    [SerializeField]
    GameObject mushroomBad;
    [SerializeField]
    GameObject mushroomLegendary;
    #endregion

    [SerializeField]
    GridLayout gridLayout;

    TileSpawner tileSpawner;

    public void SpawnItems()
    {
        ClearOldItems();
       // SpawnVictoryItems();
        SpawnShrooms(ItemType.mushroomGood);
        SpawnShrooms(ItemType.mushroomBad);
        SpawnShrooms(ItemType.mushroomLegendary);
        tileSpawner = gameObject.GetComponent<TileSpawner>();
        tileSpawner.SetupTiles();
    }

    private void SpawnVictoryItems()
    {
        int rudderX = FindTileCoordinate();
        int rudderY = FindTileCoordinate(rudderX);

        int compassX = 0;
        int compassY = 0;

        int anchorX = 0;
        int anchorY = 0;

        bool compassCoordFound = false;
        bool anchorCoordFound = false;

        while (!compassCoordFound)
        {
            compassX = FindTileCoordinate();
            compassY = FindTileCoordinate(compassX);

            if (compassX != rudderX || compassY != rudderY)
            {
                compassCoordFound = true;
            }

        }

        while (!anchorCoordFound)
        {
            anchorX = FindTileCoordinate();
            anchorY = FindTileCoordinate(anchorX);

            if ((anchorX != rudderX || anchorY != rudderY) && (compassX != anchorX || compassY != anchorY))
            {
                anchorCoordFound = true;
            }
        }

        // rudder
        GameObject newRudder  = Instantiate(rudder, new Vector3(rudderX, rudderY * 0.5f, 0)
            + rudder.GetComponent<Item>().gridToWorldOffset, Quaternion.identity);
        newRudder.gameObject.GetComponent<Item>().gridCoordinates = new Vector2Int(rudderX, rudderY);

        Vector3Int gridCoordinateRudder = gridLayout.WorldToCell(new Vector3(rudderX, rudderY, 0));
        GameManager.instance.occupiedTiles.Add(new Vector2Int(gridCoordinateRudder.x, gridCoordinateRudder.y));
        //currentRudder.GetComponent<Item>().gridCoordinates = new Vector2Int(gridCoordinateRudder.x, gridCoordinateRudder.y);
        currentRudder = newRudder;

        // anchor
        GameObject newAnchor = Instantiate(anchor, new Vector3(anchorX, anchorY * 0.5f, 0)
            + anchor.GetComponent<Item>().gridToWorldOffset, Quaternion.identity);
        newAnchor.gameObject.GetComponent<Item>().gridCoordinates = new Vector2Int(anchorX, anchorY);

        Vector3Int gridCoordinateAnchor = gridLayout.WorldToCell(new Vector3(anchorX, anchorY, 0));
        GameManager.instance.occupiedTiles.Add(new Vector2Int(gridCoordinateAnchor.x, gridCoordinateAnchor.y));
        currentAnchor.GetComponent<Item>().gridCoordinates = new Vector2Int(gridCoordinateAnchor.x, gridCoordinateAnchor.y);
        currentAnchor = newAnchor;

        // compass
        GameObject newCompass = Instantiate(compass, new Vector3(compassX, compassY * 0.5f, 0)
            + compass.GetComponent<Item>().gridToWorldOffset, Quaternion.identity);
        newCompass.gameObject.GetComponent<Item>().gridCoordinates = new Vector2Int(compassX, compassY);

        Vector3Int gridCoordinateCompass = gridLayout.WorldToCell(new Vector3(compassX, compassY, 0));
        GameManager.instance.occupiedTiles.Add(new Vector2Int(gridCoordinateCompass.x, gridCoordinateCompass.y));
        currentCompass.GetComponent<Item>().gridCoordinates = new Vector2Int(gridCoordinateCompass.x, gridCoordinateCompass.y);
        currentCompass = newCompass;

    }

    private void SpawnShrooms(ItemType mushroomType)
    {
        int remainingMushrooms = 0;
        GameObject mushroomToSpawn;

        if (mushroomType == ItemType.mushroomGood)
        {
            remainingMushrooms = Random.Range(GameManager.instance.minGoodShroomsInLevel,
                GameManager.instance.maxGoodShroomsInLevel + 1);
            mushroomToSpawn = mushroomGood;
        }
        else if (mushroomType == ItemType.mushroomBad)
        {
            remainingMushrooms = Random.Range(GameManager.instance.maxBadShroomsInLevel,
                GameManager.instance.maxBadShroomsInLevel + 1);
            mushroomToSpawn = mushroomBad;
        }
        else //if (mushroomType == ItemType.mushroomLegendary)
        {
            remainingMushrooms = Random.Range(GameManager.instance.minLegendaryShroomsInLevel,
                GameManager.instance.maxLegendaryShroomsInLevel + 1);
            mushroomToSpawn = mushroomLegendary;
        }

        Vector3 itemOffset = mushroomToSpawn.GetComponent<Item>().gridToWorldOffset;
        while (remainingMushrooms > 0)
        {
            int xCoordinate = FindTileCoordinate();
            int yCoordinate = FindTileCoordinate(xCoordinate);

            if (!IsTileOccuppied(new Vector2Int(xCoordinate, yCoordinate)))
            {
                Vector3 worldCoordinate = new Vector3(xCoordinate, yCoordinate * 0.5f, 0) + itemOffset;
                Vector3Int gridCoordinate = gridLayout.WorldToCell(worldCoordinate);
                GameObject goodShroom = Instantiate(mushroomToSpawn, worldCoordinate, Quaternion.identity);
                goodShroom.gameObject.GetComponent<Item>().gridCoordinates = new Vector2Int(gridCoordinate.x, gridCoordinate.y);
                GameManager.instance.goodShrooms.Add(goodShroom);
                GameManager.instance.occupiedTiles.Add(new Vector2Int(xCoordinate, yCoordinate));
            }
            remainingMushrooms--;
        }
    }

    private bool IsTileOccuppied(Vector2Int xy)
    {
        bool isTileOccuppied = false;

        for (int i = 0; i < GameManager.instance.occupiedTiles.Count; i++)
        {
            if (GameManager.instance.occupiedTiles[i] == xy)
            {
                isTileOccuppied = true;
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
        Destroy(currentCompass);
        Destroy(currentAnchor);
        Destroy(currentRudder);
    }

    #region OBSTACLES
    [Header("OBSTACLES")]
    [SerializeField]
    GameObject treeGreen;
    float treeGreenChance = 2f;

    [SerializeField]
    GameObject treeTall;
    float treeTallChance = 4f;

    [SerializeField]
    GameObject bushDry;
    float bushDryChance = 0.8f;

    [SerializeField]
    GameObject rockBlue;
    float rockBlueChance = 1f;

    [SerializeField]
    GameObject twoRocks;
    float twoRocksChance = 0.4f;

    [SerializeField]
    GameObject oneRock;
    float oneRockChance = 0.4f;

    float chanceToPlaceObstacle = 0.13f;
    GameObject objectToInstantiate;

    public void RollChanceToPlaceObstacleOnTile(Vector3 position, Vector3Int gridPostion)
    {
        bool isTileOccupied = false;
        if (IsTileOccuppied(new Vector2Int((int)gridPostion.x, (int)gridPostion.y)))
        {
            isTileOccupied = true;
            return;
        }

        if (Random.Range(0f,1f) < chanceToPlaceObstacle)
        {
            GameManager.instance.occupiedTiles.Add(new Vector2Int((int)gridPostion.x, (int)gridPostion.y));

            float randomMax = bushDryChance + treeGreenChance + treeTallChance + rockBlueChance + twoRocksChance + oneRockChance;
            float randomRoll = Random.Range(0, randomMax);

            if (randomRoll < bushDryChance)
            {
                objectToInstantiate = bushDry;
                InstantiateObject(position, gridPostion);
                return;
            }
            else
            {
                randomRoll -= bushDryChance;
            }

            if (randomRoll < treeGreenChance)
            {
                objectToInstantiate = treeGreen;

                InstantiateObject(position, gridPostion);
                return;
            }
            else
            {
                randomRoll -= treeGreenChance;
            }

            if (randomRoll < treeTallChance)
            {
                objectToInstantiate = treeTall;

                InstantiateObject(position, gridPostion);
                return;
            }
            else
            {
                randomRoll -= treeTallChance;
            }

            if (randomRoll < rockBlueChance)
            {
                objectToInstantiate = rockBlue;

                InstantiateObject(position, gridPostion);
                return;
            }
            else
            {
                randomRoll -= rockBlueChance;
            }

            if (randomRoll < oneRockChance)
            {
                objectToInstantiate = oneRock;

                InstantiateObject(position, gridPostion);
                return;

            }
            else
            {
                randomRoll -= oneRockChance;
            }
            if (randomRoll < twoRocksChance)
            {
                objectToInstantiate = twoRocks;

                InstantiateObject(position, gridPostion);
                return;
            }
            else
            {

                InstantiateObject(position, gridPostion);
                return;
            }
        }
    }

    void InstantiateObject(Vector3 position, Vector3Int gridPosition)
    {
        GameObject newObject = Instantiate(objectToInstantiate, position, Quaternion.identity);
        newObject.GetComponent<Item>().gridCoordinates = new Vector2Int(gridPosition.x, gridPosition.y);
        GameManager.instance.occupiedTiles.Add(new Vector2Int(gridPosition.x, gridPosition.y));
    }


    #endregion
}
