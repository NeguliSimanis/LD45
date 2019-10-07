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

    private float hungerLevel = 5;
    private float remainingTime = 100;
    private float sanityLevel = 10;

    #region VICTORY
    public bool rudderFound = false;
    public bool compassFound = false;
    public bool anchorFound = false;

    [SerializeField]
    GameObject gameWinUI;
    #endregion

    #region DEFEAT
    [SerializeField]
    GameObject gameLoseUI;
    [SerializeField]
    Text gameLoseText;
    string hungerDefeat = "Ravaged by hunger, you perished in a land far from home.";
    string sanityDefeat = "Having lost everything you held dear in a merciless storm, you sought solace in the noxious mushrooms of the unknown land. It was a mistake.";
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

        tileSpawner = gameObject.GetComponent<TileSpawner>();
        tileSpawner.SetupTiles();

        itemSpawner = gameObject.GetComponent<ItemSpawner>();
        itemSpawner.SpawnItems();

        playerCurrentMoveSpeed = defaultPlayerMoveSpeed;

        sanityProgressBar.value = sanityLevel / 100;
        hungerProgressBar.value = hungerLevel / 100;

    }

    private void Update()
    {
        if (isGamePaused)
            return;
        remainingTime -= 0.01F;
        remainingTimeProgressBar.value = remainingTime / 100;
        
        if (remainingTime <= 0)
        {
            LoseGame(DefeatType.time);
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
        hungerLevel += amount;
        Debug.Log("adding hunger " + amount);
        if (hungerLevel <= 0)
        {
            //gameOver
            LoseGame(DefeatType.hunger);
        }

        hungerProgressBar.value = hungerLevel / 100;
    }

    public void AddToSanityLevel(int amount)
    {
        sanityLevel += amount;
        Debug.Log("adding sanity " + amount);
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
