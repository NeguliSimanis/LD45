using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public int minGoodShroomsInLevel = 3;
    public int maxGoodShroomsInLevel = 9;
    public int minBadShroomsInLevel = 3;
    public int maxBadShroomsInLevel = 9;

    public int mapSizeX = 7;
    public int mapSizeY = 7;

    public bool isGamePaused = false;

    /// <summary>
    /// tiles that contain interactable objects
    /// </summary>
    public List<Vector2Int> occupiedTiles;
    public List<GameObject> goodShrooms;

    private ItemSpawner itemSpawner;
    private TileSpawner tileSpawner;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Simanis")
        {
            SetupGameplayScene();
        }
    }

    private void SetupGameplayScene()
    {

        occupiedTiles.Clear();

        tileSpawner = gameObject.GetComponent<TileSpawner>();
        tileSpawner.SetupTiles();

        itemSpawner = gameObject.GetComponent<ItemSpawner>();
        itemSpawner.SpawnItems();
    }

    

}
