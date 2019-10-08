using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentLevelID = 0;

    public static GameManager instance = null;
    [SerializeField]
    AudioSource birdSounds;

    
    public AudioClip legendaryReaction;
    public AudioClip[] badMushroomReaction;

    #region MUSHROOMS
    [Header("MUSHROOM RULES")]
    // good
    public int minGoodShroomsInLevel;
    public int maxGoodShroomsInLevel;
    // bad
    public int minBadShroomsInLevel;
    public int maxBadShroomsInLevel;
    // legendary
    public int minLegendaryShroomsInLevel;
    public int maxLegendaryShroomsInLevel;
    #endregion

    [Header("MAP")]
    public int mapSizeX = 100;
    public int mapSizeY = 100;

    [HideInInspector]
    public bool isGamePaused = false;

    [Header("MOVEMENT")]
    public float defaultPlayerMoveSpeed = 0.8f;
    public float playerRoadMoveSpeed = 1.6f;
    public float playerCurrentMoveSpeed;

    [HideInInspector]
    /// <summary>
    /// tiles that contain interactable objects
    /// </summary>
    public List<Vector2Int> occupiedTiles;
    [HideInInspector]
    public List<GameObject> goodShrooms;

    private ItemSpawner itemSpawner;
    private TileSpawner tileSpawner;
    public PlayerController playerController;
    public bool playerEyesWork = false;

    #region PLAYER STATS
    [Header("PLAYER STATS")]
    public Slider hungerProgressBar;
    public Slider remainingTimeProgressBar;
    public Slider sanityProgressBar;
    [HideInInspector]
    public bool startReducingPlayerStats = false;
    private float sateLevel = 100;
    private float remainingTime = 100;
    private float sanityLevel = 100;
    [SerializeField]
    private float hungerIncreaseSpeed = 0.025f;
    #endregion


    #region VICTORY
    [Header("VICTORY")]
    public bool rudderFound = false;
    public bool compassFound = false;
    public bool anchorFound = false;

    [SerializeField]
    GameObject gameWinUI;
    #endregion

    #region DEFEAT
    [Header("DEFEAT")]
    [SerializeField]
    GameObject gameLoseUI;
    [SerializeField]
    Text gameLoseText;
    string hungerDefeat = "Ravaged by hunger, you perished in a land far from home.";
    string sanityDefeat = "Having lost everything you held dear in a merciless storm, you sought solace in the noxious mushrooms of the unknown land.\nIt was a mistake.";
    string timeDefeat = "You wasted away too much time wandering around the forest and the ship parts were carried away by animals and winds. Now you may never find the way back home.";
    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);
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

        //tileSpawner = gameObject.GetComponent<TileSpawner>();
        //tileSpawner.SetupTiles();

        itemSpawner = gameObject.GetComponent<ItemSpawner>();
        itemSpawner.SpawnItems();

        playerCurrentMoveSpeed = defaultPlayerMoveSpeed;

        sanityProgressBar.value = sanityLevel / 100;
        hungerProgressBar.value = sateLevel / 100;

    }

    public void EnterNextLevel()
    {
        isGamePaused = true;
        playerEyesWork = false;

        occupiedTiles.Clear();

        //tileSpawner = gameObject.GetComponent<TileSpawner>();
        //tileSpawner.SetupTiles();

        itemSpawner = gameObject.GetComponent<ItemSpawner>();
        itemSpawner.SpawnItems();

        playerCurrentMoveSpeed = defaultPlayerMoveSpeed;


        isGamePaused = false;
        playerEyesWork = true;
    }

    private void Update()
    {
        if (isGamePaused)
            return;

        if (startReducingPlayerStats)
        {
            remainingTime -= 0.01F;
            sateLevel -= hungerIncreaseSpeed;
            remainingTimeProgressBar.value = remainingTime / 100;
            hungerProgressBar.value = sateLevel / 100;

            if (sateLevel <= 0)
            {
                LoseGame(DefeatType.hunger);
            }

            if (remainingTime <= 0)
            {
                LoseGame(DefeatType.time);
            }
        }

        if (anchorFound && compassFound && rudderFound)
        {
            Debug.Log("VICTORY");
            gameWinUI.SetActive(true);
            isGamePaused = true;

        }
    }

    public void AddToHungerLevel(int amount)
    {
        sateLevel += amount;
        Debug.Log("adding hunger " + amount);
        if (sateLevel <= 0)
        {
            //gameOver
            LoseGame(DefeatType.hunger);
        }

        hungerProgressBar.value = sateLevel / 100;
    }

    public void AddToSanityLevel(int amount)
    {
        sanityLevel += amount;
        //Debug.Log("adding sanity " + amount);
        if (sanityLevel <= 0)
        {
            //gameOver
            LoseGame(DefeatType.sanity);
            return;
        }

        else if (sanityLevel > 100)
        {
            sanityLevel = 100;
        }

        sanityProgressBar.value = sanityLevel / 100;
    }

    public void UnPauseGame()
    {
        playerController.InitializePlayer();
        isGamePaused = false;
        birdSounds.enabled = true;
        
    }

    void LoseGame(DefeatType defeatType)
    {
        gameLoseUI.SetActive(true);
        isGamePaused = true;
        if (defeatType == DefeatType.hunger)
        {
            gameLoseText.text = hungerDefeat;
        }
        if (defeatType == DefeatType.sanity)
        {
            gameLoseText.text = sanityDefeat;
        }
        if (defeatType == DefeatType.time)
        {
            gameLoseText.text = timeDefeat;
        }
    }


}
