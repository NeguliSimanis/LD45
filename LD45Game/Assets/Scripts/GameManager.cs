using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentLevelID = 0;

    public static GameManager instance = null;
    public int minGoodShroomsInLevel = 3;
    public int maxGoodShroomsInLevel = 9;
    public int minBadShroomsInLevel = 3;
    public int maxBadShroomsInLevel = 9;

    public int mapSizeX = 100;
    public int mapSizeY = 100;

    public bool isGamePaused = false;
    public float defaultPlayerMoveSpeed = 0.8f;
    public float playerRoadMoveSpeed = 1.6f;
    public float playerCurrentMoveSpeed;

    /// <summary>
    /// tiles that contain interactable objects
    /// </summary>
    public List<Vector2Int> occupiedTiles;
    public List<GameObject> goodShrooms;

    private ItemSpawner itemSpawner;
    private TileSpawner tileSpawner;
    [SerializeField]
    private PlayerController playerController;

    public Slider hungerProgressBar;
    public Slider remainingTimeProgressBar;
    public Slider sanityProgressBar;

    private float hungerLevel = 50;
    private float remainingTime = 100;
    private float sanityLevel = 100;

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

        playerCurrentMoveSpeed = defaultPlayerMoveSpeed;
        playerController.InitializePlayer();

        sanityProgressBar.value = sanityLevel / 100;
        hungerProgressBar.value = hungerLevel / 100;

    }

    private void Update()
    {
        remainingTime -= 0.01F;
        remainingTimeProgressBar.value = remainingTime / 100;
    }

    public void AddToHungerLevel(int amount)
    {
        hungerLevel += amount;
        if (hungerLevel >= 100)
        {
            //gameOver
        }

        hungerProgressBar.value = hungerLevel / 100;
    }

    public void AddToSanityLevel(int amount)
    {
        sanityLevel += amount;

        if (sanityLevel <= 0)
        {
            //gameOver
            return;
        }

        else if (sanityLevel > 100)
        {
            sanityLevel = 100;
        }

        sanityProgressBar.value = sanityLevel / 100;
    }
}
